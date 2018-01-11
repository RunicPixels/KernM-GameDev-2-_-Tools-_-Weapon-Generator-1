using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

[CustomEditor(typeof(Generator))]
public class WeapGenEditor : EditorWindow_EasySimpleDisplay {
    public string weaponName = "Weapon";

    public List<Texture2D> blades, guards, handles;

    public Texture2D blade, guard, handle;
    public Texture2D destTex;
    private Texture2D gameObject;

    private bool randBlade, randGuard, randHandle;
    private bool randBladeHue, randGuardHue, randHandleHue;

    private bool seeded = false;

    private bool editStats = true;

    private string seed = "";

    private int width, height;



    private Vector2Int offsetBlade, offsetGuard, offsetHandle;

    [MenuItem("Tools/Cheezegami/Weapon Generator")]
    static void Init() {
        // Get existing open window, or if none make a new one
        WeapGenEditor window = (WeapGenEditor)GetWindow(typeof(WeapGenEditor), true, "Weapon Generator");
        window.Show();
        window.minSize = new Vector2(300, 700);

    }

    public override void OnGUI() {
        offsetBlade = new Vector2Int(0, 0);
        offsetGuard = new Vector2Int(0, 0);
        offsetHandle = new Vector2Int(0, 0);
        int rightOffset = 120;
        int spacing = 66;
        int margin = 20;

        EditorGUILayout.Space();

        #region Weapon Part Specification
        blade = (Texture2D)EditorGUILayout.ObjectField("Blade", blade, typeof(Texture2D), true);
        EditorGUI.LabelField(new Rect(20, 24, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Blade Shape?");
        randBlade = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24, 100, 20), !randBlade);
        EditorGUI.LabelField(new Rect(20, 44, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Blade Color?");
        randBladeHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44, 100, 20), !randBladeHue);
        guard = (Texture2D)EditorGUILayout.ObjectField("Guard", guard, typeof(Texture2D), true);
        EditorGUI.LabelField(new Rect(20, 24 + spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Shape?");
        randGuard = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + spacing, 100, 20), !randGuard);
        EditorGUI.LabelField(new Rect(20, 44 + spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Color?");
        randGuardHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + spacing, 100, 20), !randGuardHue);
        handle = (Texture2D)EditorGUILayout.ObjectField("Handle", handle, typeof(Texture2D), true);
        EditorGUI.LabelField(new Rect(20, 24 + (spacing * 2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Shape?");
        randHandle = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + (spacing * 2), 100, 20), !randHandle);
        EditorGUI.LabelField(new Rect(20, 44 + (spacing * 2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Color?");
        randHandleHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + (spacing * 2), 100, 20), !randHandleHue);
        #endregion

        EditorGUILayout.Space();

        editStats = EditorGUILayout.BeginToggleGroup("Weapon Stats", editStats);
        if (editStats) {
            GUILayout.Label("Weapon Name:");
            weaponName = GUILayout.TextField(weaponName);
        }
        EditorGUILayout.EndToggleGroup();

        seeded = EditorGUILayout.BeginToggleGroup("Seed", seeded);
        if (!seeded) seed = Random.seed.ToString();
        if (seeded) {
            GUILayout.Label("Seed:");
            seed = GUILayout.TextField(seed);
        }
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        WeaponGenerator generator = new WeaponGenerator();

        if (GUILayout.Button("Generate Item")) {
            if (seeded) UnityEngine.Random.InitState(seed.GetHashCode());
            if (randBlade) blade = generator.GetRandomTexture(blades);
            if (randGuard) guard = GetRandomTexture(guards);
            if (randHandle) handle = GetRandomTexture(handles);
            generator.Generate();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Background Color (Preview Window):");
        Display.Camera.clearFlags = CameraClearFlags.Color;
        Display.Camera.backgroundColor = EditorGUILayout.ColorField(Display.Camera.backgroundColor);

        if (destTex != null) {
            Display.ClearRenderQueue();
            Display.AddTexture(destTex, 10);

        }
        base.OnGUI();



    }
}
