using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimpleScriptableListCreate {

	[MenuItem ("Assets/Create/SimpleScriptableList")]
	public static SimpleScriptableList Create ()
	{
		SimpleScriptableList asset = ScriptableObject.CreateInstance<SimpleScriptableList> ();
		AssetDatabase.CreateAsset (asset, "Assets/ScriptableObjectsData/SimpleScriptableList.asset");
		AssetDatabase.SaveAssets ();
		return asset;
	}
}
