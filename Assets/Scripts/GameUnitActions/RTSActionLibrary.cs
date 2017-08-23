using System.Collections.Generic;

public class RTSActionLibrary {

    private Dictionary<RTSActionType, AbstractRTSAction> actions;

    public RTSActionLibrary() {
        actions = new Dictionary<RTSActionType, AbstractRTSAction>();
    }

    public void AddRTSAction(RTSActionType type, AbstractRTSAction action) {
        actions.Add(type, action);
    }

    public AbstractRTSAction GetRTSAction(RTSActionType type) {
        return actions[type];
    }

    public bool Contains(RTSActionType type) {
        return actions.ContainsKey(type);
    }

}