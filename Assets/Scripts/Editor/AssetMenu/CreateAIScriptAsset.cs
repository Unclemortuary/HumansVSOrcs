using UnityEditor;
using UnityEngine;

public class CreateAIScriptAsset {

    [MenuItem("Assets/Create/CreateAIScriptAsset")]
    public static AIScript Create() {
        AIScript asset = ScriptableObject.CreateInstance<AIScript>();
        AssetDatabase.CreateAsset(asset, "Assets/AIScript.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

}