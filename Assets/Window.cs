using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Window : MonoBehaviour
{
     [HideInInspector] public CoreController app;

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
}
