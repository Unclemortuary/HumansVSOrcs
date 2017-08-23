using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsConstructionHandler : MonoBehaviour {

	private float buildingWidth = 3f;
	private float buildingHeight = 3f;
	private float cellSize;

	private Color greenTransparentColor = new Color (0f, 0.8f, 0f, 0.3f);
	private Color redTransparentColor = new Color (0.8f, 0f, 0f, 0.3f);
	private Renderer[] renderers;

	[SerializeField]
	private BuildingComponent settableBuilding;

	private bool canBuild;
    public bool CanBuild {
        get {
            return canBuild;
        }
    }

	void Start()
	{
		cellSize = GridMapManager.instance.CellSize;
		settableBuilding = GetComponent<BuildingComponent> ();
		Collider[] colliders = settableBuilding.gameObject.GetComponents<Collider> ();
		foreach (Collider collider in colliders)
			collider.enabled = false;
		renderers = GetComponentsInChildren<Renderer> ();
	}

	void Update()
	{
		if (settableBuilding != null) 
		{
			Color currentColor;
			Ray mRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (mRay, out hit, 1000f)) 
			{
				canBuild = CanBuildCheck (hit.point);
				settableBuilding.transform.position = hit.point;
			}
			else
				canBuild = false;
			
			if (canBuild)
				currentColor = greenTransparentColor;
			else
				currentColor = redTransparentColor;
			
			foreach (Renderer renderer in renderers) 
			{
				renderer.material.shader = Shader.Find ("Transparent/Diffuse");
				renderer.material.color = currentColor;
			}

			if (Input.GetMouseButton (0))
				BuildUp (hit.point);
		}
	}

	void BuildUp(Vector3 position)
	{
		if (canBuild) 
		{
			settableBuilding.transform.position = position;
			foreach (Renderer renderer in renderers) 
				renderer.material.color = new Color (settableBuilding.OriginColor.r, settableBuilding.OriginColor.g, settableBuilding.OriginColor.b, 0.5f);

			Collider[] colliders = settableBuilding.gameObject.GetComponents<Collider> ();
			foreach (Collider collider in colliders)
				collider.enabled = true;

			//***Animations of building and Timer class start must invoke here***

			Destroy (this);
		}
		else 
		{
			Debug.Log ("Cannot build here!");
		}
	}

	bool CanBuildCheck(Vector3 mousePosition)
	{
		int iOffset = (int) ((buildingWidth / (cellSize * 2f)) + 1);
		int jOffset = (int) ((buildingHeight / (cellSize * 2f)) + 1);

		int currentI = (int) (mousePosition.x / cellSize);
		int curretnJ = (int) (mousePosition.z / cellSize);

		return GridMapManager.instance.MapArrayCheck (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}
}
