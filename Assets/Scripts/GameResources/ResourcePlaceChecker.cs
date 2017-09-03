using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceBuilding))]
public class ResourcePlaceChecker : BuildingsConstructionHandler {

	protected override bool CanBuildCheck (Vector3 mousePosition, Collider other)
	{
		if (GetComponent<ResourceBuilding> ().ResourceType.Equals (GameResources.ResourceType.GOLD) ||
		   GetComponent<ResourceBuilding> ().ResourceType.Equals (GameResources.ResourceType.STONE))
		{
			if (other.tag.Equals (GetComponent<ResourceBuilding> ().ResourceType.ToString ()))
				return true;
			else 
				return false;
		}
		else
			return base.CanBuildCheck (mousePosition, other);
	}


}
