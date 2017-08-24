using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	private CommandPanelManager commandPanelManager;

    private ArmyDispatcher playerArmyDispatcher;
    private AbstractGameUnitsList selectedUnitsList;
    private Identification.Army playerArmy;

	void Start()
	{
		commandPanelManager = GetComponentInChildren<CommandPanelManager> ();
		commandPanelManager.ActionChanged += CurrentActionChanged;

		//***Test Actions***
		/*
		List<RTSActionType> testActions = new List<RTSActionType> ();
		testActions.Add (RTSActionType.moveTo);
		testActions.Add (RTSActionType.stop);
		CommandSetChangeHandler(testActions);
		//***End of Test Actions***
		*/

        InitDispatcherMessages();
        playerArmy = GameManager.Instance.PlayerArmy;
	}

	public RTSActionType ReturnCurrentAction()
	{
		return commandPanelManager.CurrentAction;
	}

	public void CommandSetChangeHandler(List<RTSActionType> groupActions)
	{
		var actionList = ConvertActionsToData (groupActions);
		commandPanelManager.UpdatePanel (actionList);
	}

	public void ClearCommandPanel()
	{
		commandPanelManager.ClearPanel (false);
	}

	private List<ActionData.SomeAction> ConvertActionsToData(List<RTSActionType> actions)
	{
		List<ActionData.SomeAction> result = new List<ActionData.SomeAction> ();

		for (int i = 0; i < actions.Count; i++) 
		{
			var concreteAction = GameManager.Instance.ActionsLibrary.GetRTSAction (actions [i]);
			result.Add (concreteAction.GetActionDataItem());
		}
		return result;
	}

	private void CurrentActionChanged(RTSActionType action)
	{
		GameManager.Instance.ArmyManagers[playerArmy]
                .Dispatcher.TriggerCommand<RTSActionType>(
                        ArmyMessageTypes.invokeRTSAction,
				        action
                );

		Debug.Log ("Current command is " + action.ToString ());
	}




    private void InitDispatcherMessages() {
        playerArmyDispatcher = GameManager.Instance.PlayerController.PlayerArmyDispatcher;

        playerArmyDispatcher.StartListening<AbstractGameUnitsList>(ArmyMessageTypes.selectionChanged,
                (AbstractGameUnitsList list) => {
                    UpdateSelected(list);
                }
        	);
    }

    private void UpdateSelected(AbstractGameUnitsList list) {
        this.selectedUnitsList = list;

        List<RTSActionType> actionTypes  = ConvertUnitsToActionTypes(list);

        CommandSetChangeHandler(actionTypes);
    }

    // Implementation with actions union //
    private List<RTSActionType> ConvertUnitsToActionTypes(AbstractGameUnitsList list) {
        List<RTSActionType> resultingList = new List<RTSActionType>();
		foreach (AbstractGameUnit unit in list) {
            List<RTSActionType> types = unit.ActionsList;


            if (types != null) {
                Debug.Log("this user have " + types.Count);

                foreach (RTSActionType actionType in types) {
                    Debug.Log("action:: " + actionType.ToString());

                    if (!resultingList.Contains(actionType)) {
                        resultingList.Add(actionType);
                    }
                }
            }
        }

        Debug.Log("Found " + resultingList.Count + " actions total");
        return  resultingList;
    }


}
