public class StopAction : AbstractRTSAction {



    CommandPanelItem panelItem;

    public StopAction(CommandPanelItem panelItem) {
        this.panelItem = panelItem;
    }

    public override CommandPanelItem GetCommandPanelItem(ArmyStateData data) {
        return panelItem;
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