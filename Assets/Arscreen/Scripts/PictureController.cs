using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PictureController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (! InputHandler.Clicked()) return;
        if (CoreController.Instance.Cursor.LastHitInfo.collider.gameObject != gameObject) return;

        if (NativeFilePicker.IsFilePickerBusy()) return;
        var permission = NativeFilePicker.CheckPermission();
        if (permission == NativeFilePicker.Permission.Denied) return;
        if (permission == NativeFilePicker.Permission.ShouldAsk &&
            NativeFilePicker.RequestPermission() == NativeFilePicker.Permission.Denied) return;

        string[] allowedTypes = { "image/*" };
        NativeFilePicker.PickFile(setPicture, allowedTypes);
    }

    void setPicture(string path)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        if (!File.Exists(path)) return;
        var fileData = File.ReadAllBytes(path);
        var tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        renderer.material.EnableKeyword("_NORMALMAP");
        renderer.material.SetTexture("_MainTex", tex);
    }
}
