using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHandler : MonoBehaviour {

	[SerializeField]
	private Text progressText;

	[SerializeField]
	private Image background;
	[SerializeField]
	private Image fillImage;

	[SerializeField]
	private Slider progressBar;

	private ObjectInfoPanelManager ourManager;

	private AbstractGameUnit currentUnit;

	private ITaskerHelper helper;

	private BuildingProgressComponent buildingProgress;

	private bool checkBuildingProgress = false;

	void Awake()
	{
		background = transform.GetChild (0).GetComponent<Image> ();
		progressText = GetComponentInChildren<Text> ();
		progressBar = GetComponentInChildren<Slider> ();
		progressBar.onValueChanged.AddListener (delegate {ProgressBarValueChanged (); });
		progressBar.maxValue = 100f;
		progressBar.wholeNumbers = true;
		progressText.text = "";
		ourManager = GetComponentInParent<ObjectInfoPanelManager> ();
		ourManager.Updated += ProgressBarUpdate;
		ourManager.Deselect += Deselect;
		Enable (false);
	}

	void Update()
	{
		if (currentUnit != null)
		{
			if (currentUnit.IsActive)
				ActionFinished ();
			else
			{
				if (checkBuildingProgress)
				{
					if (currentUnit.Avatar.GetComponent<BuildingProgressComponent> ())
					{
						buildingProgress = currentUnit.Avatar.GetComponent<BuildingProgressComponent> ();
						if (buildingProgress.Finished)
							ActionFinished ();
						else
						{
							Enable (true);
							ShowProgress (buildingProgress.Helper.TaskRemaintinTime, buildingProgress.Helper.TaskDuration);
						}
					}
				}
				else
				{
					Enable (true);
					if (helper.TaskRemaintinTime > 0f)
						ShowProgress (helper.TaskRemaintinTime, helper.TaskDuration);
					else
						ActionFinished ();
				}
			}
		}
	}

	private void ShowProgress(float nominator, float denominator)
	{
		float percent = (1f - (nominator / denominator)) * 100f;
		progressBar.value = percent;
		Debug.Log (nominator);
		if (percent < progressBar.maxValue + float.Epsilon && percent > progressBar.maxValue - float.Epsilon)
			ActionFinished ();
	}

	private void ProgressBarValueChanged()
	{
		progressText.text = progressBar.value.ToString ();
		progressText.text += "%";
	}

	public void ProgressBarUpdate(AbstractGameUnit unit)
	{
		progressBar.value = 0f;
		currentUnit = unit;
		helper = unit.Avatar.GetComponent<UnitStateMachine> ().TaskerHelper;
		if (helper != null)
		{
			if (unit.Avatar.GetComponent<BuildingComponent> ())
			{
				checkBuildingProgress = true;
			}
		}
		else
			Debug.LogError (unit.Avatar.name + " helper is null!");
		Debug.Log ("Progress bar updated; checkBuildingProgress = " + checkBuildingProgress);
	}

	private void Enable(bool value)
	{
		background.enabled = value;
		progressText.enabled = value;
		fillImage.enabled = value;
	}

	private void ActionFinished()
	{
		checkBuildingProgress = false;
		buildingProgress = null;
		Enable (false);
	}

	public void Deselect()
	{
		Enable (false);
		currentUnit = null;
		helper = null;
	}
}
