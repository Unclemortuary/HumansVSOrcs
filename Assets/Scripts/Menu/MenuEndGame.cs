using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuEndGame : MonoBehaviour {

	public GameObject endCanvas;
	public Text endText;

	public void EndMenuActive()
	{
		endCanvas.SetActive (true);
	}

	public void TextString(string text)
	{
		endText.text = "" + text;
		if (endText.text.Contains ("Vict")) {
			endText.color = Color.green;	
		} else {
			endText.color = Color.red;
		}
			
	}

	public void Exit()
	{
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Exit();
	}
}
