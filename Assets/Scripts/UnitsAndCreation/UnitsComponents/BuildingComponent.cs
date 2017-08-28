using UnityEngine;

public class BuildingComponent : RTSMonoBehaviour{

    private Renderer[] renderers;

	private Shader originShader;
	private Color originColor;

    private Renderer renderer0;

	public Shader OriginShader { get { return originShader; } set { originShader = value; } }
	public Color OriginColor {get { return originColor; } set { originColor = value; } }

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


}