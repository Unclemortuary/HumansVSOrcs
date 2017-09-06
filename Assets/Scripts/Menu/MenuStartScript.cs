using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStartScript : MonoBehaviour {

	// Use this for initialization
	public GameObject startMenu;
	public GameObject hud;
	public GameObject cameraHolder;
	public GameObject panelStart;
	public GameObject panelChoose;
	public GameObject human;
	public GameObject orc;
	bool timeState = false;
	public Animator meshProText;
	public Animator panel;
	public GameObject weather;

	void Awake()
	{
		startMenu.SetActive (false);
		hud.SetActive (true);
		cameraHolder.SetActive (true);
	}

	void Start () {
		startMenu.SetActive (true);
		hud.SetActive (false);
		cameraHolder.SetActive (false);
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			meshProText.SetBool ("EndState", true);
			panel.SetBool("EndState", true);
		}
	}

	public void RaceChoose()
	{
		panelStart.SetActive (false);
		panelChoose.SetActive (true);
		human.SetActive (true);
		orc.SetActive (true);
	}

	public void StartGameHumans()
	{
        CloseMainMenu();
        GameManager.Instance.StartGameWithPlayerArmyThe(Identification.Army.Humans);
		weather.SetActive (true);
		RenderSettings.fog = true;
    }

	public void StartGameOrcs()
	{
		CloseMainMenu();
        GameManager.Instance.StartGameWithPlayerArmyThe(Identification.Army.Orcs);
		weather.SetActive (true);
		RenderSettings.fog = true;
    }

	private void CloseMainMenu() {
        startMenu.SetActive (false);
        hud.SetActive (true);
        cameraHolder.SetActive (true);
    }


	public void Exit()
	{
		Application.Quit();
	}

	public void TimeChange()
	{
		timeState = !timeState;
		if (timeState) {
			Time.timeScale = 0.2f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
