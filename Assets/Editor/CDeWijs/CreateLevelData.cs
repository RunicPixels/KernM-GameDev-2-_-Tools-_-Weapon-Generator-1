using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateLevelData {

	[MenuItem("Assets/Create/Color Mapping List")]
    public static LevelData Create() {
        // Create new scriptable object
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();
        
        AssetDatabase.CreateAsset(asset, "Assets/Level Data.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
