public class StopAction : AbstractRTSAction {




    public StopAction(ActionData.SomeAction actionData) {
        this.actionData = actionData;
    }

    //////////////////

    public override void Doing(ArmyStateData data) {
        data.ThisArmyManager.StateMachine.Trigger(ArmySMTransitionType.doActionToSelected);
    }


    public override void Stopping(ArmyStateData data) {
    }

    public override void Starting(ArmyStateData data) {
        foreach (AbstractGameUnit unit in data.SelectedUnits) {
            data.ThisArmyManager.Dispatcher.TriggerCommand(ArmyMessageTypes.unitCommandStop, unit.ID);
        }
    }
}