using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfoPanelManager : MonoBehaviour {

	//private InfoText infoText;
	//private HPSliderHandler hpSliderHandler;
	//[SerializeField]
	//private ModelImage modelImage;

	public delegate void UpdateHandler(AbstractGameUnit unit);
	public delegate void DeselectHandler ();

	public event UpdateHandler Updated;
	public event DeselectHandler Deselect;

	/*
	void Start()
	{
		infoText = GetComponentInChildren<InfoText> ();
		hpSliderHandler = GetComponentInChildren<HPSliderHandler> ();
	}
	*/

	public void PanelUpdate(AbstractGameUnitsList units)
	{
		gameObject.SetActive (true);
		Updated (units [0]);
	}

	public void PanelDeselect()
	{
		Deselect ();
		gameObject.SetActive (false);
	}
}
