using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingComponent : RTSMonoBehaviour{

	private float buildingWidth = 8f;
	private float buildingHeight = 8f;

    private Renderer[] renderers;

	private List<Shader> originShaders;
	private List<Color> originColors;
	 
	public float BuildingWidth {get { return buildingWidth; } }
	public float BuildingHeight {get { return buildingHeight; } }

	void Awake()
	{
        base.Awake();

		InitRenderers();

		originShaders = new List<Shader>();
		foreach (Renderer rend in renderers)
			foreach (Material mat in rend.materials)
				originShaders.Add (mat.shader);

		originColors = new List<Color> ();
		foreach (Renderer rend in renderers)
			foreach (Material mat in rend.materials)
				originColors.Add (mat.color);
        
	}


    private void InitRenderers() 
	{
		renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetTransparent(bool val) {

		int i = 0;

        if(val) 
		{
			foreach (Renderer rend in renderers) 
			{
				foreach (Material mat in rend.materials)
				{
					mat.shader = Shader.Find ("Transparent/Diffuse");
					mat.color = new Color(originColors[i].r, originColors[i].g, originColors[i].b, 0.3f);
					i++;
				}
			}
        }
		else
		{
			foreach (Renderer rend in renderers) 
			{
				foreach (Material mat in rend.materials)
				{
					mat.shader = originShaders[i];
					mat.color = originColors[i];
					i++;
				}
			}
        }
    }

	public void ChangeGridMapStatus(Vector3 position)
	{
		int iOffset = (int) ((buildingWidth / (GridMapManager.instance.CellSize * 2f)) + 1);
		int jOffset = (int) ((buildingHeight / (GridMapManager.instance.CellSize * 2f)) + 1);

		int currentI = (int) (position.x / GridMapManager.instance.CellSize);
		int curretnJ = (int) (position.z / GridMapManager.instance.CellSize);

		GridMapManager.instance.StatusToOccupied (currentI - iOffset, curretnJ - jOffset, currentI + iOffset, curretnJ + jOffset);
	}


}