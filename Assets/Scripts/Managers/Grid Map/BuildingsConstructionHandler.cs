using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsConstructionHandler : RTSMonoBehaviour {

	private float buildingWidth = 3f;
	private float buildingHeight = 3f;
	private float cellSize;

	private Shader originShader;
	private Color originColor;
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
		settableBuilding.gameObject.GetComponent<Collider> ().enabled = false;
		renderers = GetComponentsInChildren<Renderer> ();
		originShader = renderers [0].material.shader;
		originColor = renderers [0].material.color;
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
			{
				renderer.material.shader = originShader;
				renderer.material.color = originColor;
			}
			settableBuilding.gameObject.GetComponent<Collider> ().enabled = true;
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
