using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActionData : ScriptableObject {

    [System.Serializable]
    public class ActionDataItem
    {
        [SerializeField]
        string name;

        [SerializeField]
        RTSActionType action;

        [SerializeField]
        Sprite icon;

        [SerializeField]
        private float timeToComplete;

        public float TimeToComplete {
            get {
                return timeToComplete;
            }
        }

        [SerializeField]
        private GameResources priceToUse;
        public GameResources PriceToUse {
            get {
                return priceToUse;
            }
        }

        public RTSActionType Action { get { return action; } }

        public Sprite Icon { get { return icon; } }

        public ActionDataItem(string name, RTSActionType action, Sprite icon)
        {
            this.name = name;
            this.action = action;
            this.icon = icon;
        }
    } // End of SomeAction //


    [SerializeField]
    private List<ActionDataItem> actionsData;
    public List<ActionDataItem> ActionsData {
        get {
            return actionsData;
        }
    }

	/*
	public SomeAction getSomeAction(RTSActionType type)
	{
		for (int i = 0; i < actionsData.Count; i++) 
		{
			if (actionsData [i].Action == type)
				return actionsData [i];	
		}
		return null;
	}
	*/

} // End of class //


