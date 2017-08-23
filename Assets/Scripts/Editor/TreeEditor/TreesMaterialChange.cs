using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreesMaterialChange : Editor {

	GameObject[] trees;
	public SimpleScriptableList mat;
	public  void ArrayGreate()
	{
		trees = GameObject.FindGameObjectsWithTag ("TestTree");
	}

	public  void TreeChange(GameObject tree)
	{
		tree.GetComponent<Renderer> ().material = mat.myMaterial [Random.Range (0, 4)];
	}

	public void MyMain()
	{
		ArrayGreate ();
		for (int i = 0; i < trees.Length; i++) {
			TreeChange (trees [i]);
		}
	}
}

