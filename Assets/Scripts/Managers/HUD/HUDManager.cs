﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	private CommandPanelManager commandPanelManager;
	private ObjectInfoPanelManager objectInfoPanelManager;

    private ArmyDispatcher playerArmyDispatcher;
    private AbstractGameUnitsList selectedUnitsList;
    private Identification.Army playerArmy;

	private int multiSelectIndex = 0;

	void Start()
	{
		commandPanelManager = GetComponentInChildren<CommandPanelManager> ();
		objectInfoPanelManager = GetComponentInChildren<ObjectInfoPanelManager> ();
		objectInfoPanelManager.PanelDeselect ();
		commandPanelManager.ActionChanged += CurrentActionChanged;

        selectedUnitsList = new AbstractGameUnitsList();

        GameManager.Instance.HereIsHud(this);
	}

	public void InitHUD()
	{
		InitDispatcherMessages();
		playerArmy = GameManager.Instance.PlayerArmy;
	}


	void Update()
	{
		if (CheckDeadUnits ()) 
		{
			if (selectedUnitsList.Count == 1)
				ClearSelection ();
		}
		if (selectedUnitsList.Count > 1) 
		{
			if(Input.GetKeyUp("tab"))
			{
				multiSelectIndex++;
				if (multiSelectIndex == selectedUnitsList.Count)
					multiSelectIndex = 0;
				Debug.Log ("MULTISELECT index is " + multiSelectIndex);
				objectInfoPanelManager.PanelUpdate (selectedUnitsList[multiSelectIndex]);
			}
		}
	}


	private void InitDispatcherMessages() 
	{

		playerArmyDispatcher = GameManager.Instance.PlayerController.PlayerArmyDispatcher;

		playerArmyDispatcher.StartListening<AbstractGameUnitsList>(ArmyMessageTypes.selectionChanged,
			(AbstractGameUnitsList list) => {
				UpdateSelected(list);
			}
		);

		playerArmyDispatcher.StartListening<AbstractGameUnit>(ArmyMessageTypes.enemyUnitSelected,
			(AbstractGameUnit unit) => {
				ShowEnemyInformation(unit);
			}
		);

		playerArmyDispatcher.StartListening(ArmyMessageTypes.refreshSelection,
			() => {
				RefreshActionsList();
			}
		);


	}

    private void ShowEnemyInformation(AbstractGameUnit unit) {
        Debug.Log ("HUDManager : tryint to show enemy info");

        AbstractGameUnitsList unitsList = new AbstractGameUnitsList();
		unitsList.Add(unit);

        this.selectedUnitsList = unitsList;

		List<RTSActionType> actionTypes = new List<RTSActionType>();
		CommandSetChangeHandler (actionTypes);
		objectInfoPanelManager.PanelUpdate (this.selectedUnitsList);
    }

	private void UpdateSelected(AbstractGameUnitsList list)
	{
		Debug.Log ("HUDManager : UpdateSelected call, units number=" + list.Count);
		this.selectedUnitsList = list;
		RefreshActionsList();
	}

    private void RefreshActionsList()
	{
		for (int i = 0; i < selectedUnitsList.Count; i++)
		{
			if (selectedUnitsList [i].IsDead ())
			{
				selectedUnitsList.Remove (selectedUnitsList [i]);
			}
		}

        if (this.selectedUnitsList.Count != 0)
        {
			List<RTSActionType> actionTypes;
			if (CheckIfSameUnit ())
				actionTypes = ConvertUnitsToActionTypes (this.selectedUnitsList);
			else
				actionTypes = UniteActions (selectedUnitsList);
			CommandSetChangeHandler (actionTypes);
			objectInfoPanelManager.PanelUpdate (this.selectedUnitsList);
        }
        else {
            ClearSelection ();
        }
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
		multiSelectIndex = 0;
	}

	private List<ActionData.ActionDataItem> ConvertActionsToData(List<RTSActionType> actions)
	{
		List<ActionData.ActionDataItem> result = new List<ActionData.ActionDataItem> ();

		for (int i = 0; i < actions.Count; i++) 
		{
			AbstractRTSAction concreteAction = GameManager.Instance.ActionsLibrary.GetRTSAction (actions [i]);
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

	private bool CheckDeadUnits()
	{
		for (int i = 0; i < selectedUnitsList.Count; i++)
			if (selectedUnitsList [i].IsDead ())
				return true;
		return false;
	}





		

    // Implementation with actions union //
    private List<RTSActionType> ConvertUnitsToActionTypes(AbstractGameUnitsList list) 
	{


        List<RTSActionType> resultingList = new List<RTSActionType>();
		foreach (AbstractGameUnit unit in list) {

            if (unit.IsActive) {
                List<RTSActionType> types = unit.Characteristics.ActionsList;

                if (types != null) {
//                Debug.Log("this user have " + types.Count);

                    foreach (RTSActionType actionType in types) {
//                    Debug.Log("action:: " + actionType.ToString());

                        if (!resultingList.Contains(actionType)) {
                            resultingList.Add(actionType);
                        }
                    }
                }
            } // if unit is active //

        }

        Debug.Log("Found " + resultingList.Count + " actions total");
        return  resultingList;
    }

	private List<RTSActionType> UniteActions(AbstractGameUnitsList list)
	{
		List<RTSActionType> input = new List<RTSActionType> ();

		for (int i = 0; i < list.Count; i++) 
		{
			for (int j = 0; j < list[i].Characteristics.ActionsList.Count; j++)
			{
				input.Add (list [i].Characteristics.ActionsList [j]);
			}
		}

		List<RTSActionType> result = input.GroupBy (x => x).Where (g => g.Count() > 1).Select (g => g.Key).ToList ();
		return result;
	}

	private bool CheckIfSameUnit()
	{
		var prevUnitActions = selectedUnitsList [0].Characteristics.ActionsList;
		List<RTSActionType> currentUnitActions;
		for (int i = 0; i < selectedUnitsList.Count; i++) 
		{
			currentUnitActions = selectedUnitsList [i].Characteristics.ActionsList;
			if (currentUnitActions != prevUnitActions)
				return false;
			prevUnitActions = currentUnitActions;
		}
		return true;
	}

	public RTSActionType ReturnCurrentAction()
	{
		return commandPanelManager.CurrentAction;
	}
}
