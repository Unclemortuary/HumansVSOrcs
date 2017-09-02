using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : MonoBehaviour {

	[SerializeField]
	private GameResources.ResourceType recourseType;

	public GameResources.ResourceType RecourseType { get { return recourseType; } }
}
