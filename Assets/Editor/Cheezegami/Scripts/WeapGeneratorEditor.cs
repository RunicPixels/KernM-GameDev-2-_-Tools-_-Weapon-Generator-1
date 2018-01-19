using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

namespace Cheezegami {
    public class WeapGenEditor : EditorWindow_EasySimpleDisplay {

        public static Dictionary<string, string> folders = new Dictionary<string, string>();
        public bool randBlade, randGuard, randHandle;
        public bool randBladeHue, randGuardHue, randHandleHue;

        private bool editStats = true;
        private bool seeded = false;
        private Texture2DFinder finder = new Texture2DFinder();
        private WeaponGenerator generator = new WeaponGenerator();

        [MenuItem("Tools/Cheezegami/Weapon Generator")]
        private static void Init() {
            folders.Add("bladeFolder", "Blades");
            folders.Add("guardFolder", "Guards");
            folders.Add("handleFolder", "Handles");

            // Get existing open window, or if none make a new one
            WeapGenEditor window = (WeapGenEditor)GetWindow(typeof(WeapGenEditor), true, "Weapon Generator");
            window.Show();
            window.minSize = new Vector2(300, 700);
        }

        public override void OnGUI() {
            ShowGUIContent();
            base.OnGUI();
        }

        private void ShowGUIContent() {
            int rightOffset = 120;
            int spacing = 66;
            int margin = 20;

            EditorGUILayout.Space();

            #region Weapon Part Specification
            generator.blade = (Texture2D)EditorGUILayout.ObjectField("Blade", generator.blade, typeof(Texture2D), true);
            EditorGUI.LabelField(new Rect(20, 24, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Blade Shape?");
            randBlade = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24, 100, 20), !randBlade);
            EditorGUI.LabelField(new Rect(20, 44, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Blade Color?");
            randBladeHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44, 100, 20), !randBladeHue);
            generator.guard = (Texture2D)EditorGUILayout.ObjectField("Guard", generator.guard, typeof(Texture2D), true);
            EditorGUI.LabelField(new Rect(20, 24 + spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Shape?");
            randGuard = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + spacing, 100, 20), !randGuard);
            EditorGUI.LabelField(new Rect(20, 44 + spacing, position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Guard Color?");
            randGuardHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + spacing, 100, 20), !randGuardHue);
            generator.handle = (Texture2D)EditorGUILayout.ObjectField("Handle", generator.handle, typeof(Texture2D), true);
            EditorGUI.LabelField(new Rect(20, 24 + (spacing * 2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Shape?");
            randHandle = EditorGUI.Toggle(new Rect(position.width - rightOffset, 24 + (spacing * 2), 100, 20), !randHandle);
            EditorGUI.LabelField(new Rect(20, 44 + (spacing * 2), position.width - rightOffset - margin, 20), new GUIContent().text = "Randomize Handle Color?");
            randHandleHue = EditorGUI.Toggle(new Rect(position.width - rightOffset, 44 + (spacing * 2), 100, 20), !randHandleHue);
            #endregion

            EditorGUILayout.Space();

            editStats = EditorGUILayout.BeginToggleGroup("Weapon Stats", editStats);
            if (editStats) {
                GUILayout.Label("Weapon Name:");
                generator.weaponName = GUILayout.TextField(generator.weaponName);
            }
            EditorGUILayout.EndToggleGroup();

            seeded = EditorGUILayout.BeginToggleGroup("Seed", seeded);
            if (!seeded) generator.seed = Random.seed.ToString();
            if (seeded) {
                GUILayout.Label("Seed:");
                generator.seed = GUILayout.TextField(generator.seed);
            }
            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate Item")) {
                if (seeded) UnityEngine.Random.InitState(generator.seed.GetHashCode());

                if (randBlade) generator.blade = finder.FindRandomTexture(folders["blades"]);
                if (randGuard) generator.guard = finder.FindRandomTexture(folders["guards"]);
                if (randHandle) generator.handle = finder.FindRandomTexture(folders["handles"]);
                generator.Generate(randBladeHue, randGuardHue, randHandleHue); // this is where the magic happens.
                generator.SaveTexture();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Background Color (Preview Window):");
            Display.Camera.clearFlags = CameraClearFlags.Color;
            Display.Camera.backgroundColor = EditorGUILayout.ColorField(Display.Camera.backgroundColor);

            if (generator.destTex != null) {
                Display.ClearRenderQueue();
                Display.AddTexture(generator.destTex, 10);

            }
        }
    }
}