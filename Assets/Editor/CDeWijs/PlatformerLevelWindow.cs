using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlatformerLevelWindow : EditorWindow 
{
    // Reference to ScriptableObject
    public LevelData levelData;

    // Texture that will be read
    public Texture2D map;
    
    // Data
    private string levelName;

    private GameObject parentObject;
    private Vector2 scrollPos;

    [MenuItem("Tools/CDeWijs/Platformer Level Editor")]
    static void Init() 
    {
        // Get existing open window, or if none make a new one
        PlatformerLevelWindow window = (PlatformerLevelWindow)GetWindow(typeof(PlatformerLevelWindow), true, "Platformer Level Editor");
        window.Show();
    }

    private void OnEnable() 
    {
        // Load ScriptableObject with level data
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            levelData = (LevelData)AssetDatabase.LoadAssetAtPath(objectPath, typeof(LevelData));
        }
    }

    private void OnGUI() 
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        // Open list from files
        if (GUILayout.Button("Open List"))
        {
            OpenList();
        }
        // Select existing list in project
        if (levelData) {
            if (GUILayout.Button("Select Existing List"))
            {
                Selection.activeObject = levelData;
            }
        }
        // Create a new list
        if (GUILayout.Button("New List"))
        {
            CreateNewList();
        }

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        
        if (levelData != null)
        {
            EditorGUILayout.Space();

            // Level Name
            levelName = levelData.levelName;
            levelName = EditorGUILayout.TextField("Level name", levelName);
            levelData.levelName = levelName;

            EditorGUILayout.Space();

            GUILayout.Label("Color Mapping", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            // Texture
            levelData.texture = (Texture2D)EditorGUILayout.ObjectField("Texture", levelData.texture, typeof(Texture2D), true);
            map = levelData.texture;

            EditorGUILayout.Space();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            
            // Show as many colors/prefabs as there are in levelData
            if (levelData.mappings.Count > 0)
            {
                foreach (ColorMapping mapping in levelData.mappings)
                {
                    EditorGUILayout.BeginHorizontal();

                    // Color
                    GUILayout.Label("Color");
                    mapping.color = EditorGUILayout.ColorField(mapping.color);

                    // Prefab
                    GUILayout.Label("Prefab");
                    mapping.prefab = (GameObject)EditorGUILayout.ObjectField(mapping.prefab, typeof(GameObject), true);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.BeginHorizontal();

            // Create color mappings
            if (GUILayout.Button("Create Color Mappings"))
            {
                GetColors();
            }

            // Delete last mapping in list
            // TODO let user select which colormapping to delete
            if (GUILayout.Button("Delete Last Mapping"))
            {
                levelData.mappings.RemoveAt(levelData.mappings.Count - 1);
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.EndScrollView();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            // Generate level from level data
            if (GUILayout.Button("Generate Level"))
            {
                // Create parent object for the tiles
                parentObject = new GameObject(levelName + " Generated Tiles");
                GenerateLevel();
            }
            
            // Create prefab from Generated Tiles and assign to level data
            if (GUILayout.Button("Export Level"))
            {
                if (parentObject) {
                    ExportLevel();
                }
                else {
                    Debug.LogError("Can't export level if it's not generated! Click on 'Generate Level'.");
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        
        // Save changes to ScriptableObject
        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }
    }

    void CreateNewList() 
    {
        levelData = CreateLevelData.Create();

        if (levelData)
        {
            levelData.mappings = new List<ColorMapping>();
            string relPath = AssetDatabase.GetAssetPath(levelData);
            EditorPrefs.SetString("ObjectPath", relPath);
            Selection.activeObject = levelData;
        }
    }

    void OpenList() 
    {
        string absPath = EditorUtility.OpenFilePanel("Select Color Mapping List", "", "");

        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            levelData = (LevelData)AssetDatabase.LoadAssetAtPath(relPath, typeof(LevelData));

            if (levelData)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
            if (levelData.mappings == null)
            {
                levelData.mappings = new List<ColorMapping>();
            }

            Selection.activeObject = levelData;
        }
    }

    public void GetColors()
    {
        List<Color> colors = new List<Color>();

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixelColor = map.GetPixel(x, y);

                if (!colors.Contains(pixelColor) && pixelColor.a != 0)
                {
                    colors.Add(pixelColor);
                    NewColorMapping(pixelColor);
                }
            }
        }
    }

    public void NewColorMapping(Color color)
    {
        ColorMapping newMapping = new ColorMapping();
        newMapping.color = color;
        newMapping.prefab = null;
        levelData.mappings.Add(newMapping);
    }

    [ExecuteInEditMode]
    public void GenerateLevel() 
    {
        // Iterate over pixels in map.
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    public void GenerateTile(int x, int y) 
    {
        // Read color from pixel in map
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            // The pixel is transparent.
            return;
        }

        foreach (ColorMapping mapping in levelData.mappings)
        {
            if (mapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);

                Instantiate(mapping.prefab, position, Quaternion.identity, parentObject.transform);
            }
        }
    }
    
    public void ExportLevel() 
    {
        Selection.activeGameObject = parentObject;
        GameObject[] gObjects = Selection.gameObjects;

        foreach(GameObject g in gObjects)
        {
            Object obj = PrefabUtility.CreateEmptyPrefab("Assets/" + g.gameObject.name + ".prefab");
            Object prefab = PrefabUtility.ReplacePrefab(g.gameObject, obj, ReplacePrefabOptions.ConnectToPrefab);
            levelData.generatedTiles = prefab;
        }
    }
}
