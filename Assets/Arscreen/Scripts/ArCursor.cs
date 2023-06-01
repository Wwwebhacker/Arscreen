using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArCursor : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public RaycastHit LastHitInfo { private set; get; }

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
    /// <returns>True if Window Object or AR plane was hit.</returns>
    public bool RaycastCursor()
    {
        Ray ray;
        if (InputHandler.IsUsingGestures)
        {
            var midPoint = CoreController.Instance.getPinchMidPoint();
            var direction = midPoint - CoreController.Camera.transform.position;
            direction.Normalize();
            ray = new Ray(CoreController.Camera.transform.position, direction);
        }
        else
        {
            ray = new Ray(CoreController.Camera.transform.position, CoreController.Camera.transform.forward);
        }
        // 
        
        const float rayLength = 10.0f;

        // check for window hit
        if (Physics.Raycast(ray, 
                out var hit, rayLength))
        {
            LastHitInfo = hit;
            if (Window.IsPartOfWindow(LastHitInfo.collider.gameObject, out var window))
            {
                transform.position = LastHitInfo.point + (LastHitInfo.normal * 0.001f);
                transform.rotation = Quaternion.LookRotation(LastHitInfo.normal);
                transform.Rotate(new Vector3(-90, 0, 0));
                CoreController.Instance.ActiveWindow = window;
                return true;
            }
        }
        
        CoreController.Instance.ActiveWindow = null;
        // check for plane hit
        // Vector2 centerOfScreen = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(ray, hits, TrackableType.Planes);
        if (hits.Count == 0) return false;
        
        transform.position = hits[0].pose.position;
        transform.rotation = hits[0].pose.rotation;
        return true;
    }
}