using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Cheezegami.Scripts {
    public class Texture2DFinder {
        public Texture2D FindRandomTexture(String type) {
            Texture2D[] list;
            list = Resources.LoadAll(type, typeof(Texture2D));
            Texture2D output;
            output = list[UnityEngine.Random.Range(0, list.Length)];
            return output;
        }
    }
}
