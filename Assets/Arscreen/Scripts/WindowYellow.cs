using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowYellow : Window
{
    bool attachedToWall = false;
    private int ColorIndex = 0;
    public Material[] ScreenColorMaterial;
    public Material[] BarColorMaterial;
    public Material[] ButtonsColorMaterial;

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
                //PROBA NAPRAWY TEKSTU
            case "Screen":
                this.HandleScreen(onAim);
                break;
        }
    }

    new protected void HandleBar(GameObject onAim)
    {
        if (attachedToWall == false)
        {
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

    protected void HandleScreen(GameObject onAim)
    {
        transform.Find("KeyBoard Canvas").gameObject.SetActive(true);
    }



    new private void CheckButtons()
    {
        if (base.CheckConditionsForButtons() == false) return;

        var onAim = CoreController.Instance.Cursor.LastHitInfo.collider.gameObject;

        switch (onAim.name)
        {
            case "CloseWindowButton":
                onDestroyButtonClick();
                break;
            case "MinimizeWindowButton":
                onMinimizeButtonClick();
                break;
            case "HideWindowButton":
                this.onHideButtonClick();
                break;

            case "ChangeColorButton":
                ColorIndex += 1;
                if (ColorIndex >= 3) ColorIndex = 0;
                Screen.GetComponent<MeshRenderer>().material = ScreenColorMaterial[ColorIndex]; //sizeofScreenColormaterial tutaj zamiast 3?
                Bar.GetComponent<MeshRenderer>().material = BarColorMaterial[ColorIndex];
                CloseWindowButton.GetComponent<MeshRenderer>().material = ButtonsColorMaterial[ColorIndex];
                HideWindowButton.GetComponent<MeshRenderer>().material = ButtonsColorMaterial[ColorIndex];
                MinimizeWindowButton.GetComponent<MeshRenderer>().material = ButtonsColorMaterial[ColorIndex];
                transform.Find("ChangeColorButton").gameObject.GetComponent<MeshRenderer>().material = ButtonsColorMaterial[ColorIndex];
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

    new protected void onHideButtonClick()
    {
        if (Screen != null) Screen.SetActive(!Screen.activeSelf);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall" && attachedToWall == false)
        {
            if (CoreController.Instance.ActiveWindow.transform.parent != other.gameObject.transform)
            {
                CoreController.Instance.ActiveWindow.transform.SetParent(other.gameObject.transform);
                CoreController.Instance.ActiveWindow.transform.localScale /= 2;

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
