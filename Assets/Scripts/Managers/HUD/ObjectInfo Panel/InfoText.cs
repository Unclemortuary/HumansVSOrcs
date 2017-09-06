using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

	private ObjectInfoPanelManager ourManager;

	[SerializeField]
	private Text nameText;
	//[SerializeField]
	//private Text attackText;
	//[SerializeField]
	//private Text defenseText;
	[SerializeField]
	private Text attackValueText;
	[SerializeField]
	private Text defenseValueText;
	[SerializeField]
	private Text cooldownValueText;
	[SerializeField]
	private Text moveSpeedValueText;

	[SerializeField]
	private Image attackImage;
	[SerializeField]
	private Image defenceImage;
	[SerializeField]
	private Image cooldownImage;
	[SerializeField]
	private Image moveSpeedImage;

	private AbstractGameUnit selectedUnit = null;

	private bool isUnit = false;

	private List<Text> allText = new List<Text>();

	private List<Image> allImages = new List<Image>();

	void Awake()
	{
		allText.Add (nameText);
		//allText.Add (attackText);
		//allText.Add (defenseText);
		allText.Add (attackValueText);
		allText.Add (defenseValueText);
		allText.Add (moveSpeedValueText);
		allText.Add (cooldownValueText);

		allImages.Add (attackImage);
		allImages.Add (defenceImage);
		allImages.Add (moveSpeedImage);
		allImages.Add (cooldownImage);

		ourManager = GetComponentInParent<ObjectInfoPanelManager> ();

		ourManager.Updated += UpdateInfoPanel;
		ourManager.Deselect += ClearInfoText;

		ClearInfoText ();
	}

	void Update()
	{
		if (isUnit) 
		{
			attackValueText.text = selectedUnit.Characteristics.AttackPhisDamage.ToString();
			defenseValueText.text = (selectedUnit.Characteristics.Defence * 100f).ToString() + "%";
			moveSpeedValueText.text = selectedUnit.Characteristics.MaxMovingSpeed.ToString ();
			cooldownValueText.text = selectedUnit.Characteristics.AttackCooldownTime.ToString ();
		}
	}

	public void UpdateInfoPanel(AbstractGameUnit unit)
	{
		selectedUnit = unit;
		nameText.text = selectedUnit.Description;
		if (unit.Avatar.GetComponent<BuildingComponent>())
			return;
		else
		{
			isUnit = true;
			SetUnitImages ();
		}
	}

	private void SetUnitImages()
	{
		//attackText.text = "Attack";
		//defenseText.text = "Defence";

		for (int i = 0; i < allImages.Count; i++)
			allImages [i].enabled = true;
	}

	public void ClearInfoText()
	{
		selectedUnit = null;
		isUnit = false;
		for (int i = 0; i < allText.Count; i++)
			allText [i].text = "";
		for (int i = 0; i < allImages.Count; i++)
			allImages [i].enabled = false;
	}
}
