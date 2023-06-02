using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowWall : Window
{
    new void Update()
    {
        base.CheckStandardWindowInteractions();
        CheckButtons();
    }

    new protected void CheckButtons()
    {
        if (base.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "CloseWindowButton":
                onDestroyButtonClick();
                break;

            case "MinimizeWindowButton":
                //if (Screen != null) Screen.SetActive(!Screen.activeSelf);
                onMinimizeButtonClick();
                break;
        }
    }
}
