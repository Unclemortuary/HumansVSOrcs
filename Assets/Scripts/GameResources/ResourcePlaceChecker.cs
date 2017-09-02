using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceBuilding))]
public class ResourcePlaceChecker : BuildingsConstructionHandler {

	protected override bool CanBuildCheck (Vector3 mousePosition, Collider other)
	{
		if (other.tag.Equals (GetComponent<ResourceBuilding> ().RecourseType.ToString ()))
			return true;
		else
			return false;
	}
}
