using UnityEngine;

public class NeutralsController {

/////////////////////////////

    private ArmyManager thisArmyManager;

    private ArmyDispatcher ThisArmyDispatcher {
        get {
            return thisArmyManager.Dispatcher;
        }
    }


    public ArmyDispatcher PlayerArmyDispatcher {
        get {
            return GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].Dispatcher;
        }
    }

    public NeutralsController(ArmyManager armyManager) {
        SetArmyManager(armyManager);

        SubscribeOnDispatcherMessages();
    }



    private void SubscribeOnDispatcherMessages() {

        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("neutral unit is left clicked" + unit.Description);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("neutral unit is right clicked: " + unit.Description);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("Unit is dead: " + unit.Description);
                }
        );

    } // Subscribe on dispatcher messages //






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




}