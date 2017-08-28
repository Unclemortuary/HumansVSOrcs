using UnityEditor;
using UnityEngine;

public class CreateStartingGameUnitsPositionsAsset {

    [MenuItem("Assets/Create/StartingGameUnitsPositions")]
    public static StartingGameUnitsPositions Create() {
        StartingGameUnitsPositions asset = ScriptableObject.CreateInstance<StartingGameUnitsPositions>();
        AssetDatabase.CreateAsset(asset, "Assets/StartingGameUnitsPositions.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

}