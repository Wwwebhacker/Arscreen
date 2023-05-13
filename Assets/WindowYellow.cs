using System.Collections;
using System.Collections.Generic;
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
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;

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
                    screen.transform.localScale = _savedScaleOfWindow;
                }
                break;
            case "MoveToWall":
                //to napewno nie jest dobrze zrobione ale zobaczmy czy dziala
                //TODO
                GameObject anyWall = GameObject.Find("Wall");
                if (anyWall != null)
                    app.ActiveWindow.transform.parent = anyWall.transform;
                break;
        }
    }
}
