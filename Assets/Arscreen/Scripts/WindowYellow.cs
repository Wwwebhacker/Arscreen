using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowYellow : MonoBehaviour
{

    void Update()
    {
        CheckButtons();
    }
    private void CheckButtons()
    {
        // Conditions
        if (CoreController.Instance.ActiveWindow != gameObject) return;
        if(! InputHandler.Clicked()) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;
        switch (onAim.name)
        {
            case "WallButton":
                //TODO
                //GameObject anyWall = GameObject.Find("WindowWall").gameObject;// zatrzymuje dalsze wykonanie kodu
                //GameObject anyWall = GameObject.Find("WindowWall");
                onAim.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(255.0f, 0.0f, 0.0f);

                Debug.Log("TEST");
                //Destroy(anyWall);
                //Destroy(app.transform.Find("WindowWall").gameObject);
                Debug.Log(GameObject.Find("WindowWall")); //zwraca: WindowYellow:CheckButtons()...
                Debug.Log(GameObject.Find("WindowWall").name); //zwraca:
                Debug.Log(GameObject.Find("WindowWall").gameObject); //zwraca null
                Debug.Log(GameObject.Find("WindowWall").gameObject.name); //zwraca: WindowYellow:CheckButtons()...

                Debug.Log("TEST2");
                Debug.Log(CoreController.Instance.transform.Find("WindowWall"));
                Debug.Log(CoreController.Instance.transform.Find("WindowWall").name);
                Debug.Log(CoreController.Instance.transform.Find("WindowWall").gameObject);
                Debug.Log(CoreController.Instance.transform.Find("WindowWall").gameObject.name);

                //app.ActiveWindow.transform.parent = anyWall.transform;
                //app.ActiveWindow.transform.SetParent(anyWall.transform);
                break;
        }
    }
    //JJ test

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (CoreController.Instance.ActiveWindow.transform.parent != other.gameObject.transform)
            {
                CoreController.Instance.ActiveWindow.transform.SetParent(other.gameObject.transform);
                CoreController.Instance.ActiveWindow.transform.localScale /= 2;
                
                Destroy(CoreController.Instance.ActiveWindow.transform.Find("Bar").gameObject);
                CoreController.Instance.ActiveWindow.transform.localPosition = new Vector3(0, 0, -0.025f);
                CoreController.Instance.ActiveWindow.transform.rotation = Quaternion.identity;
                CoreController.Instance.ActiveWindow.transform.rotation = other.gameObject.transform.rotation;
            }

            //Destroy(app.ActiveWindow);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        /*
        if (other.gameObject.tag == "Wall")
        {
            app.ActiveWindow.transform.localPosition = new Vector3(app.ActiveWindow.transform.localPosition.x, app.ActiveWindow.transform.localPosition.y, -0.25f);
        }
        */
    }
}
