using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInterface : MonoBehaviour {

	public void NewGame()
	{
		Application.LoadLevel("First");
	}

	public void Exit()
	{
		Application.Quit();
	}
}
