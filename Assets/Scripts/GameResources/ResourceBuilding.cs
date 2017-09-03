using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour {

	private GameResources resources;

	[SerializeField]
	private GameResources.ResourceType resourceType;

	private const int increasePerSecond = 1;
	[SerializeField]
	private float speed = 0.4f;
	private float value = 0f;

	private bool start = false;

	public GameResources.ResourceType ResourceType { get { return resourceType; } }

	void Start()
	{
		resources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;
	}

	void Update()
	{
		if (start) 
		{
			if (value <= (float)increasePerSecond) {
				value += speed * Time.deltaTime;
			}
			else
			{
				resources.ChangeResourceAmount (resourceType, (float)increasePerSecond);
				value = 0f;
			}
		}
			
	}

	public void StartMining()
	{
		start = true;
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log (name + " is on collision");
		if (collision.collider.tag.Equals (resourceType.ToString ()))
			Destroy (collision.collider.gameObject);
	}

	void OnCollisionStay(Collision collision)
	{
		Debug.Log (name + " is on collision");
		if (collision.collider.tag.Equals (resourceType.ToString ()))
			Destroy (collision.collider.gameObject);
	}
}
