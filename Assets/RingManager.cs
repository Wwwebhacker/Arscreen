using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{

    [SerializeField] private FingerInfoGizmo _fingerInfoGizmo;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        ManomotionManager.Instance.ShouldCalculateGestures(true);
        ManomotionManager.Instance.ShouldRunFingerInfo(true);
        ManomotionManager.Instance.ToggleFingerInfoFinger(4);

        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class == ManoClass.GRAB_GESTURE)
        {
            _fingerInfoGizmo.ShowFingerInformation();
            float centerPosition = 0.5f;
            Vector3 ringPlacement = Vector3.Lerp(_fingerInfoGizmo.LeftFingerPoint3DPosition,
                _fingerInfoGizmo.RightFingerPoint3DPosition, centerPosition);
            GameObject.Find("Finger").transform.position = ringPlacement;
        }
        
        
    }
}
