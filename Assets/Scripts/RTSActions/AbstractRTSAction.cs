public abstract class AbstractRTSAction {

    protected ActionData.ActionDataItem actionData = null;

    public ActionData.ActionDataItem GetActionDataItem() {
        return actionData;
    }

    public void SetActionDataItem(ActionData.ActionDataItem adata) {
        actionData = adata;
    }

    /////////////////////////////

    public abstract void Starting(ArmyStateData data);

    public abstract void Doing(ArmyStateData data);

    public abstract void Stopping(ArmyStateData data);

}