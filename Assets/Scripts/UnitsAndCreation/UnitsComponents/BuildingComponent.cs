using UnityEngine;

public class BuildingComponent : RTSMonoBehaviour{

	private Shader originShader;
	private Color originColor;

	public Shader OriginShader { get { return originShader; } set { originShader = value; } }
	public Color OriginColor {get { return originColor; } set { originColor = value; } }

	void Awake()
	{
		originShader = GetComponentsInChildren<Renderer> ()[0].material.shader;
		originColor = GetComponentsInChildren<Renderer> ()[0].material.color;
	}
}