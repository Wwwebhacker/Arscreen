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

        }


    }
}
