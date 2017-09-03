using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSliderHandler : MonoBehaviour {

	/*
	private ObjectInfoPanelManager ourManager;

	private Slider slider;

	[SerializeField]
	private Image fillImage;

	[SerializeField]
	private Text HPText;

	private Color minHPColor = Color.red;
	private Color maxHPColor = Color.green;

	private AbstractGameUnit selectedUnit = null;
	[SerializeField]
	private UnitStateMachineHelper helper = null;

	void Awake()
	{
		ourManager = GetComponentInParent<ObjectInfoPanelManager> ();

		slider = GetComponent<Slider> ();
		slider.interactable = false;
		//slider.wholeNumbers = true;
		slider.onValueChanged.AddListener(delegate {HPValueChanged(); });

		ourManager.Updated += UpdateSlider;
		ourManager.Deselect += ClearSlider;

		ClearSlider ();
	}

	void FixedUpdate()
	{
		if (selectedUnit != null) 
		{
			if ((helper.TaskRemaintinTime < 0f + float.Epsilon && helper.TaskRemaintinTime > 0f + float.Epsilon) || helper.TaskRemaintinTime > 0f)
			{
				SliderColorToEmpty (true);
				float percents = (1 - helper.TaskRemaintinTime / helper.TaskDuration) * 100f;
				HPText.text = percents.ToString();
			}
			else
				slider.value = selectedUnit.CurrentHP;
		}
	}

	public void UpdateSlider(AbstractGameUnit unit)
	{
		selectedUnit = unit;
		helper = selectedUnit.Avatar.GetComponent<UnitStateMachine> ().Helper;
        if(selectedUnit != null) 
		{
            slider.maxValue = selectedUnit.Characteristics.MaxHP;
            if (slider.value < 1f)
                SliderColorToEmpty (true);
            else
                SliderColorToEmpty (false);
        } else {
            ClearSlider();
        }
	}

	public void HPValueChanged()
	{
        if(selectedUnit != null) {
            HPText.text = selectedUnit.CurrentHP.ToString () + " / " + selectedUnit.Characteristics.MaxHP.ToString ();
            fillImage.color = Color.Lerp (minHPColor, maxHPColor, slider.normalizedValue);
            if (slider.value < 1f)
                SliderColorToEmpty (true);
        }
	}

	public void ClearSlider()
	{
		selectedUnit = null;
		HPText.text = "";
		slider.maxValue = 0f;
		slider.value = 0f;
	}

	private void SliderColorToEmpty(bool value)
	{
		if (value)
		{
			Color empty = new Color (fillImage.color.r, fillImage.color.g, fillImage.color.b, 0f);
			fillImage.color = empty;
		}
		else
		{
			Color full = new Color (fillImage.color.r, fillImage.color.g, fillImage.color.b, 1f);
			fillImage.color = full;	
		}
	}
	*/
}
