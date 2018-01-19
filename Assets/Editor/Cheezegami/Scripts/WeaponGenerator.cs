using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;

namespace Cheezegami {
    public class WeaponGenerator {

        public WeaponGenerator() {
        }
        public string weaponName = "Weapon";

        public Texture2D blade, guard, handle;
        public Texture2D destTex;

        private bool seeded = false;

        public string path;

        public string seed = "";

        private int width, height;

        private Vector2Int offsetBlade, offsetGuard, offsetHandle;



        /// <summary>
        /// Generates a random weapon by placing the blade, guard and handle sprite on top of each other, with blade being on top, guard below that and handle at the bottom.
        /// </summary>
        /// <param name="randBlade">Randomize the color of the blade?</param>
        /// <param name="randGuard">Randomize the color of the guard?</param>
        /// <param name="randHandle">Randomize the color of the handle?</param>
        public void Generate( bool randBlade = false, bool randGuard = false, bool randHandle = false ) {
            offsetBlade = new Vector2Int(0, 0);
            offsetGuard = new Vector2Int(0, 0);
            offsetHandle = new Vector2Int(0, 0);


            Texture2D bladeModifier = new Texture2D(blade.width, blade.height, TextureFormat.RGBA32, false),
                      guardModifier = new Texture2D(guard.width, guard.height, TextureFormat.RGBA32, false),
                      handleModifier = new Texture2D(handle.width, handle.height, TextureFormat.RGBA32, false);
            
            bladeModifier.SetPixels(blade.GetPixels());
            guardModifier.SetPixels(guard.GetPixels());
            handleModifier.SetPixels(handle.GetPixels());

            if (randBlade) bladeModifier = ChangeColor(bladeModifier);
            if (randGuard) guardModifier = ChangeColor(guardModifier);
            if (randHandle) handleModifier = ChangeColor(handleModifier);

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

            destTex = new Texture2D(width, height, TextureFormat.RGBA32, false, true) {
                filterMode = FilterMode.Point
                
            };
            for (int i = 0; i < destTex.GetPixels32().Length; i++) { // Clears Out DestTex to make it a clear texture.
                destTex.SetPixel(i % destTex.width, Mathf.FloorToInt(i / destTex.width), Color.clear);
            }

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
            //Sprite sprite = Sprite.Create(destTex, new Rect(0, 0, width, height), new Vector2(0, 0));
        }

        public void SaveTexture() {
            byte[] bytes = destTex.EncodeToPNG();
            path = Application.dataPath + "/" + weaponName + ".png";
            File.WriteAllBytes(path, bytes: bytes);

            Debug.Log("Weapon saved at " + Application.dataPath + "/" + weaponName + ".png");
            if (AssetImporter.GetAtPath(path) != null) {
                TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
                texImp.filterMode = FilterMode.Point;
            }
        }

        private void SetPixel( Texture2D input, Texture2D output, int x, int y, Vector2Int offset ) {
            if (input.GetPixel(x - offset.x, y - offset.y).a != 0) {
                output.SetPixel(x, y, input.GetPixel(x - offset.x, y - offset.y));
                output.Apply();
            }
        }
        private Texture2D ChangeColor( Texture2D texture ) {
            float hRand = UnityEngine.Random.Range(0f, 1f);
            float sRand = UnityEngine.Random.Range(0.5f, 1f);
            float vRand = UnityEngine.Random.Range(0.9f, 1f);
            texture.filterMode = FilterMode.Point;
            Color32[] colors = texture.GetPixels32();
            for (int i = 0; i < colors.Length; i++) {
                if (colors[i].a != 0) {
                    float h;
                    float s;
                    float v;

                    Color.RGBToHSV(colors[i], out h, out s, out v);

                    h += hRand;

                    s = sRand;

                    //v = vRand;

                    h = h % 1;

                    colors[i] = Color.HSVToRGB(h, s, v);
                    
                    //texture.SetPixel(i % texture.width, Mathf.FloorToInt(i / texture.width), colors[i]);
                }

                //else {
                //    for (int x = 0; x < width; x++) {
                //        for (int y = 0; y < height; y++) {
                //            texture.SetPixel(x, y, Color.clear);
                //        }
                //    }
                //}
            }

            texture.SetPixels32(colors);
            return texture;
        }
    }
}