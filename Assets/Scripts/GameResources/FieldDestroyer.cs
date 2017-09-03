using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDestroyer : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
		ResourceBuilding resourceBuilding = collision.collider.GetComponent<ResourceBuilding> ();
		if (resourceBuilding != null) 
		{
			if (tag.Equals (resourceBuilding.ResourceType.ToString ()))
				Destroy (this.gameObject);	
		}
	}
}
