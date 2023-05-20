using UnityEngine;

public class InputHandler
{
    public const bool IsUsingGestures = false;
    
    public static bool Clicked()
    {
        if (IsUsingGestures)
        {
            ManomotionManager.Instance.ShouldCalculateGestures(true);
            bool result = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info
                .mano_gesture_trigger == ManoGestureTrigger.CLICK;
            return result;
        }
        else return Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public static bool Holding()
    {
        if (IsUsingGestures)
        {
            ManomotionManager.Instance.ShouldCalculateGestures(true);
            bool result = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info
                .mano_gesture_continuous == ManoGestureContinuous.HOLD_GESTURE;
            return result;
        }
        else return Input.touches.Length > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary;
    }
}