public abstract class AbstractRTSAction {

    protected ActionData.SomeAction actionData;

    public ActionData.SomeAction GetActionDataItem() {
        return actionData;
    }

    public void SetActionDataItem(ActionData.SomeAction adata) {
        actionData = adata;
    }

    /////////////////////////////

    public abstract void Starting(ArmyStateData data);

    public abstract void Doing(ArmyStateData data);

    public abstract void Stopping(ArmyStateData data);

}