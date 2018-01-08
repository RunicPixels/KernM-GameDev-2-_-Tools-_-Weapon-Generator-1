using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generator : MonoBehaviour {
    public List<Texture2D> blades;
    public List<Texture2D> guards;
    public List<Texture2D> handles;

    public Texture2D blade;
    public Texture2D guard;
    public Texture2D handle;

    int width;
    int height;

    public Texture2D destTex;

    private Vector2Int offsetBlade = new Vector2Int(0, 0);
    private Vector2Int offsetGuard = new Vector2Int(0, 0);
    private Vector2Int offsetHandle = new Vector2Int(0, 0);

    // Use this for initialization
    void Start () {
        Generate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGUI() {
        offsetBlade = Vector2Int.zero;
        offsetGuard = Vector2Int.zero;
        offsetHandle = Vector2Int.zero;
        if(Time.time % 0.5 < Time.deltaTime)Generate();
        EditorGUI.DrawPreviewTexture(new Rect(0, 0, width, height), destTex);
    }

    void Generate() {
        blade = GetRandomTexture(blades);
        guard = GetRandomTexture(guards);
        handle = GetRandomTexture(handles);

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
        for(int i = 0; i<colors.Length; i++) {
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
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void SetPixel(Texture2D input, Texture2D output, int x, int y, Vector2Int offset) {
        if(input.GetPixel(x-offset.x,y-offset.y).a != 0) {
            output.SetPixel(x, y, input.GetPixel(x - offset.x, y - offset.y));
            output.Apply();
        }
    }
    Texture2D GetRandomTexture(List<Texture2D> list) {
        Texture2D output;
        output = list[UnityEngine.Random.Range(0, list.Count)];
        return output;
    }
}
