public class NeutralsController {

/////////////////////////////

    private ArmyManager armyManager;


    public ArmyDispatcher PlayerArmyDispatcher {
        get {
            return armyManager.Dispatcher;
        }
    }

    public NeutralsController(ArmyManager armyManager) {
        SetArmyManager(armyManager);
    }

    public void SetArmyManager(ArmyManager manager) {
        this.armyManager = manager;

        SetUnitsToStandPreparedToFight();
    }

    private void SetUnitsToStandPreparedToFight() {
        // Make all units waiting for ocasion to fight //

        foreach(AbstractGameUnit unit in armyManager.Warriors) {
            PlayerArmyDispatcher.TriggerCommand(ArmyMessageTypes.unitCommandGoToStandPreparedState, unit.ID);
        }
    }


}