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
        base.CheckButtons();
        this.CheckYellowWindowButtons();
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
                this.HandleBar(onAim);
                break;
            case "FrameLeft":
                base.HandleLeftFrame(onAim, difference);
                break;
            case "FrameRight":
                base.HandleRightFrame(onAim, difference);
                break;
            case "FrameBottom":
                base.HandleFrameBottom(onAim, difference);
                break;
        }
    }

    new protected void HandleBar(GameObject onAim)
    {
        if (attachedToWall == false)
        {
            /*
            var newPos = CoreController.Camera.transform.position + (CoreController.Camera.transform.forward * 1.0f);
            var p = onAim.transform.position - transform.position;
            transform.position = newPos - p;
            transform.rotation = CoreController.Camera.transform.rotation;
            */
            base.HandleBar(onAim);
        }
        else
        {
            var cursorPosition = CoreController.Instance.Cursor.transform.position;
            var wallCursorPosition = transform.parent.transform.InverseTransformPoint(cursorPosition);
            var wallBarPosition = transform.parent.transform.InverseTransformPoint(onAim.transform.position);
            var walloffset = wallBarPosition - transform.localPosition;

            transform.localPosition = Vector3.right * wallCursorPosition.x + Vector3.up * (wallCursorPosition.y - walloffset.y) + Vector3.forward * transform.localPosition.z;
        }
    }

    private void CheckYellowWindowButtons()
    {
        if (base.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "ChangeColourButton":
                //TODO dodac zmiane kolorow kartek
                break;

            case "DetachButton":
                if (attachedToWall == true)
                {
                    transform.localScale *= 2;
                    transform.localPosition -= new Vector3(0, 0, 0.5f);
                    transform.parent = null;
                    attachedToWall = false;
                    transform.Find("DetachButton").gameObject.SetActive(false);
                }
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

                transform.Find("DetachButton").gameObject.SetActive(true);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
    
    }
}
