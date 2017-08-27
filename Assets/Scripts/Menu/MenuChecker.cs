using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuChecker : MonoBehaviour {

	// Use this for initialization

	bool menuState = true;

	public GameObject hud;
	public GameObject menuInterface;

	void Awake () {
		Time.timeScale = 1.0f;
	}

	public void MenuStateChange()
	{
		menuState = !menuState;
		if (menuState) {
			hud.SetActive (true);
			menuInterface.SetActive (false);
			Time.timeScale = 1.0f;
		} else {
			hud.SetActive (false);
			menuInterface.SetActive (true);
			Time.timeScale = 0.0f;
		}
	}
}
