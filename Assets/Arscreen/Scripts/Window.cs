using System;
using UnityEngine;

/// <summary>
/// Window Class.
/// </summary>
/// <param name="_minimized">Boolean flag informs whether window is minimized or not</param>
/// <param name="_savedScaleOfWindow">Saves the scale of window, so it's value can be reset</param>
public class Window : MonoBehaviour
{
    protected GameObject Screen;
    protected GameObject FrameLeft;
    protected GameObject FrameRight;
    protected GameObject FrameBottom;
    protected GameObject Bar;

    protected GameObject CloseWindowButton; //ewentualnie mozna zastapic obiektem na ktorym znajdowaly by sie przyciski
    protected GameObject MinimizeWindowButton;

    void Awake()
    {
        FrameLeft = transform.Find("FrameLeft").gameObject;
        FrameRight = transform.Find("FrameRight").gameObject;
        FrameBottom = transform.Find("FrameBottom").gameObject;
        Bar = transform.Find("Bar").gameObject;
        Screen = transform.Find("Screen").gameObject;

        CloseWindowButton = transform.Find("CloseWindowButton").gameObject;
        MinimizeWindowButton = transform.Find("MinimizeWindowButton").gameObject;
    }

    protected void Start()
    {
        gameObject.SetActive(true);
    }

    protected void Update()
    {
        this.CheckStandardWindowInteractions();
        this.CheckButtons();
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
    protected void CheckStandardWindowInteractions()
    {
        if (this.CheckConditionsForStandardWindowInteractions() == false) return;

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
            case "FrameLeft":
                {
                    var tmp = Vector3.right * difference.x;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameBottom, Bar})
                    {
                        element.transform.localScale -= tmp;
                        element.transform.Translate(tmp / 2.0f);
                    }
                    break;
                }
            case "FrameRight":
                {
                    var tmp = Vector3.right * difference.x;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameBottom, Bar })
                    {
                        element.transform.localScale += tmp;
                        element.transform.Translate(tmp / 2.0f);
                    }
                    transform.Find("CloseWindowButton").gameObject.transform.Translate(tmp);
                    transform.Find("MinimizeWindowButton").gameObject.transform.Translate(tmp);
                    break;
                }
            case "FrameBottom":
                {
                    var tmp = Vector3.up * difference.y;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameRight, FrameLeft })
                    {
                        if (element == Screen) element.transform.localScale += tmp;
                        else element.transform.localScale -= tmp;
                        element.transform.Translate(tmp /2.0f);
                    }
                    break;
                }
        }
    }

    protected void CheckButtons()
    {
        if (this.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "CloseWindowButton":
                Destroy(gameObject);
                break;

            case "MinimizeWindowButton":
                if (Screen != null) Screen.SetActive(!Screen.activeSelf);
                if (FrameLeft != null) FrameLeft.SetActive(!FrameLeft.activeSelf);
                if (FrameRight != null) FrameRight.SetActive(!FrameRight.activeSelf);
                if (FrameBottom != null) FrameBottom.SetActive(!FrameBottom.activeSelf);
                break;
        }
    }

    protected bool CheckConditionsForButtons()
    {
        if (CoreController.Instance.ActiveWindow != gameObject) return false;
        if (!InputHandler.Clicked()) return false;
        return true;
    }

    protected bool CheckConditionsForStandardWindowInteractions()
    {
        if (CoreController.Instance.ActiveWindow != gameObject) return false;
        if (!InputHandler.Holding()) return false;
        return true;
    }
}