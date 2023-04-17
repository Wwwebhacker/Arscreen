using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArCursor : MonoBehaviour
{

    private CoreController _app;
    public ARRaycastManager raycastManager;
    public RaycastHit LastHitInfo { private set; get; }

    public void Start()
    {
        _app = GetComponent<CoreController>();
    }
    
    /// <summary>
    /// Raycast in 2 steps:
    /// <list type="number">
    /// <item>Looks for Window instance.</item>
    /// <item>Looks for the AR plane.</item>
    /// </list>
    /// If Window GameObject was found then following would be set:
    /// <list type="bullet">
    /// <item>ActiveWindow property of CoreController to top GameObject of Window instance.</item>
    /// <item>LastHitInfo property of ArCursor which provides more info about Raycast hit.</item>
    /// </list>
    /// If Window was not found but Ar plane was then following would be set:
    /// <list type="bullet">
    /// <item>ActiveWindow property of CoreController to <b>null</b>.</item>
    /// </list>
    /// In both cases position and rotation of cursor will be updated.
    /// </summary>
    /// <returns>True if Window Object of AR plane was hit.</returns>
    public bool RaycastCursor()
    {
        // check for window hit
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, 
                out var hit, 5.0f))
        {
            LastHitInfo = hit;
            if (Window.IsPartOfWindow(LastHitInfo.collider.gameObject, out var window))
            {
                transform.position = LastHitInfo.point + (LastHitInfo.normal * 0.001f);
                transform.rotation = Quaternion.LookRotation(LastHitInfo.normal);
                transform.Rotate(new Vector3(-90, 0, 0));
                _app.ActiveWindow = window;
                return true;
            }

            
        }
        
        _app.ActiveWindow = null;
        // check for plane hit
        Vector2 centerOfScreen = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(centerOfScreen, hits, TrackableType.Planes);
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
            return true;
        }
        return false;

    }
}