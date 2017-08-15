using UnityEditor;
using UnityEngine;

public class CreatePrototypesAsset {

    [MenuItem("Assets/Create/PrototypesList")]
    public static ScriptablePrototypesLists Create() {
        ScriptablePrototypesLists asset = ScriptableObject.CreateInstance<ScriptablePrototypesLists>();
        AssetDatabase.CreateAsset(asset, "Assets/PrototypesList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
