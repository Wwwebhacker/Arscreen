using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class WindowWithPicture : Window
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
        //

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
                    screen.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else if (_minimized == true)
                {
                    _minimized = false;
                    GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
                    screen.transform.localScale = new Vector3(0.45f, 0.25f, 0.01f);
                }
                break;
        }
    }
}
