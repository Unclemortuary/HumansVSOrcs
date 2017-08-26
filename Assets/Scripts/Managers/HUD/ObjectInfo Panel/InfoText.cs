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

	public void UpdateInfoPanel(AbstractGameUnit unit)
	{
		nameText.text = unit.Description;
		if (unit.Avatar.GetComponent<BuildingComponent> ())
			return;
		else
		{
			
		}
	}

	public void ClearInfoText()
	{
		for (int i = 0; i < allText.Count; i++)
			allText [i].text = "";
	}
}
