using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SavedPNG {
    byte[] png;

    public SavedPNG(Texture2D tex) {
    }

    public void SaveAsPNG( Texture2D tex, string path) {
        png = tex.EncodeToPNG();
    }
}
