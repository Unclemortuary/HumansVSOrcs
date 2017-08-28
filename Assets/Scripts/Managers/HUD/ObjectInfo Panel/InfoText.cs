using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

	[SerializeField]
	private Text nameText;
	[SerializeField]
	private Text attackText;
	[SerializeField]
	private Text defenseText;
	[SerializeField]
	private Text attackValueText;
	[SerializeField]
	private Text defenseValueText;

	private AbstractGameUnit selectedUnit = null;

	private bool isUnit = false;

	private List<Text> allText = new List<Text>();

	void Start()
	{
		allText.Add (nameText);
		allText.Add (attackText);
		allText.Add (defenseText);
		allText.Add (attackValueText);
		allText.Add (defenseValueText);

		ClearInfoText ();
	}

	void Update()
	{
		if (isUnit) 
		{
			//attackValueText.text = selectedUnit.Characteristics.Attack.ToString()
			defenseValueText.text = selectedUnit.Characteristics.Defence.ToString();
		}
	}

	public void UpdateInfoPanel(AbstractGameUnit unit, bool isBuilding)
	{
		selectedUnit = unit;
		nameText.text = selectedUnit.Description;
		if (isBuilding)
			return;
		else
		{
			isUnit = true;
			SetUnitText ();
		}
	}

	private void SetUnitText()
	{
		attackText.text = "Attack";
		defenseText.text = "Defence";
	}

	public void ClearInfoText()
	{
		selectedUnit = null;
		isUnit = false;
		for (int i = 0; i < allText.Count; i++)
			allText [i].text = "";
	}
}
