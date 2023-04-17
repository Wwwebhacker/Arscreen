using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ScreenOrientation.LandscapeLeft:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case ScreenOrientation.LandscapeRight:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case ScreenOrientation.PortraitUpsideDown:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            default:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

}
