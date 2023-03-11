using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowWithPaint : Window
{
    
    public Texture2D originalTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject drawingArea = transform.GetChild(1).gameObject;
        var renderer = drawingArea.GetComponent<Renderer>();
        var copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
        copyTexture.SetPixels(originalTexture.GetPixels());
        copyTexture.Apply();
        renderer.material.EnableKeyword("_NORMALMAP");
        renderer.material.SetTexture("_MainTex", copyTexture);
        
        // Add Input handlers to core
        app.InputHandlers.Add(CheckDrawing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckDrawing()
    {
        if (app.ActiveWindow != gameObject) return;
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Stationary) return;

        var hit = app.Cursor.LastHitInfo;
        var rend = hit.transform.GetComponent<Renderer>();
        var meshCollider = hit.collider as MeshCollider;

        if (! rend || ! rend.sharedMaterial || ! rend.sharedMaterial.mainTexture || ! meshCollider)
            return;

        var tex = rend.material.mainTexture as Texture2D;
        var pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
        tex.SetPixel((int)pixelUV.x, (int)pixelUV.y + 1, Color.red);
        tex.SetPixel((int)pixelUV.x, (int)pixelUV.y - 1, Color.red);
        tex.SetPixel((int)pixelUV.x + 1, (int)pixelUV.y, Color.red);
        tex.SetPixel((int)pixelUV.x - 1, (int)pixelUV.y, Color.red);

        tex.Apply();
    }
}
