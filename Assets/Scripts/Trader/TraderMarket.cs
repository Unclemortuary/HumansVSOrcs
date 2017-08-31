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
		Exchange (GameResources.ResourceType.GOLD, 10, GameResources.ResourceType.FOOD, 10);
	}

	public void BuyWood()
	{
		Exchange (GameResources.ResourceType.GOLD, 15, GameResources.ResourceType.WOOD, 10);
	}

	public void BuyStone()
	{
		Exchange (GameResources.ResourceType.GOLD, 20, GameResources.ResourceType.STONE, 10);
	}

	//-------------------SELL-------------------------

	public void SellFood()
	{
		Exchange (GameResources.ResourceType.FOOD, 10, GameResources.ResourceType.GOLD, 5);
	}

	public void SelLWood()
	{
		Exchange (GameResources.ResourceType.WOOD, 10, GameResources.ResourceType.GOLD, 7);
	}

	public void SellStone()
	{
		Exchange (GameResources.ResourceType.STONE, 10, GameResources.ResourceType.GOLD, 9);
	}

	private void Exchange(GameResources.ResourceType fromRes, int fromCount
		, GameResources.ResourceType toRes, int toCount) {
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
		if (myResources.GetResourceAmount (fromRes) >= fromCount) {
			myResources.ChangeResourceAmount (fromRes, -fromCount);
			myResources.ChangeResourceAmount (toRes, toCount);
		}
	}
}
