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
    protected GameObject HideWindowButton;

    void Awake()
    {
        FrameLeft = transform.Find("FrameLeft").gameObject;
        FrameRight = transform.Find("FrameRight").gameObject;
        FrameBottom = transform.Find("FrameBottom").gameObject;
        Bar = transform.Find("Bar").gameObject;
        Screen = transform.Find("Screen").gameObject;
        
        CloseWindowButton = transform.Find("CloseWindowButton").gameObject;
        MinimizeWindowButton = transform.Find("MinimizeWindowButton").gameObject;
        HideWindowButton = transform.Find("HideWindowButton").gameObject;
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
                HandleBar(onAim);
                break;
            case "FrameLeft":
                HandleLeftFrame(onAim, difference);
                break;
            case "FrameRight":
                HandleRightFrame(onAim, difference);
                break;
            case "FrameBottom":
                HandleFrameBottom(onAim, difference);
                break;
        }
    }

    protected void HandleBar(GameObject onAim)
    {
        /*
        var newPos = CoreController.Camera.transform.position + (CoreController.Camera.transform.forward * 1.0f);
        var p = onAim.transform.position - transform.position;
        transform.position = newPos - p;
        transform.rotation = CoreController.Camera.transform.rotation;
        */

        Vector3 newPos;
        if (InputHandler.IsUsingGestures) 
        {
            var midPoint = CoreController.Instance.getPinchMidPoint();
            var dir = midPoint - CoreController.Camera.transform.position;
            dir.Normalize();
            newPos = midPoint + dir*2;
        }
        else 
        {
            newPos = CoreController.Camera.transform.position + (CoreController.Camera.transform.forward * 1.0f);
        }

        var p = onAim.transform.position - transform.position;

        transform.position = newPos - p;
        transform.rotation = CoreController.Camera.transform.rotation;
    }

    protected void HandleLeftFrame(GameObject onAim, Vector3 difference)
    {
        var tmp = Vector3.right * difference.x;
        onAim.transform.Translate(tmp);
        foreach (var element in new GameObject[] { Screen, FrameBottom, Bar })
        {
            element.transform.localScale -= tmp;
            element.transform.Translate(tmp / 2.0f);
        }
        transform.Find("HideWindowButton").gameObject.transform.Translate(tmp);
    }

    protected void HandleRightFrame(GameObject onAim, Vector3 difference)
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
    }

    protected void HandleFrameBottom(GameObject onAim, Vector3 difference)
    {
        var tmp = Vector3.up * difference.y;
        onAim.transform.Translate(tmp);
        foreach (var element in new GameObject[] { Screen, FrameRight, FrameLeft })
        {
            if (element == Screen) element.transform.localScale += tmp;
            else element.transform.localScale -= tmp;
            element.transform.Translate(tmp / 2.0f);
        }
    }


    protected void CheckButtons()
    {
        if (this.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "CloseWindowButton":
                onDestroyButtonClick();
                break;
            case "MinimizeWindowButton":
                onMinimizeButtonClick();
                break;
            case "HideWindowButton":
                onHideButtonClick();
                break;
        }
    }

    protected void onDestroyButtonClick()
    {
        Destroy(gameObject);
    }

    protected void onMinimizeButtonClick()
    {
        CoreController.Instance.minimizeHandler.Minimize(this);
    }

    protected void onHideButtonClick()
    {
        if (Screen != null) Screen.SetActive(!Screen.activeSelf);
        if (FrameLeft != null) FrameLeft.SetActive(!FrameLeft.activeSelf);
        if (FrameRight != null) FrameRight.SetActive(!FrameRight.activeSelf);
        if (FrameBottom != null) FrameBottom.SetActive(!FrameBottom.activeSelf);
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