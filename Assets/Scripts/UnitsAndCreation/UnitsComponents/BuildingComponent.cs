using UnityEngine;

public class BuildingComponent : RTSMonoBehaviour{

	private float buildingWidth = 8f;
	private float buildingHeight = 8f;

    private Renderer[] renderers;

	private Shader originShader;
	private Color originColor;

    private Renderer renderer0;

	public Shader OriginShader { get { return originShader; } set { originShader = value; } }
	public Color OriginColor {get { return originColor; } set { originColor = value; } }
	public float BuildingWidth {get { return buildingWidth; } }
	public float BuildingHeight {get { return buildingHeight; } }

	void Awake()
	{
        base.Awake();

        renderer0 = GetComponentsInChildren<Renderer> ()[0];

		originShader = renderer0.material.shader;
		originColor = renderer0.material.color;

        InitRenderers();
	}


    private void InitRenderers() {
        renderers = GetComponents<Renderer>();
    }

    public void SetTransparent(bool val) {


        if(val) {
            Debug.Log("Trying to make transparent");
            renderer0.material.color = new Color(originColor.r, originColor.g, originColor.b, 0.3f);
        } else {
            renderer0.material.color = originColor;
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