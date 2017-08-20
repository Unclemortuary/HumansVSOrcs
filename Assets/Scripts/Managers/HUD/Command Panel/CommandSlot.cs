using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommandSlot : MonoBehaviour, IClickable, IPointerClickHandler {

	public delegate void SelectHandler(RTSActionType actionType, CommandSlot slot);

	public event SelectHandler Click;

	[SerializeField]
	private PointerEventData.InputButton workableButton = PointerEventData.InputButton.Left;

	private Image commandImage;

	private RTSActionType command;

	private bool isActionActive = false;

	private Color empty = new Color (0f, 0f, 0f, 0f);

	void Start()
	{
		commandImage = GetComponentInChildren<Image> ();
		UnsetCommand ();
	}

	public void SetCommand(RTSActionType command ,Sprite sprite)
	{
		this.command = command;
		Color color = commandImage.color;
		color.a = 1f;
		commandImage.sprite = sprite;
	}

	public void UnsetCommand()
	{
		commandImage.color = empty;
		commandImage.sprite = null;
		command = RTSActionType.NULL;
	}

	public void SelectAction()
	{
		//Makes some visual staff here
	}

	public void DeselectAction()
	{
		//Makes some visual staff here
	}

	#region IClickable implementation

	public void Clicked ()
	{
		Click (command, this);
	}

	#endregion


	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button.Equals (workableButton))
			Clicked ();
	}

	#endregion
}
