public abstract class AbstractRTSAction {

    public abstract CommandPanelItem GetCommandPanelItem(ArmyStateData data);

    public abstract void Starting(ArmyStateData data);

    public abstract void Doing(ArmyStateData data);

    public abstract void Stopping(ArmyStateData data);

}