using UnityEngine;

public class NeutralsController : AbstractContorller {

/////////////////////////////

    private ArmyManager thisArmyManager;

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


    // Constructor //////////////////////////////////
    public NeutralsController(ArmyManager armyManager) {
        SetArmyManager(armyManager);

        SubscribeOnDispatcherMessages();
    }



    private void SubscribeOnDispatcherMessages() {

        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryLeftClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("neutral unit is left clicked" + unit.Description);

                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryRightClicked,
                (AbstractGameUnit unit) => {
                    Debug.Log("neutral unit is right clicked: " + unit.Description);

                    TellPlayerThatUnitWasClicked(unit);
                }
        );


        ThisArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.unitCryIsDead,
                (AbstractGameUnit unit) => {
                    Debug.Log("Neutrals Controller:: Unit is dead: " + unit.Description);

                    thisArmyManager.DestroyGameUnit(unit.ID);
                }
        );

    } // Subscribe on dispatcher messages //



    // Send message to PlayerController to tell it "some other unit is clicked" //
    private void TellPlayerThatUnitWasClicked(AbstractGameUnit unit) {
        Debug.Log("NeutralsController:: Sending message about a click to player");
        GameManager.Instance.PlayerController.NewTargetUnit(unit);
    }



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