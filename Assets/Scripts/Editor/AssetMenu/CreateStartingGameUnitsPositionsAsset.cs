using UnityEditor;
using UnityEngine;

public class CreateStartingGameUnitsPositionsAsset {

    [MenuItem("Assets/Create/StartingGameUnitsPositions")]
    public static StartingGamePositions Create() {
        StartingGamePositions asset = ScriptableObject.CreateInstance<StartingGamePositions>();
        AssetDatabase.CreateAsset(asset, "Assets/StartingGameUnitsPositions.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

}