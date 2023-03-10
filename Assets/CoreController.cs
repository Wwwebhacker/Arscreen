using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreController : MonoBehaviour
{

    public GameObject emptyWindowPrefab;


    private GameObject _activeWindow;

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

    public GameObject taskBar;

    public readonly List<Action> InputHandlers = new List<Action>();

    public void Start()
    {
        Cursor = GetComponent<ArCursor>();
        // Add basic input handlers
        InputHandlers.Add(CheckNewWindowCreation);
        InputHandlers.Add(CheckStandardWindowInteractions);
    }

    public void Update()
    {
        Cursor.RaycastCursor();
        foreach (var handler in InputHandlers)
        {
            handler();
        }
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
        if (Input.touchCount == 0) return;
        if (Input.GetTouch(0).phase != TouchPhase.Began) return;
        if (!Cursor.RaycastCursor() || ActiveWindow) return;
        
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
        if (Input.touchCount == 0) return;
        if (! ActiveWindow) return;

        var onAim = Cursor.LastHitInfo.collider.gameObject;

        if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
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
            }
        }
    }
}