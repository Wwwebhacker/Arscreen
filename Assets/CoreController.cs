using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    public GameObject emptyWindowPrefab;
    public GameObject textObject;
    private GameObject _activeWindow;

    //moja sciana
    //public GameObject wallObject; 

    [HideInInspector]
    public GameObject ActiveWindow
    {
        get => _activeWindow;
        set
        {
            if (value && !_windows.Contains(value))
            {
                Debug.LogWarning("Window object is not in windows list");
                _windows.Add(value);
            }
        
            _activeWindow = value;
        }
    }

    private readonly List<GameObject> _windows = new List<GameObject>();
    
    public ArCursor Cursor { private set; get; }

    public void Start()
    {
        Cursor = GetComponent<ArCursor>();
    }

    public void Update()
    {
        Cursor.RaycastCursor();
        CheckNewWindowCreation();
        CheckStandardWindowInteractions();
    }

    /// <summary>
    /// Instantiates new Window from prefab with Instantiate() function.
    /// </summary>
    public GameObject InstantiateEmptyWindow(Vector3 position, Quaternion rotation)
    {
        GameObject newWindow = Instantiate(emptyWindowPrefab, position, rotation);
        newWindow.GetComponent<Window>().app = this; 
        _windows.Add(newWindow);
        return newWindow;
    }

    private void CheckNewWindowCreation()
    {
        if (!InputHandler.clicked()) return;

        if (!Cursor.RaycastCursor() || ActiveWindow) return;

        // canvas
        // setting window on eyesight height
        var cursorPosition = Cursor.transform.position;
        var windowPosition = new Vector3(cursorPosition.x, Camera.main.transform.position.y,
            cursorPosition.z);
        InstantiateEmptyWindow(windowPosition, Cursor.transform.rotation);
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
        if (!Cursor.LastHitInfo.collider) return;
        if (!InputHandler.holding()) return;
        var onAim = Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "Bar":
            {
                    var newPos = Camera.main.transform.position + (Camera.main.transform.forward * 1.0f);
                    var p = onAim.transform.position - ActiveWindow.transform.position;

                    ActiveWindow.transform.position = newPos - p;
                    ActiveWindow.transform.rotation = Camera.main.transform.rotation;
                    break;
            }
            case "FrameLeft":
            {
                    var CursorPosition = Cursor.transform.position;
                    var LocalCursorPosition = ActiveWindow.transform.InverseTransformPoint(CursorPosition);
                    var FramePositionLocal = ActiveWindow.transform.Find("FrameLeft").transform.localPosition;
                    var Difference = FramePositionLocal.x - LocalCursorPosition.x;

                    ActiveWindow.transform.localScale += new Vector3(1f, 0f, 0f) * Difference;
                    //ActiveWindow.transform.localPosition -= new Vector3(1f, 0f, 0f) * Difference / 2.0f;
                    break;
                }
            case "FrameRight":
            {
                    var CursorPosition = Cursor.transform.position;
                    var LocalCursorPosition = ActiveWindow.transform.InverseTransformPoint(CursorPosition);
                    var FramePositionLocal = ActiveWindow.transform.Find("FrameRight").transform.localPosition;
                    var Difference = LocalCursorPosition.x - FramePositionLocal.x;

                    ActiveWindow.transform.localScale += new Vector3(1f, 0f, 0f) * Difference;
                    //ActiveWindow.transform.localPosition += new Vector3(1f, 0f, 0f) * Difference / 2.0f;
                    break;
                }
            case "FrameBottom":
            {
                    var CursorPosition = Cursor.transform.position;
                    var LocalCursorPosition = ActiveWindow.transform.InverseTransformPoint(CursorPosition);
                    var FramePositionLocal = ActiveWindow.transform.Find("FrameBottom").transform.localPosition;
                    var Difference = FramePositionLocal.y - LocalCursorPosition.y;

                    ActiveWindow.transform.localScale += new Vector3(0f, 1f, 0f) * Difference;
                    //ActiveWindow.transform.localPosition -= new Vector3(0f, 1f, 0f) * Difference / 2.0f;
                    break;
            }
        }
        
    }
}