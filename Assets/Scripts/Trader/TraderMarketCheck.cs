using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TraderMarketCheck : MonoBehaviour {

	public GameObject market;
	float speed = 0.0f;
	public void MarketOpen()
	{
		//var nav = GetComponent<NavMeshAgent> (); 
		//speed = nav.speed;
		//nav.speed = 0;
		market.SetActive (true);
	}

	public void MarketClose()
	{
		//var nav = GetComponent<NavMeshAgent> (); 
		//nav.speed = speed;
		market.SetActive (false);
	}
}
