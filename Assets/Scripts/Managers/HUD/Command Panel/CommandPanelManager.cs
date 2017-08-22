using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Commands
{
	public class CommandPanelManager : MonoBehaviour
	{
		[SerializeField]
		private const int commandsCount = 12;

		[SerializeField]
		private List<CommandSlot> commands = new List<CommandSlot> ();

		[SerializeField]
		private RTSActionType defaultAction = RTSActionType.moveTo;

		[SerializeField]
		private RTSActionType currentAction;

		public RTSActionType CurrentAction { get { return currentAction; } }

		void Start ()
		{
			InitCommandsList ();
			currentAction = RTSActionType.NULL;
		}

		void InitCommandsList ()
		{
			CommandSlot[] listItems = GetComponentsInChildren<CommandSlot> ();
			if (listItems.Length != commandsCount) {
				Debug.LogError ("Commands images count not appropriate. Check the command panel child gameObjects");
				commands = null;
			} else {
				for (int i = 0; i < commandsCount; i++) {
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

			if (currentAction.Equals (actionType)) {
				commands [slotIndex].DeselectAction ();
				currentAction = defaultAction;
			} else {
				commands [slotIndex].SelectAction ();
				currentAction = actionType;
			}
			Debug.Log ("Current action is " + actionType.ToString ());
		}


		// Changing icons set here
		public void UpdatePanel (List<ActionData.SomeAction> list)
		{
			for (int i = 0; i < commandsCount; i++)
			{
				commands [i].SetCommand (list [i].Action, list [i].Icon);
			}
		}

		public void ClearPanel ()
		{
			for (int i = 0; i < commands.Count; i++)
				commands [i].UnsetCommand ();

			currentAction = RTSActionType.NULL;
		}
	}
}
