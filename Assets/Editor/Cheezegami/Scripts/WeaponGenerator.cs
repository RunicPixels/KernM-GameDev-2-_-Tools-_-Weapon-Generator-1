using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

[CustomEditor(typeof(Generator))]
public class WeaponGenerator {

    public WeaponGenerator(string WeaponName,) {

    }
    public string weaponName = "Weapon";

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

    public void Generate() {


        Texture2D bladeModifier = new Texture2D(blade.width,blade.height, TextureFormat.RGBA32, true),
                  guardModifier = new Texture2D(guard.width, guard.height, TextureFormat.RGBA32, true),
                  handleModifier = new Texture2D(handle.width, handle.height, TextureFormat.RGBA32, true);

        bladeModifier.SetPixels32(blade.GetPixels32());
        guardModifier.SetPixels32(guard.GetPixels32());
        handleModifier.SetPixels32(handle.GetPixels32());

        if (randBladeHue) bladeModifier = ChangeColor(bladeModifier);
        if (randGuardHue) guardModifier = ChangeColor(guardModifier);
        if (randHandleHue) handleModifier = ChangeColor(handleModifier);

        width = Mathf.Max(bladeModifier.width, guardModifier.width, handleModifier.width);
        height = bladeModifier.height + guardModifier.height + handleModifier.height + offsetBlade.y + offsetGuard.y + offsetHandle.y;

        offsetBlade.y += guardModifier.height + offsetGuard.y + handleModifier.height + offsetHandle.y;
        offsetGuard.y += handleModifier.height + offsetHandle.y;

        offsetBlade.x += width / 2 - bladeModifier.width / 2;
        offsetHandle.x += width / 2 - handleModifier.width / 2;
        offsetGuard.x += width / 2 - guardModifier.width / 2;

        if (guardModifier.GetPixel(guardModifier.width, guardModifier.height).a < 1) {

            offsetGuard.y++;
        }
        Debug.Log(guard.GetPixel(guard.width, guard.height));

        //Color[] bladePix = blade.GetPixels(0, 0, blade.width, blade.height);
        //Color[] guardPix = guard.GetPixels(0, 0, guard.width, guard.height);
        //Color[] handlePix = handle.GetPixels(0, 0, handle.width, handle.height);

        destTex = new Texture2D(width, height, TextureFormat.RGBA32, false) {
            filterMode = FilterMode.Point
        };

        destTex.EncodeToPNG();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x >= offsetBlade.x && x < offsetBlade.x + bladeModifier.width
                 && y >= offsetBlade.y && y < offsetBlade.y + bladeModifier.height) { SetPixel(bladeModifier, destTex, x, y, offsetBlade); }
                if (x >= offsetGuard.x && x < offsetGuard.x + guardModifier.width
                 && y >= offsetGuard.y && y < offsetGuard.y + guardModifier.height) { SetPixel(guardModifier, destTex, x, y, offsetGuard); }
                if (x >= offsetHandle.x && x < offsetHandle.x + handleModifier.width
                 && y >= offsetHandle.y && y < offsetHandle.y + handleModifier.height) { SetPixel(handleModifier, destTex, x, y, offsetHandle); }

            }
        }


        //destTex.SetPixels(texturePix);



        // Set the current object's texture to show the
        // extracted rectangle.
        Sprite sprite = Sprite.Create(destTex, new Rect(0, 0, width, height), new Vector2(0, 0));
        byte[] bytes = destTex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/" + weaponName + ".png", bytes: bytes);
        Debug.Log("Weapon saved at " + Application.dataPath + "/" + weaponName +".png");
    }

    private void SetPixel( Texture2D input, Texture2D output, int x, int y, Vector2Int offset ) {
        if (input.GetPixel(x - offset.x, y - offset.y).a != 0) {
            output.SetPixel(x, y, input.GetPixel(x - offset.x, y - offset.y));
            output.Apply();
        }
    }
    private Texture2D ChangeColor(Texture2D texture) {
        float hRand = UnityEngine.Random.Range(0f, 1f);
        float sRand = UnityEngine.Random.Range(0.5f, 1f);
        float vRand = UnityEngine.Random.Range(0.9f, 1f);
        texture.filterMode = FilterMode.Point;
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++) {
            if (colors[i].a > 0) {
                float h;
                float s;
                float v;

                Color.RGBToHSV(colors[i], out h, out s, out v);

                h += hRand;

                s = sRand;

                //v = vRand;

                h = h % 1;

                colors[i] = Color.HSVToRGB(h, s, v);
                texture.SetPixel(i % texture.width, Mathf.FloorToInt(i / texture.width), colors[i]);
            }
            //else {
            //    for (int x = 0; x < width; x++) {
            //        for (int y = 0; y < height; y++) {
            //            texture.SetPixel(x, y, Color.clear);
            //        }
            //    }
            //}
        }
        return texture;
    }
}
