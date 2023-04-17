using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Toolbar : MonoBehaviour
{
    public bool isPanelClicked = true;
    public GameObject cursor;
    [HideInInspector] public CoreController app;

    void Awake()
    {
        app = cursor.GetComponent<CoreController>();

    }
    public void switchApp(GameObject window)
    {
        app.emptyWindowPrefab = window;

    }
    // Start is called before the first frame update
    void Start()
    {
        
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
