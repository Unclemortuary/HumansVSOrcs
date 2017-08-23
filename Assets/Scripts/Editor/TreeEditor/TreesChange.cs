using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreesChange : Editor {

	GameObject[] trees;

	void ArrayGreate()
	{
		trees = GameObject.FindGameObjectsWithTag ("TestTree");
	}

	void TreeChange(GameObject tree)
	{
		//tree.transform.localScale =  new Vector3 (tree.transform.localScale.x + Random.Range(2,5), tree.transform.localScale.y + Random.Range(2,5),tree.transform.localScale.z + Random.Range(2,5));
		//tree.transform.localPosition =  new Vector3 (tree.transform.localPosition.x + Random.Range(-5,5), tree.transform.localPosition.y ,tree.transform.localPosition.z + Random.Range(-5,5));
		int myXZ = Random.Range(3,5);
		//tree.transform.position =  new Vector3 (tree.transform.position.x + Random.Range(-1,1), tree.transform.position.y ,tree.transform.position.z + Random.Range(-1,1));
		tree.transform.localScale =  new Vector3 (myXZ, Random.Range(2,6),myXZ);
	}

	public void MyMain()
	{
		ArrayGreate ();
		for (int i = 0; i < trees.Length; i++) {
			TreeChange (trees [i]);
		}
	}
}
