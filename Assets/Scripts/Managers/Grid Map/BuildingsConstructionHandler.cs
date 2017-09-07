using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsConstructionHandler : MonoBehaviour {

	private float buildingWidth;
	private float buildingHeight;
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

	protected void Start()
	{
		cellSize = GridMapManager.instance.CellSize;
		settableBuilding = GetComponent<BuildingComponent> ();
		buildingWidth = settableBuilding.BuildingWidth;
		buildingHeight = settableBuilding.BuildingHeight;
		Collider[] colliders = settableBuilding.gameObject.GetComponents<Collider> ();
		foreach (Collider collider in colliders)
			collider.enabled = false;
		renderers = GetComponentsInChildren<Renderer> ();
	}

	protected void Update()
	{
		if (settableBuilding != null)
		{
			Color currentColor;
			Ray mRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast (mRay, out hit, 1000f))
			{
				canBuild = CanBuildCheck (hit.point, hit.collider);
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
				foreach (Material mat in renderer.materials) 
				{
					mat.shader = Shader.Find ("Nature/Tree Creator Leaves");
					mat.color = currentColor;
				}

			}
		}
	}


	protected virtual bool CanBuildCheck(Vector3 mousePosition, Collider other)
	{
		int iOffset = (int) ((buildingWidth / (cellSize * 2f)) + 1);
		int jOffset = (int) ((buildingHeight / (cellSize * 2f)) + 1);

		int currentI = (int) (mousePosition.x / cellSize);
		int curretnJ = (int) (mousePosition.z / cellSize);

		return GridMapManager.instance.MapArrayCheck (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}

	public static bool CanBuildCheck(Vector3 position)
	{
		int iOffset = (int) ((10f / (GridMapManager.instance.CellSize * 2f)) + 1);
		int jOffset = (int) ((10f / (GridMapManager.instance.CellSize * 2f)) + 1);

		int currentI = (int) (position.x / GridMapManager.instance.CellSize);
		int curretnJ = (int) (position.z / GridMapManager.instance.CellSize);

		return GridMapManager.instance.MapArrayCheck (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}

	public static bool CanBuildCheck(Vector3 position, float width, float height)
	{
		int iOffset = (int) ((width / (GridMapManager.instance.CellSize * 2f)) + 1);
		int jOffset = (int) ((height / (GridMapManager.instance.CellSize * 2f)) + 1);

		int currentI = (int) (position.x / GridMapManager.instance.CellSize);
		int curretnJ = (int) (position.z / GridMapManager.instance.CellSize);

		return GridMapManager.instance.MapArrayCheck (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}


}
