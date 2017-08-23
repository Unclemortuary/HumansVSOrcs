using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActionData : ScriptableObject {

    [System.Serializable]
    public class SomeAction
    {
        [SerializeField]
        string name;

        [SerializeField]
        RTSActionType action;

        [SerializeField]
        Sprite icon;

        public RTSActionType Action { get { return action; } }

        public Sprite Icon { get { return icon; } }

        public SomeAction(string name, RTSActionType action, Sprite icon)
        {
            this.name = name;
            this.action = action;
            this.icon = icon;
        }
    }

    [SerializeField]
    private List<SomeAction> actionsData;
    public List<SomeAction> ActionsData {
        get {
            return actionsData;
        }
    }
}
