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
		RTSActionType myType = slot.Command;
		if(myType != RTSActionType.NULL)
		{
				slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image> ().color = new Color32(255,255,255,255);
				if (GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.STONE)) > myResources.GetResourceAmount (GameResources.ResourceType.STONE)
					|| GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.GOLD)) > myResources.GetResourceAmount (GameResources.ResourceType.GOLD)
					|| GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.WOOD))> myResources.GetResourceAmount (GameResources.ResourceType.WOOD)
					|| GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.FOOD)) > myResources.GetResourceAmount (GameResources.ResourceType.FOOD)) 
				{
					slot.gameObject.transform.GetChild (0).gameObject.GetComponent<Image> ().color =  new Color32(25,25,25,255);
					
				}
			 else 
				{
					slot.gameObject.transform.GetChild(0).gameObject.GetComponent<Image> ().color = new Color32(255,255,255,255);
				}
			}
	}

	public void SlotInfoEnter()
	{
		var slot = GetComponent<CommandSlot> ();
		RTSActionType myType = slot.Command;
		if(myType != RTSActionType.NULL)
		{
			if ((GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).ToString().Contains("reate") 
				|| GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).ToString().Contains("Build")))
			{		
				RectTransform slotTrans = slotMove.GetComponent<RectTransform> (); 
				slotTrans.position = new Vector3(Input.mousePosition.x - slotTrans.rect.width/2,Input.mousePosition.y + slotTrans.rect.height/2,slotTrans.position.z );

				slotMove.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = 
			" " + GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.STONE));
				slotMove.gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = 
			" " + GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.WOOD));
			
				slotMove.gameObject.transform.GetChild(5).gameObject.GetComponent<Text>().text = 
			" " + GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.FOOD));
				slotMove.gameObject.transform.GetChild(7).gameObject.GetComponent<Text>().text = 
			" " + GameManager.Instance.ActionsLibrary.GetRTSAction( myType ).GetActionDataItem().PriceToUse.GetResourceAmount((GameResources.ResourceType.GOLD));
				slotMove.SetActive (true);
			}
		}
	}

	public void SlotInfoExit()
	{
		slotMove.SetActive (false);
	}
}
