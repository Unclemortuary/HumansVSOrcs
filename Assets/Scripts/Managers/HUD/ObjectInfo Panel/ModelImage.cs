﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelImage : MonoBehaviour {

	[SerializeField]
	private ObjectInfoPanelManager ourManager;

	[SerializeField]
	private Transform spotPoint;

	[SerializeField]
	private GameObject currentUnit;

	void Start()
	{
		spotPoint = transform;

		ourManager.Updated += ModelImageUpdate;
		ourManager.Deselect += ClearModelImage;
	}

	public void ModelImageUpdate(AbstractGameUnit unit)
	{
		ClearModelImage ();
		Vector3 position;
		if (unit.Avatar.GetComponent<BuildingComponent> ())
			position = spotPoint.position + new Vector3 (0f, 0f, -15f);
		else
			position = spotPoint.position;
		currentUnit = Instantiate (unit.Avatar, position, Quaternion.identity, spotPoint);
	}

	public void ClearModelImage()
	{
		Destroy (currentUnit);
		//for (int i = 0; i < transform.childCount; i++) 
		//{
			//Destroy(transform.GetChild(i).gameObject);
		//}
	}
}
