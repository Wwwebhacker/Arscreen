using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Window : MonoBehaviour
{
     [HideInInspector] public CoreController app;

    //this bool tells us in all inherited windows if window is minimized.
    protected bool _minimized = false;


    /// <summary>
    /// Checks if game object is part of Window object.
    /// </summary>
    /// <param name="obj">GameObject to check.</param>
    /// <param name="window">Top GameObject of Window instance.</param>
    /// <returns>True if obj has a parent with attached "Window" script.</returns>
    public static bool IsPartOfWindow(GameObject obj, out GameObject window)
     {
          window = obj;
          if (!obj) return false;
          while (! obj.TryGetComponent<Window>(out var _))
          {
               if (!obj.transform.parent) return false;
               obj = obj.transform.parent.gameObject;
          }
          window = obj;
          return true;
     }

    /*
    protected void CheckButtons()
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
                    screen.transform.localScale = new Vector3(0.45f, 0.25f, 0.01f);
                }
                break;
        }
    }
    */
}
