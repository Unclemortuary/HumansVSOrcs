using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSlotInfo : MonoBehaviour {

	public ActionData myActionData;
	public GameObject slotMove;
	GameResources myResources;
	void Start()
	{
		myResources = new GameResources ();
		//slotMove.SetActive (false);
	}

	void Update()
	{
		ChangeColor ();
	}

	//ChangeColor if we don't have enought resources
	public void ChangeColor()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		var slot = GetComponent<CommandSlot> ();
		//myActionData.ActionsData [slot.Command].PriceToUse [0];
		for (int i = 0; i < myActionData.ActionsData.Count; i++) 
		{
			
			if (myActionData.ActionsData [i].Action == slot.Command)// && (myActionData.ActionsData [i].Action.ToString().Contains("reate") 
				//|| myActionData.ActionsData [i].Action.ToString().Contains("Build")))
			{		
				slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image> ().color = new Color32(255,255,255,255);
				if (myActionData.ActionsData [i].PriceToUse.GetResourceAmount (GameResources.ResourceType.STONE) > myResources.GetResourceAmount (GameResources.ResourceType.STONE)
				    || myActionData.ActionsData [i].PriceToUse.GetResourceAmount (GameResources.ResourceType.GOLD) > myResources.GetResourceAmount (GameResources.ResourceType.GOLD)
				    || myActionData.ActionsData [i].PriceToUse.GetResourceAmount (GameResources.ResourceType.WOOD) > myResources.GetResourceAmount (GameResources.ResourceType.WOOD)
				    || myActionData.ActionsData [i].PriceToUse.GetResourceAmount (GameResources.ResourceType.FOOD) > myResources.GetResourceAmount (GameResources.ResourceType.FOOD)) 
				{
					slot.gameObject.transform.GetChild (0).gameObject.GetComponent<Image> ().color =  new Color32(25,25,25,255);
					//Debug.Log ("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
				}
			 else 
				{
					slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image> ().color = new Color32(255,255,255,255);
				}
				break;
			}
		}
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
