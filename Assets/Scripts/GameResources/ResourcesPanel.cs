using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Project.TimeManager;

public class ResourcesPanel : MonoBehaviour {

	GameResources myResources;
	public Text myGold;
	public Text myStone;
	public Text myWood;
	public Text myFood;
	public Text myPeople;
	public Text dayTime;
	public GameManager myManager;
	TimeManager myTimeManager;
	public GameObject timeImage;
	public GameObject timeText;
	Quaternion startRotationText;
	// Use this for initialization
	void Start () {
		myResources = new GameResources ();
		startRotationText = timeText.transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;

		GoldDraw ();
		StoneDraw ();
		WoodDraw ();
		FoodDraw ();
		PeopleHousesDraw ();
		TimeDraw ();
		RotateDayTime ();
	}

	void GoldDraw()
	{
		myGold.text = "" + myResources.GetResourceAmount (GameResources.ResourceType.GOLD)+ " ";
	}

	void StoneDraw()
	{
		myStone.text = "" + myResources.GetResourceAmount (GameResources.ResourceType.STONE)+ " ";
	}

	void WoodDraw()
	{
		myWood.text = "" + myResources.GetResourceAmount (GameResources.ResourceType.WOOD) + " ";
	}

	void FoodDraw()
	{
		myFood.text = "" + myResources.GetResourceAmount (GameResources.ResourceType.FOOD) + " ";
	}

	void PeopleHousesDraw()
	{
		myPeople.text = "" + myResources.GetResourceAmount (GameResources.ResourceType.MEN) + "/" + myResources.GetLivingPlacesNumber()+ " ";
	}

	void TimeDraw()
	{
		int hour = (int)(myManager.GetComponent<TimeManager> ()._currTime * 24);
		float minute = myManager.GetComponent<TimeManager> ()._currTime * 24 - hour;
		minute = minute * 60 / 100;
		minute = (int)(minute * 100);
		string myMinute = minute.ToString ();
		string myHour = hour.ToString ();
		if (myMinute.Length < 2)
			myMinute = "0" + myMinute;
		if (myHour.Length < 2)
			myHour = "0" + myHour;
		dayTime.text = " " +  myHour + ":" + myMinute + " ";
	}

	void RotateDayTime()
	{
		timeImage.transform.Rotate (new Vector3 (0, 0, -1) * Time.deltaTime * 360/myManager.GetComponent<TimeManager> ()._fullDay);
		timeText.transform.rotation = startRotationText;
	}
		
}
