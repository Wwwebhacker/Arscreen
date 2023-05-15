using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class WindowWall : Window
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
        _savedScaleOfScreen = screen.transform.localScale;

    }

    void Update()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        // Conditions
        if (app.ActiveWindow != gameObject) return;
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
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
                    _savedScaleOfScreen = screen.transform.localScale;
                    screen.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
                else if (_minimized == true)
                {
                    _minimized = false;
                    GameObject screen = app.ActiveWindow.transform.Find("Screen").gameObject;
                    screen.transform.localScale = _savedScaleOfScreen;
                }
                break;
        }
    }
}
