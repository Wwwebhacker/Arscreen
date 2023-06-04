using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreController : MonoBehaviour
{
    private GameObject _activeWindow;
    private readonly List<GameObject> _windows = new List<GameObject>();
    
    public GameObject emptyWindowPrefab;
    public GameObject textObject;
    public MinimizeHandler minimizeHandler;
    public static CoreController Instance { get; private set; }

    public static Camera Camera { get; private set; }

    //moja sciana
    //public GameObject wallObject; 

    [HideInInspector]

    public string GetWindowPrefabName()
    {
        if(emptyWindowPrefab != null)
        {
            return emptyWindowPrefab.name;
        }
        return "No Window Name";

    }
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

    public ArCursor Cursor { private set; get; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            this.gameObject.SetActive(false);
            Debug.LogWarning("More than 1 CoreControllers in scene");
        }
        
        if (Camera == null)
        {
            Camera = Camera.main;
        }
    }
    
    public void Start()
    {
        Cursor = GetComponent<ArCursor>();
    }

    public void Update()
    {
        var raycastResult = Cursor.RaycastCursor();
        CheckNewWindowCreation(raycastResult);
    }

    /// <summary>
    /// Instantiates new Window from prefab with Instantiate() function.
    /// </summary>
    private GameObject InstantiateEmptyWindow(Vector3 position, Quaternion rotation)
    {
        if (emptyWindowPrefab.IsUnityNull()) return null;
        var newWindow = Instantiate(emptyWindowPrefab, position, rotation);
        //newWindow.transform.localScale = new Vector3(2,2,2);
        _windows.Add(newWindow);
        return newWindow;
    }

    private void CheckNewWindowCreation(bool raycastResult)
    {
        if (!InputHandler.Clicked()) return;
        if (raycastResult == false || ActiveWindow) return;

        // canvas
        // setting window on eyesight height
        var cursorPosition = Cursor.transform.position;
        var windowPosition = new Vector3(cursorPosition.x, Camera.transform.position.y,
            cursorPosition.z);
        InstantiateEmptyWindow(windowPosition, Cursor.transform.rotation);
    }

    public Vector3 getPinchMidPoint()
    {
        var joints = SkeletonManager.instance._listOfJoints;
        const int indexFingerEnd = 8;
        const int thumbFingerEnd = 4;
        var indexFinger = joints[indexFingerEnd].transform.position;
        var thumbFinger = joints[thumbFingerEnd].transform.position;
        const float center = 0.5f;
        return Vector3.Lerp(thumbFinger, indexFinger, center);
    }

    public void ToggleUsingGestures()
    {
        InputHandler.IsUsingGestures = !InputHandler.IsUsingGestures;
    }
    
}