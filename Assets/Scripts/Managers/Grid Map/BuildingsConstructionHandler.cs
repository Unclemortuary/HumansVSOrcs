using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsConstructionHandler : MonoBehaviour {

	private float buildingWidth = 3f;
	private float buildingHeight = 3f;
	private float cellSize;

	private GridMapManager gmManager;

	private bool canBuild;

	void Start()
	{
		gmManager = GetComponent<GridMapManager> ();
		cellSize = gmManager.CellSize;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray mRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (mRay, out hit, 1000f))
				canBuild = CanBuildCheck (hit.point);
			else
				canBuild = false;
			if (canBuild)
				Debug.Log ("Can Build Here!");
			else
				Debug.Log ("Cannot Build Here");
		}
	}

	bool CanBuildCheck(Vector3 mousePosition)
	{
		int iOffset = (int) ((buildingWidth / (cellSize * 2f)) + 1);
		int jOffset = (int) ((buildingHeight / (cellSize * 2f)) + 1);

		Debug.Log (iOffset + " - iOffset;" + jOffset + " - jOffset");

		int currentI = (int) (mousePosition.x / cellSize);
		int curretnJ = (int) (mousePosition.z / cellSize);

		Debug.Log ("currentI is " + currentI + " currentJ is" + curretnJ);

		return gmManager.MapArrayCheck (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}
}
