using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class Toolbar : MonoBehaviour
{
    public bool isPanelClicked = true;
    public Button switchAppButton;


    public GameObject appsPanel;
    public CoreController app;
    public MinimizeHandler minimizeHandler;
    private bool isVerticalPanelHidden = false;
    private GameObject defaultAppWindow;
    private Dictionary<string, string> map;
    public void SwitchApp(GameObject window)
    {
        CoreController.Instance.emptyWindowPrefab = window;
        HideVerticalPanel();


        
        ChangeSwitchButtonText(map[window.name + "(Clone)"]);


    }
    

    public void ChangeSwitchButtonText(string text)
    {
        TextMeshProUGUI textMeshProText = switchAppButton.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProText.text = text;
    }
    public void ChangeToolbarVisibility(){
        if(isVerticalPanelHidden){
            ShowVerticalPanel();
            isVerticalPanelHidden = false;
        }
        else{
            HideVerticalPanel();
            isVerticalPanelHidden = true;
        }
    }
    private void HideVerticalPanel()
    {
        appsPanel.SetActive(false);
    }
    private void ShowVerticalPanel()
    {
        appsPanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        map = minimizeHandler.getMap();
        ChangeSwitchButtonText(map[app.GetWindowPrefabName() + "(Clone)"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool IsPartOfToolbar(GameObject obj)
    {
        if (!obj) return false;


        while (!obj.TryGetComponent<Toolbar>(out var _))
        {
            if (!obj.transform.parent) return false;
            obj = obj.transform.parent.gameObject;
        }
        
        return true;
    }
}
