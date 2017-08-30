using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	private CommandPanelManager commandPanelManager;
	private ObjectInfoPanelManager objectInfoPanelManager;

    private ArmyDispatcher playerArmyDispatcher;
    private AbstractGameUnitsList selectedUnitsList;
    private Identification.Army playerArmy;

	void Start()
	{
		commandPanelManager = GetComponentInChildren<CommandPanelManager> ();
		objectInfoPanelManager = GetComponentInChildren<ObjectInfoPanelManager> ();
		objectInfoPanelManager.PanelDeselect ();
		commandPanelManager.ActionChanged += CurrentActionChanged;
	}

	public void InitHUD()
	{
		InitDispatcherMessages();
		playerArmy = GameManager.Instance.PlayerArmy;
	}

	private void InitDispatcherMessages() 
	{

		playerArmyDispatcher = GameManager.Instance.PlayerController.PlayerArmyDispatcher;

		playerArmyDispatcher.StartListening<AbstractGameUnitsList>(ArmyMessageTypes.selectionChanged,
			(AbstractGameUnitsList list) => {
				UpdateSelected(list);
			}
		);
		Debug.Log ("INIT DONE");
	}

	private void UpdateSelected(AbstractGameUnitsList list)
	{
		Debug.Log ("UpdateSelected call");
		this.selectedUnitsList = list;

		if (list.Count == 0)
		{
			List<RTSActionType> actionTypes = ConvertUnitsToActionTypes (list);
			CommandSetChangeHandler (actionTypes);
			objectInfoPanelManager.PanelUpdate (list);	
		}
		else
			ClearSelection ();
	}

	public void CommandSetChangeHandler(List<RTSActionType> groupActions)
	{
		var actionList = ConvertActionsToData (groupActions);
		commandPanelManager.UpdatePanel (actionList);
	}

	public void ClearSelection()
	{
		commandPanelManager.ClearPanel (false);
		objectInfoPanelManager.PanelDeselect ();
	}

	private List<ActionData.ActionDataItem> ConvertActionsToData(List<RTSActionType> actions)
	{
		List<ActionData.ActionDataItem> result = new List<ActionData.ActionDataItem> ();

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





		

    // Implementation with actions union //
    private List<RTSActionType> ConvertUnitsToActionTypes(AbstractGameUnitsList list) 
	{
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

	public RTSActionType ReturnCurrentAction()
	{
		return commandPanelManager.CurrentAction;
	}
}
