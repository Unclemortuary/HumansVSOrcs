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

	[SerializeField]
	private Image activeBorder;

    private Image commandImage;

	private RTSActionType command = RTSActionType.NULL;

    private bool isActionActive = false;

    private Color empty = new Color (0f, 0f, 0f, 0f);

    public RTSActionType Command { get { return command; } }

	GameResources myResources;
    void Awake()
    {
//commandImage = GetComponentInChildren<Image> ();
        commandImage = transform.GetChild(0).GetComponent<Image>();
		activeBorder = transform.GetChild(1).GetComponent<Image>();
		activeBorder.enabled = false;
        UnsetCommand ();
		myResources = new GameResources ();
    }

    public void SetCommand(RTSActionType command, Sprite sprite)
    {
        this.command = command;
        commandImage.color = new Color(commandImage.color.r, commandImage.color.g, commandImage.color.b, 1f);
        commandImage.sprite = sprite;
    }

    public void UnsetCommand()
    {
        commandImage.color = new Color (commandImage.color.r, commandImage.color.g, commandImage.color.b, 0f);
        commandImage.sprite = null;
        command = RTSActionType.NULL;
    }

	void ChangeColor()
	{
		myResources = GameManager.Instance.ArmyManagers[GameManager.Instance.PlayerArmy].AvailableResources;

	}

    public void SelectAction()
    {
		//activeBorder.enabled = true;	
    }

    public void DeselectAction()
    {
		//activeBorder.enabled = false;
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

