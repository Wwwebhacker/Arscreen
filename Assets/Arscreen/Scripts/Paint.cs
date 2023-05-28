using UnityEngine;
using Color = UnityEngine.Color;

/// <summary>
/// WindowWithPaint Class.
/// </summary>
/// <param name="original Texture">?</param>
/// <param name="NO_POINT">?</param>
/// <param name="_lastPoint">?</param>
/// <param name="_renderer">?</param>
/// <param name="_brushSize">Size of the brush used to paint on screen</param>
/// <param name="_color">Index of a color used to paint on screen</param>
public class Paint : MonoBehaviour
{
    public Texture2D originalTexture;
    private static readonly Vector2 NO_POINT = Vector2.zero;
    private Vector2 _lastPoint = NO_POINT;
    private Renderer _renderer;
    private int _brushSize = 1;
    private int _colorIdx = 0;

    private Color BrushColor
    {
        set {}
        get => _colorPallet[_colorIdx];
    }
    private int _debug = 0;
    private Tools _tool = Tools.Brush;

    private enum Tools
    {
        Brush, Line, Circle
    }

    private readonly Color[] _colorPallet = new Color[]{
    Color.red, Color.blue, Color.green, Color.magenta, Color.yellow, Color.black, Color.white
    };

    void Start()
    {
        var drawingArea = transform.Find("Screen");
        _renderer = drawingArea.GetComponent<Renderer>();
        var copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
        copyTexture.SetPixels(originalTexture.GetPixels());
        copyTexture.Apply();
        _renderer.material.EnableKeyword("_NORMALMAP");
        _renderer.material.SetTexture("_MainTex", copyTexture);
    }

    void Update()
    {
        CheckDrawing();
        CheckStopDrawing();
        CheckButtons();
    }

    private void DrawPoint(Texture2D tex, Vector2 point)
    {
        for (var x = (int)point.x - _brushSize; x <= (int)point.x + _brushSize; x++)
        {
            for (var y = (int)point.y - _brushSize; y <= (int)point.y + _brushSize; y++)
            {
                var dist = Vector2.Distance(new Vector2(x, y), point);
                if ( dist < _brushSize)
                    tex.SetPixel(x, y, BrushColor);
            }
        }
    }

    private void CheckButtons()
    {
        // Conditions
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if (!InputHandler.Clicked()) return;
        //

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;
        switch (onAim.name)
        {
            case "Button1": _brushSize++;
                break;
            case "Button2": _brushSize = _brushSize == 1 ? 1 : _brushSize - 1;
                break;
            case "Button3": 
                _colorIdx = (_colorIdx + 1) % _colorPallet.Length;
                onAim.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = BrushColor;
                break;
            case "Button4":
                _tool = _tool switch
                {
                    Tools.Brush => Tools.Line,
                    Tools.Line => Tools.Brush,
                    _ => Tools.Brush
                };
                onAim.transform.GetChild(0).transform.localScale = _tool switch
                {
                    Tools.Brush => new Vector3(0.4f, 0.4f, 1),
                    Tools.Line => new Vector3(0.8f, 0.2f, 1),
                    _ => new Vector3(0.8f, 0.2f, 1),
                };
                break;
            case "Button5": 
                _debug = (_debug + 1) % 4;
                onAim.transform.GetChild(0).GetComponent<MeshRenderer>().material
                    .color = new Color(0.2f,0.2f,0.2f)*_debug;
                break;
        }
    }

    private void CheckStopDrawing()
    {
        // Conditions
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if (InputHandler.Holding()) return;
        //
        if (_tool == Tools.Line) return;
        _lastPoint = NO_POINT;
    }

    private void CheckDrawing()
    {
        // Conditions
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if (!InputHandler.Holding()) return;
        var hit = CoreController.Instance.Cursor.LastHitInfo;
        if (hit.collider.name != "Screen") return;
        //if (_tool != Tools.Brush) return;

        var tex = _renderer.material.mainTexture as Texture2D;
        var pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        var p = pixelUV;

        if (_lastPoint == NO_POINT)
            DrawPoint(tex, p);
        else
        {
            var finalPoint = p;
            p = _lastPoint;

            for (var i = 0; i < Vector2.Distance(_lastPoint, finalPoint); i++)
            {
                DrawPoint(tex, p);
                p = Vector2.MoveTowards(p, finalPoint, 1);
            }
        }
        _lastPoint = p;
        tex.Apply();
    }

    private void CheckLineDrawing()
    {
        // Conditions
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if (!InputHandler.Holding()) return;
        var hit = CoreController.Instance.Cursor.LastHitInfo;
        if (hit.collider.name != "Screen") return;
        if (_tool != Tools.Line) return;
        //
        
        var tex = _renderer.material.mainTexture as Texture2D;
        var pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        var p = pixelUV;
    }
}