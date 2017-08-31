using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderMarket : MonoBehaviour {

	GameResources myResources;

	void Start () {
		myResources = new GameResources ();
	}
	

	//------------------Buy--------------------------
	public void BuyFood()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, -10);
		myResources.ChangeResourceAmount (GameResources.ResourceType.FOOD, 10);
	}

	public void BuyWood()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, -15);
		myResources.ChangeResourceAmount (GameResources.ResourceType.WOOD, 10);
	}

	public void BuyStone()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, -20);
		myResources.ChangeResourceAmount (GameResources.ResourceType.STONE, 10);
	}

	//-------------------SELL-------------------------

	public void SellFood()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, 5);
		myResources.ChangeResourceAmount (GameResources.ResourceType.FOOD, -10);
	}

	public void SelLWood()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, 7);
		myResources.ChangeResourceAmount (GameResources.ResourceType.WOOD, -10);
	}

	public void SellStone()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		myResources.ChangeResourceAmount (GameResources.ResourceType.GOLD, 9);
		myResources.ChangeResourceAmount (GameResources.ResourceType.STONE, -10);
	}
}
