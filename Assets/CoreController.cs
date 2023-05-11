using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoreController : MonoBehaviour
{

    public GameObject emptyWindowPrefab;
    public GameObject textObject;
    


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


        ManomotionManager.Instance.ShouldCalculateGestures(true);
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger !=
            ManoGestureTrigger.CLICK) return;
        //     if (Input.touchCount == 0) return;
        // if (Input.GetTouch(0).phase != TouchPhase.Began) return;

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
                //TODO
                case "FrameLeft":
                {
                        //tu zmienic rozmiary w lewo

                        break;
                }
                case "FrameRight":
                {
                        //tu zmienic rozmiary w prawo
                        //var newPosition = Camera.main.transform.position + (Camera.main.transform.forward * 1.0f);
                        //var currentPosition = ActiveWindow.transform.position;
                        //var p = onAim.transform.position - ActiveWindow.transform.position;

                        //Debug.Log("newPosition: " + newPosition);
                        //Debug.Log("currentPosition: " + currentPosition);
                        //Debug.Log("p: " + p);


                        //ActiveWindow.transform.localScale = new Vector3(p.x - newPosition.x, 1, 1);

                        //pseudo dzialajace
                        //ActiveWindow.transform.position += new Vector3(0f, 0f, 1f) * 1.5f / 2.0f;
                        //ActiveWindow.transform.localScale += new Vector3(1f, 0f, 0f) * 1.5f;

                        var CursorPosition = Cursor.transform.position; //dodac to forwad?
                        var FramePosition = ActiveWindow.transform.Find("FrameRight").transform.position;
                        //problem to jest pozycja wobec obiektu rodzica nie obiektu wobec calej przestrzeni! albo i nie?
                        var difference = CursorPosition.x - FramePosition.x;

                        Debug.Log("difference" + difference); 
                        
                        Debug.Log("Camera Position: " + Camera.main.transform.position);
                        Debug.Log("Window Position: " + ActiveWindow.transform.position);
                        Debug.Log("Frame  Position: " + ActiveWindow.transform.Find("FrameRight").transform.position);

                        //Debug.Log("--Wyniki--");
                        //Debug.Log("Window - Camera" + (Camera.main.transform.position - ActiveWindow.transform.position));
                        //Debug.Log("Window - Camera" + (Camera.main.transform.position - ActiveWindow.transform.position));



                        //ActiveWindow.transform.position += new Vector3(0f, 0f, 1f) * (CameraPosition-FramePosition) / 2.0f;
                       // ActiveWindow.transform.localScale += new Vector3(1f, 0f, 0f) * difference; //czy na pewno x?
                        //ActiveWindow.transform.position += new Vector3(1f, 0f, 0f) * difference;

                        /*
                        Debug.Log("Camera Position: " + Camera.main.transform.position);
                        Debug.Log("Window Position: " + ActiveWindow.transform.position);
                        Debug.Log("Frame Right position: " + ActiveWindow.transform.Find("FrameRight").transform.position);

                        Debug.Log("--Wyniki--");
                        Debug.Log("Window - Camera" + (Camera.main.transform.position - ActiveWindow.transform.position));
                        Debug.Log("Window - Camera" + (Camera.main.transform.position - ActiveWindow.transform.position));
                        */
                        break;
                }
            }
        }
    }
}