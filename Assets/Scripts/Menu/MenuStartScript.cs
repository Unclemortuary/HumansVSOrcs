using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStartScript : MonoBehaviour {

	// Use this for initialization
	public GameObject startMenu;
	public GameObject hud;
	public GameObject cameraHolder;
	bool timeState = false;
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

	public void StartGame()
	{
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
