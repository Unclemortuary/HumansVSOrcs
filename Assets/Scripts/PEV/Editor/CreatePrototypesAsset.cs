using UnityEditor;
using UnityEngine;

public class CreatePrototypesAsset {

    [MenuItem("Assets/Create/UnitPrototypes")]
    public static ScriptablePrototypesDictionaries Create() {
        ScriptablePrototypesDictionaries asset = ScriptableObject.CreateInstance<ScriptablePrototypesDictionaries>();
        AssetDatabase.CreateAsset(asset, "Assets/UnitPrototypes.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
