using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowYellow : Window
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
        _savedScaleOfWindow = screen.transform.localScale;
    }
    void Update()
    {
        CheckButtons();
    }
    private void CheckButtons()
    {
        // Conditions
        if (app.ActiveWindow != gameObject) return;
        if(! InputHandler.clicked()) return;

        var hit = app.Cursor.LastHitInfo;
        switch (hit.collider.name)
        {
            case "CloseWindowButton":
                Destroy(app.ActiveWindow);
                break;

            case "MinimizeWindowButton":
                if (_minimized == false)
                {
                    _minimized = true;
                    GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
                    _savedScaleOfScreen = screen.transform.localScale;
                    screen.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else if (_minimized == true)
                {
                    _minimized = false;
                    GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
                    screen.transform.localScale = _savedScaleOfWindow;
                }
                break;

            case "WallButton":
                //TODO
                //GameObject anyWall = GameObject.Find("WindowWall").gameObject;// zatrzymuje dalsze wykonanie kodu
                //GameObject anyWall = GameObject.Find("WindowWall");
                hit.collider.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(255.0f, 0.0f, 0.0f);

                Debug.Log("TEST");
                //Destroy(anyWall);
                //Destroy(app.transform.Find("WindowWall").gameObject);
                Debug.Log(GameObject.Find("WindowWall")); //zwraca: WindowYellow:CheckButtons()...
                Debug.Log(GameObject.Find("WindowWall").name); //zwraca:
                Debug.Log(GameObject.Find("WindowWall").gameObject); //zwraca null
                Debug.Log(GameObject.Find("WindowWall").gameObject.name); //zwraca: WindowYellow:CheckButtons()...

                Debug.Log("TEST2");
                Debug.Log(app.transform.Find("WindowWall"));
                Debug.Log(app.transform.Find("WindowWall").name);
                Debug.Log(app.transform.Find("WindowWall").gameObject);
                Debug.Log(app.transform.Find("WindowWall").gameObject.name);

                //app.ActiveWindow.transform.parent = anyWall.transform;
                //app.ActiveWindow.transform.SetParent(anyWall.transform);
                break;
        }
    }
    //JJ test

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (app.ActiveWindow.transform.parent != other.gameObject.transform)
            {
                app.ActiveWindow.transform.SetParent(other.gameObject.transform);
                app.ActiveWindow.transform.localScale /= 2;
                
                Destroy(app.ActiveWindow.transform.Find("Bar").gameObject);
                app.ActiveWindow.transform.localPosition = new Vector3(0, 0, -0.025f);
                app.ActiveWindow.transform.rotation = Quaternion.identity;
                app.ActiveWindow.transform.rotation = other.gameObject.transform.rotation;
            }

            //Destroy(app.ActiveWindow);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        /*
        if (other.gameObject.tag == "Wall")
        {
            app.ActiveWindow.transform.localPosition = new Vector3(app.ActiveWindow.transform.localPosition.x, app.ActiveWindow.transform.localPosition.y, -0.25f);
        }
        */
    }
}
