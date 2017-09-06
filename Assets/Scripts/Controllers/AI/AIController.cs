using System.Collections.Generic;
using UnityEngine;

public class AIController {


    public class BuildingsDictionary : Dictionary<Identification.UnitType, List<AbstractGameUnit>> {}

    private AIScript behaviourScript;

    public void Update() {
        // update information about number of buildings and units //

        // if have enough units, send them to attack one of enemyPoints //

        // if have enough resources and do not have enough buildings, Build //
        // if worker is unavailable, create worker

        // if have enough resources and living places, create warrior //
        //
        // if barracks unavailable, skip the step //

    }

///////////////////////////////////////////////////////////////////////////////////////////////////////

    private ArmyManager thisArmyManager;
    public ArmyManager ThisArmyManager {
        get {
            return thisArmyManager;
        }
    }

    private ArmyDispatcher ThisArmyDispatcher {
        get {
            return thisArmyManager.Dispatcher;
        }
    }


    private ArmyDispatcher PlayerArmyDispatcher {
        get {
            return GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].Dispatcher;
        }
    }
    private ArmyManager PlayerArmyManager {
        get {
            return GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy];
        }
    }


// Constructor ///////////////////////////////////////////////////////////////////////////////////////////

    public AIController(ArmyManager armyManager, AIScript script) {

        this.behaviourScript = script;

        SetArmyManager(armyManager);

        GameResources resources = new GameResources(400, 400, 400, 400);
        thisArmyManager.AvailableResources.AddResources(resources);

        SubscribeOnDispatcherMessages();
    }


// ################################################################
    public void SetArmyManager(ArmyManager manager) {
        this.thisArmyManager = manager;

        SetUnitsToStandPreparedToFight();
    }

    private void SetUnitsToStandPreparedToFight() {
// Make all units waiting for ocasion to fight //

        foreach(AbstractGameUnit unit in thisArmyManager.Warriors) {
            ThisArmyDispatcher.TriggerCommand(ArmyMessageTypes.unitCommandGoToStandPreparedState, unit.ID);
        }

    }
// ###############################################################


    private void SubscribeOnDispatcherMessages() {

        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked,
                (AbstractGameUnit unit) => {
                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("AI Controller:: Unit is dead: " + unit.Description);

                    thisArmyManager.DestroyGameUnit(unit.ID);
                }
        );

    } // Subscribe on dispatcher messages //



// Send message to PlayerController to tell it "some other unit is clicked" //
    private void TellPlayerThatUnitWasClicked(AbstractGameUnit unit) {
        Debug.Log("AI:: Sending message about a click to player");
        GameManager.Instance.PlayerController.NewTargetUnit(unit);
    }




}