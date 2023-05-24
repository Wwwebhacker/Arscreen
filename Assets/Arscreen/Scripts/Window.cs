using System;
using UnityEngine;

/// <summary>
/// Window Class.
/// </summary>
/// <param name="_minimized">Boolean flag informs whether window is minimized or not</param>
/// <param name="_savedScaleOfWindow">Saves the scale of window, so it's value can be reset</param>
public class Window : MonoBehaviour
{
    protected Vector3 _savedScaleOfWindow = new Vector3(1.0f, 1.0f, 1.0f);
    protected Vector3 _savedScaleOfScreen = new Vector3(0.45f, 0.25f, 0.01f);
    protected GameObject Screen;
    protected GameObject FrameLeft;
    protected GameObject FrameRight;
    protected GameObject FrameBottom;
    protected GameObject Bar;


    void Awake()
    {
        FrameLeft = transform.Find("FrameLeft").gameObject;
        FrameRight = transform.Find("FrameRight").gameObject;
        FrameBottom = transform.Find("FrameBottom").gameObject;
        Bar = transform.Find("Bar").gameObject;
        Screen = transform.Find("Screen").gameObject;
        _savedScaleOfScreen = Screen.transform.localScale;
    }

    private void Start()
    {
        gameObject.SetActive(true);
    }


    void Update()
    {
        CheckStandardWindowInteractions();
        CheckButtons();
    }

    /// <summary>
    /// Checks if game object is part of Window object.
    /// </summary>
    /// <param name="obj">GameObject to check.</param>
    /// <param name="window">Top GameObject of Window instance.</param>
    /// <returns>True if obj has a parent with attached "Window" script.</returns>
    public static bool IsPartOfWindow(GameObject obj, out GameObject window)
    {
          window = obj;
          if (!obj) return false;
          while (! obj.TryGetComponent<Window>(out _))
          {
               if (!obj.transform.parent) return false;
               obj = obj.transform.parent.gameObject;
          }
          window = obj;
          return true;
    }

    /// <summary>
    /// Checks for input for all windows (not an input for applications in windows).
    /// <br />
    /// <br />Input checked:
    /// <list type="bullet">
    /// <item> move when holding Window Bar</item>
    /// </list>
    /// </summary>
    private void CheckStandardWindowInteractions()
    {
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if (!InputHandler.Holding()) return;
        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;
        

        var cursorPosition = CoreController.Instance.Cursor.transform.position;
        var localCursorPosition = transform.InverseTransformPoint(cursorPosition);
        var localFramePosition = onAim.transform.localPosition;
        var difference = localCursorPosition - localFramePosition;
        
        switch (onAim.name)
        {
            case "Bar":
            {
                    var newPos = CoreController.Camera.transform.position + (CoreController.Camera.transform.forward * 1.0f);
                    var p = onAim.transform.position - transform.position;
            
                    transform.position = newPos - p;
                    transform.rotation = CoreController.Camera.transform.rotation;
                    break;
            }
            case "FrameRight":
            {
                var tmp = Vector3.right * difference.x;
                onAim.transform.Translate(tmp);
                foreach (var elem1 in new GameObject[] { Screen, FrameBottom, Bar })
                {
                    elem1.transform.localScale -= tmp;
                    elem1.transform.Translate(tmp / 2.0f);
                }
                break;
            }
            case "FrameLeft":
            {
                var tmp = Vector3.right * difference.x;
                onAim.transform.Translate(tmp);
                foreach (var elem2 in new GameObject[] { Screen, FrameBottom, Bar })
                {
                    elem2.transform.localScale += tmp;
                    elem2.transform.Translate(tmp / 2.0f);
                }
                break;
                }
            case "FrameBottom":
            {
                var tmp = Vector3.up * difference.y;
                onAim.transform.Translate(tmp);
                foreach (var elem3 in new GameObject[] { Screen, FrameRight, FrameLeft })
                {
                    if (elem3 == Screen) elem3.transform.localScale += tmp;
                    else elem3.transform.localScale -= tmp;
                    elem3.transform.Translate(tmp /2.0f);
                }
                break;
            }
        }
        
    }
    
    private void CheckButtons()
    {
        // Conditions
        if(! InputHandler.Clicked()) return;
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        //
        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;
        switch (onAim.name)
        {
            case "CloseWindowButton":
                Destroy(gameObject);
                break;

            case "MinimizeWindowButton":
                CoreController.Instance.minimizeHandler.Minimize(this);
                break;
        }
    }
}
