using System.Collections.Generic;

public class RTSActionLibrary {

    private Dictionary<RTSActionType, AbstractRTSAction> actions;

    public void AddRTSAction(RTSActionType type, AbstractRTSAction action) {
        actions.Add(type, action);
    }

    public AbstractRTSAction GetRTSAction(RTSActionType type) {
        return actions[type];
    }
}