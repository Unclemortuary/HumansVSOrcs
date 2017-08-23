﻿using UnityEditor;
using UnityEngine;

public class CreateActionDataAsset
{
	[MenuItem ("Assets/Create/Actions")]
	public static ActionData Create ()
	{
		ActionData asset = ScriptableObject.CreateInstance<ActionData> ();
		AssetDatabase.CreateAsset (asset, "Assets/ScriptableObjectsData/ActionsData.asset");
		AssetDatabase.SaveAssets ();
		return asset;
	}
}

