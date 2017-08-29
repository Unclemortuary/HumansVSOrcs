using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelImage : MonoBehaviour {

	[SerializeField]
	private Transform spotPoint;

	[SerializeField]
	private GameObject currentUnit;

	void Start()
	{
		spotPoint = transform;
	}

	public void ModelImageUpdate(AbstractGameUnit unit)
	{
		currentUnit = Instantiate (unit.Avatar, spotPoint.position, Quaternion.identity, spotPoint);
	}

	public void ClearModelImage()
	{
		Destroy (currentUnit);
	}
}
