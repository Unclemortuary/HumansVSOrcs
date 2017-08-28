using System.Collections.Generic;
using UnityEngine;

public class CommandPanelManager : MonoBehaviour {

    [SerializeField]
    private const int commandsCount = 12;

    [SerializeField]
    private List<CommandSlot> commands = new List<CommandSlot> ();

    [SerializeField]
    private RTSActionType defaultAction = RTSActionType.moveTo;

    [SerializeField]
    private RTSActionType currentAction;

    public delegate void actionChangeHandler (RTSActionType action);

    public event actionChangeHandler ActionChanged;

    public RTSActionType CurrentAction { get { return currentAction; } }

    void Awake ()
    {
        InitCommandsList ();
        currentAction = RTSActionType.NULL;
    }

    void InitCommandsList ()
    {
        CommandSlot[] listItems = GetComponentsInChildren<CommandSlot> ();
        if (listItems.Length != commandsCount)
        {
            Debug.LogError ("Commands images count not appropriate. Check the command panel child gameObjects");
            commands = null;
        }
        else
        {
            for (int i = 0; i < commandsCount; i++)
            {
                commands.Add (listItems [i]);
                commands [i].Click += ActionChangeHandler;
            }
        }
    }

    void ActionChangeHandler (RTSActionType actionType, CommandSlot slot)
    {
        if (actionType == RTSActionType.NULL)
            return;

        var slotIndex = commands.IndexOf (slot);

//Deselect All actions at first
        for (int i = 0; i < commands.Count; i++)
            commands [i].DeselectAction ();

//Then Select/Deselect clicked action
//        if (currentAction.Equals (actionType))
//        {
//            commands [slotIndex].DeselectAction ();
//            currentAction = RTSActionType.NULL;
//            ActionChanged (currentAction);
//        }
//        else
        {
            commands [slotIndex].SelectAction ();
            currentAction = actionType;
            ActionChanged (currentAction);
        }
//Debug.Log ("Current action is " + actionType.ToString ());
    }


// Changing icons set here
    public void UpdatePanel (List<ActionData.ActionDataItem> list)
    {
        ClearPanel (true);

        for (int i = 0; i < list.Count; i++)
        {
            commands [i].SetCommand (list [i].Action, list [i].Icon);
//            if (list [i].Action == defaultAction)
//            {
//                commands [i].SelectAction ();
//                currentAction = defaultAction;
//                ActionChanged (currentAction);
//            }
        }
    }

    public void ClearPanel (bool intermediateCall)
    {
        for (int i = 0; i < commands.Count; i++)
            commands [i].UnsetCommand ();

//        if (!intermediateCall)
//        {
//            currentAction = RTSActionType.NULL;
//            ActionChanged (currentAction);
//        }
    }

}