using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowYellow : Window
{
    bool attachedToWall = false;

    new void Update()
    {
        this.CheckStandardWindowInteractions();
        this.CheckButtons();
    }

    new protected void CheckStandardWindowInteractions()
    {
        if (base.CheckConditionsForStandardWindowInteractions() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;
        var cursorPosition = CoreController.Instance.Cursor.transform.position;
        var localCursorPosition = transform.InverseTransformPoint(cursorPosition);
        var localFramePosition = onAim.transform.localPosition;
        var difference = localCursorPosition - localFramePosition;

        switch (onAim.name)
        {
            case "Bar":
                {
                    if (attachedToWall == false)
                    {
                        var newPos = CoreController.Camera.transform.position + (CoreController.Camera.transform.forward * 1.0f);
                        var p = onAim.transform.position - transform.position;
                        transform.position = newPos - p;
                        transform.rotation = CoreController.Camera.transform.rotation;
                    }
                    else
                    {
                        //onAim.transform.localPosition = new Vector3(localCursorPosition.x, localCursorPosition.y, -0.025f); // -dziala idealnie ale trzeba caly obiekt a nie sam bar

                        //werja oparta o 
                        var tmpX = Vector3.right * difference.x;
                        var tmpY = Vector3.up * difference.y;

                        transform.Translate(tmpX + tmpY);
                        //transform.localPosition += tmpX + tmpY;
                        //transform.position += tmpX +tmpY;
                    }
                    break;
                }
            case "FrameLeft":
                {
                    var tmp = Vector3.right * difference.x;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameBottom, Bar })
                    {
                        element.transform.localScale -= tmp;
                        element.transform.Translate(tmp / 2.0f);
                    }
                    break;
                }
            case "FrameRight":
                {
                    var tmp = Vector3.right * difference.x;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameBottom, Bar })
                    {
                        element.transform.localScale += tmp;
                        element.transform.Translate(tmp / 2.0f);
                    }
                    transform.Find("CloseWindowButton").gameObject.transform.Translate(tmp);
                    transform.Find("MinimizeWindowButton").gameObject.transform.Translate(tmp);
                    break;
                }
            case "FrameBottom":
                {
                    var tmp = Vector3.up * difference.y;
                    onAim.transform.Translate(tmp);
                    foreach (var element in new GameObject[] { Screen, FrameRight, FrameLeft })
                    {
                        if (element == Screen) element.transform.localScale += tmp;
                        else element.transform.localScale -= tmp;
                        element.transform.Translate(tmp / 2.0f);
                    }
                    break;
                }
        }
    }

    new private void CheckButtons()
    {
        if (base.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "CloseWindowButton":
                Destroy(gameObject);
                break;

            case "MinimizeWindowButton":
                Screen.SetActive(!Screen.activeSelf);
                break;

            case "ChangeColourButton":
                //TODO dodac zmiane kolorow kartek
                break;

            //zamienic na nowy przycisk ktory
            case "WallButton":
                if (attachedToWall == true)
                {
                    transform.parent = null; //SPRAWDZIC CZY DOBRZE
                    transform.localScale *= 2; //zrobic samo transform bez active window instance i tak dalej
                    transform.position -= new Vector3(0,0,-0.25f);
                    //remove the new button
                }
                //onAim.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(255.0f, 0.0f, 0.0f);
                //Debug.Log("TEST");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" && attachedToWall == false)
        {
            if (CoreController.Instance.ActiveWindow.transform.parent != other.gameObject.transform)
            {
                CoreController.Instance.ActiveWindow.transform.SetParent(other.gameObject.transform);
                CoreController.Instance.ActiveWindow.transform.localScale /= 2;

                //Destroy(CoreController.Instance.ActiveWindow.transform.Find("Bar").gameObject);
                attachedToWall = true;

                CoreController.Instance.ActiveWindow.transform.localPosition = new Vector3(0, 0, -0.025f);
                CoreController.Instance.ActiveWindow.transform.rotation = Quaternion.identity;
                CoreController.Instance.ActiveWindow.transform.rotation = other.gameObject.transform.rotation;
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
    
    }
}
