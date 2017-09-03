using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSlotInfo : MonoBehaviour {

	public ActionData myActionData;
	public GameObject slotMove;
	void Awake()
	{
		slotMove.SetActive (false);
	}

	public void SlotInfoEnter()
	{
		var slot = GetComponent<CommandSlot> ();
		//myActionData.ActionsData [slot.Command].PriceToUse [0];
		for(int i = 0; i < myActionData.ActionsData.Count ;i++)
		{
			if (myActionData.ActionsData [i].Action == slot.Command && (myActionData.ActionsData [i].Action.ToString().Contains("reate") 
				|| myActionData.ActionsData [i].Action.ToString().Contains("Build")))
			{		
				RectTransform slotTrans = slotMove.GetComponent<RectTransform> (); 
				slotTrans.position = new Vector3(Input.mousePosition.x - slotTrans.rect.width/2,Input.mousePosition.y + slotTrans.rect.height/2,slotTrans.position.z );

				slotMove.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = 
					" " + myActionData.ActionsData [i].PriceToUse.GetResourceAmount((GameResources.ResourceType.STONE));
				slotMove.gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = 
					" " + myActionData.ActionsData [i].PriceToUse.GetResourceAmount((GameResources.ResourceType.WOOD));
			
				slotMove.gameObject.transform.GetChild(5).gameObject.GetComponent<Text>().text = 
					" " + myActionData.ActionsData [i].PriceToUse.GetResourceAmount((GameResources.ResourceType.FOOD));
				slotMove.gameObject.transform.GetChild(7).gameObject.GetComponent<Text>().text = 
					" " + myActionData.ActionsData [i].PriceToUse.GetResourceAmount((GameResources.ResourceType.GOLD));
				slotMove.SetActive (true);
				break;
			}
		}
	}

	public void SlotInfoExit()
	{
		slotMove.SetActive (false);
	}
}
