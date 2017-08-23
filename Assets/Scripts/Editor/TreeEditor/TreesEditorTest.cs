using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreesEditorTest : EditorWindow {

	//TreesChange myTrees;
	TreesMaterialChange myTreesMaterial;
	string treeButton = "Trees";
	[MenuItem ("Window/TreesGenerate")]
	static void Init () {
		TreesEditorTest window = (TreesEditorTest)EditorWindow.GetWindow(typeof(TreesEditorTest));
	}


	//public static void  ShowWindow () {
		//EditorWindow.GetWindow(typeof(TreesEditorTest));
	//}

	void OnGUI () {
		if(GUILayout.Button(treeButton))
		{
			//myTrees = new TreesChange ();
			//myTrees.MyMain ();
			myTreesMaterial = new TreesMaterialChange();
			myTreesMaterial.MyMain ();
		}
	}

}
