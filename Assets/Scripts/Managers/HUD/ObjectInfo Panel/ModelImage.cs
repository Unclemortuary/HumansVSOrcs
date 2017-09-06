using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelImage : MonoBehaviour {

	[SerializeField]
	private ObjectInfoPanelManager ourManager;

	[SerializeField]
	private Transform spotPoint;
	[SerializeField]
	private Image backround;

	//private ParticleSystem.MainModule fireParticles;

	private Color originColor;
	private Color humansColor;
	private Color orcsColor;
	private Color neutralsColor;

	[SerializeField]
	private GameObject currentUnit;

	void Start()
	{
		//fireParticles = transform.GetChild (0).GetComponent<ParticleSystem> ().main;
		//originColor = fireParticles.startColor.color;
		humansColor = new Color (0.1f, 0.15f, 0.3f, 0.95f);
		orcsColor = new Color (0.3f, 0.15f, 0.15f, 0.95f);
		neutralsColor = new Color (0.2f, 0.2f, 0.2f, 0.95f);

		spotPoint = transform;

		ourManager.Updated += ModelImageUpdate;
		ourManager.Deselect += ClearModelImage;
	}

	public void ModelImageUpdate(AbstractGameUnit unit)
	{
		ClearModelImage ();
		Vector3 position;
		Vector3 rotation;

		if (unit.Avatar.GetComponent<UnitStateMachine> ().EnemyHelper.MyArmy == Identification.Army.Humans) 
		{
			backround.color = humansColor;
		}
			//fireParticles.startColor = new ParticleSystem.MinMaxGradient (humansColor);
		if(unit.Avatar.GetComponent<UnitStateMachine> ().EnemyHelper.MyArmy == Identification.Army.Orcs)
		{
			backround.color = orcsColor;
		}
		if(unit.Avatar.GetComponent<UnitStateMachine> ().EnemyHelper.MyArmy == Identification.Army.Neutrals)
		{
			backround.color = neutralsColor;
		}
			//fireParticles.startColor = new ParticleSystem.MinMaxGradient (originColor);

		if (unit.Avatar.GetComponent<BuildingComponent> ())
		{
			position = spotPoint.position + new Vector3 (0f, -10f, -20f);
			rotation = new Vector3 (0f, -40f, 0f);
		}
		else
		{
			position = spotPoint.position;
			rotation = Vector3.zero;
		}
			
		currentUnit = Instantiate (unit.Characteristics.AvatarPrefab, position, Quaternion.Euler(rotation), spotPoint);
	}

	public void ClearModelImage()
	{
		Destroy (currentUnit);
	}
}
