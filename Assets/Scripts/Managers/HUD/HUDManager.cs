using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	private CommandPanelManager commandPanelManager;

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
		GameManager.Instance.ArmyManagers[Identification.Army.Humans]
			.Dispatcher.TriggerCommand<RTSActionType>(ArmyMessageTypes.invokeRTSAction,
				action);

		Debug.Log ("Current command is " + action.ToString ());
	}
}
