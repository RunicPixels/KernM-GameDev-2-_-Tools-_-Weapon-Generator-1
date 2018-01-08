using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

[CustomEditor(typeof(Generator))]
public class WeaponGenerator : EditorWindow_EasySimpleDisplay {
    public string weaponName = "Weapon";

    public List<Texture2D> blades;
    public List<Texture2D> guards;
    public List<Texture2D> handles;

    public Texture2D blade, guard, handle;
    private Texture2D gameObject;

    private bool randBlade, randGuard, randHandle;
    private bool randBladeHue, randGuardHue, randHandleHue;

    private bool seeded = false;

    private bool editStats = true;

    private string seed = "";

    private int width, height;

    public Texture2D destTex;

    private Vector2Int offsetBlade, offsetGuard, offsetHandle;

    [MenuItem("Tools/Cheezegami/Weapon Generator")]
    static void Init() {
        // Get existing open window, or if none make a new one
        WeaponGenerator window = (WeaponGenerator)GetWindow(typeof(WeaponGenerator), true, "WeaponGenerator");
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
        EditorGUI.LabelField(new Rect(20, 24, position.width- rightOffset-margin, 20), new GUIContent().text = "Randomize Blade Shape?");
        randBlade = EditorGUI.Toggle(new Rect(position.width-rightOffset, 24, 100, 20), !randBlade);
        EditorGUI.LabelField(new Rect(20, 44, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Blade Color?");
        randBladeHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44, 100, 20), !randBladeHue);
        guard = (Texture2D)EditorGUILayout.ObjectField("Guard", guard, typeof(Texture2D), true);
        EditorGUI.LabelField(new Rect(20, 24+spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Shape?");
        randGuard = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + spacing, 100, 20), !randGuard);
        EditorGUI.LabelField(new Rect(20, 44 + spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Color?");
        randGuardHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + spacing, 100, 20), !randGuardHue);
        handle = (Texture2D)EditorGUILayout.ObjectField("Handle", handle, typeof(Texture2D), true);
        EditorGUI.LabelField(new Rect(20, 24+(spacing*2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Shape?");
        randHandle = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + (spacing * 2), 100, 20), !randHandle);
        EditorGUI.LabelField(new Rect(20, 44 + (spacing * 2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Color?");
        randHandleHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + (spacing * 2), 100, 20), !randHandleHue);
        #endregion

        EditorGUILayout.Space();

        editStats = EditorGUILayout.BeginToggleGroup("Weapon Stats", editStats);
            GUILayout.Label("Weapon Name:");
            weaponName = GUILayout.TextField(weaponName);
        EditorGUILayout.EndToggleGroup();

        seeded = EditorGUILayout.BeginToggleGroup("Seed", seeded);
            GUILayout.Label("Seed:");
            seed = GUILayout.TextField(seed);
        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Generate Item")) {
            Generate();
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

    void Generate() {
        if(seeded)UnityEngine.Random.InitState(seed.GetHashCode());
        if (randBlade)   blade  =    GetRandomTexture(blades);
        if(randGuard)   guard  =    GetRandomTexture(guards);
        if(randHandle)  handle =    GetRandomTexture(handles);


        width = Mathf.Max(blade.width, guard.width, handle.width);
        height = blade.height + guard.height + handle.height + offsetBlade.y + offsetGuard.y + offsetHandle.y;

        offsetBlade.y += guard.height + offsetGuard.y + handle.height + offsetHandle.y;
        offsetGuard.y += handle.height + offsetHandle.y;

        offsetBlade.x += width / 2 - blade.width / 2;
        offsetHandle.x += width / 2 - handle.width / 2;
        offsetGuard.x += width / 2 - guard.width / 2;

        if (guard.GetPixel(guard.width, guard.height) == Color.clear) {

            offsetGuard.y++;
        }
        Debug.Log(guard.GetPixel(guard.width, guard.height));

        //Color[] bladePix = blade.GetPixels(0, 0, blade.width, blade.height);
        //Color[] guardPix = guard.GetPixels(0, 0, guard.width, guard.height);
        //Color[] handlePix = handle.GetPixels(0, 0, handle.width, handle.height);

        destTex = new Texture2D(width, height, TextureFormat.RGBA32, true) {
            filterMode = FilterMode.Point
        };

        destTex.EncodeToPNG();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                destTex.SetPixel(x, y, Color.clear);
            }
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x >= offsetBlade.x && x < offsetBlade.x + blade.width
                 && y >= offsetBlade.y && y < offsetBlade.y + blade.height) { SetPixel(blade, destTex, x, y, offsetBlade); }
                if (x >= offsetGuard.x && x < offsetGuard.x + guard.width
                 && y >= offsetGuard.y && y < offsetGuard.y + guard.height) { SetPixel(guard, destTex, x, y, offsetGuard); }
                if (x >= offsetHandle.x && x < offsetHandle.x + handle.width
                 && y >= offsetHandle.y && y < offsetHandle.y + handle.height) { SetPixel(handle, destTex, x, y, offsetHandle); }

            }
        }

        float hRand = UnityEngine.Random.Range(0f, 1f);
        float sRand = UnityEngine.Random.Range(0.5f, 1f);
        float vRand = UnityEngine.Random.Range(0.9f, 1f);

        //destTex.SetPixels(texturePix);

        Color[] colors = destTex.GetPixels();
        for (int i = 0; i < colors.Length; i++) {
            if (colors[i] != Color.clear) {
                float h;
                float s;
                float v;

                Color.RGBToHSV(colors[i], out h, out s, out v);

                h += hRand;

                s = sRand;

                //v = vRand;

                h = h % 1;

                colors[i] = Color.HSVToRGB(h, s, v);

                //destTex.SetPixel(i % destTex.width, Mathf.FloorToInt(i / destTex.width), colors[i]);
            }
        }
        destTex.SetPixels(colors);

        // Set the current object's texture to show the
        // extracted rectangle.
        Sprite sprite = Sprite.Create(destTex, new Rect(0, 0, width, height), new Vector2(0, 0));
        byte[] bytes = destTex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + weaponName + ".png", bytes: bytes);
        Debug.Log("Weapon saved at " + Application.dataPath + "/" + weaponName +".png");
    }

    void SetPixel( Texture2D input, Texture2D output, int x, int y, Vector2Int offset ) {
        if (input.GetPixel(x - offset.x, y - offset.y).a != 0) {
            output.SetPixel(x, y, input.GetPixel(x - offset.x, y - offset.y));
            output.Apply();
        }
    }

    Texture2D GetRandomTexture( List<Texture2D> list ) {
        Texture2D output;
        output = list[UnityEngine.Random.Range(0, list.Count)];
        return output;
    }
}
