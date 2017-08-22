using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Commands;

public class HUDManager : MonoBehaviour {

	private CommandPanelManager commandPanelManager;


	void Start()
	{
		commandPanelManager = GetComponentInChildren<CommandPanelManager> ();
	}

	public RTSActionType ReturnCurrentAction()
	{
		return commandPanelManager.CurrentAction;
	}

	public void CommandSetChangeHandler()
	{
		//commandPanelManager.UpdatePanel ();
	}

	public void ClearCommandPanel()
	{
		
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
}
