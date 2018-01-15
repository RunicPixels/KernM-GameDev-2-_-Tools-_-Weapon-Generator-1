using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Cheezegami {
    public class Texture2DFinder {
        /// <summary>
        /// Folder in which all images all converted to an array of Texture2Ds whilest picking out a random Texture2D of those supplied.
        /// </summary>
        public Texture2D FindRandomTexture(String folder) {
            Texture2D[] array = Resources.LoadAll<Texture2D>(folder);
            Texture2D output;
            Debug.Log(array);
            output = array[UnityEngine.Random.Range(0, array.Length)];
            return output;
        }
    }
}
