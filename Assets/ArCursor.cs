using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArCursor : MonoBehaviour
{
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private GameObject objectUnderCursor = null;
    // Start is called before the first frame update
    void Start()
    {
        cursorChildObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursor();
        checkInput();
    }

    void checkInput()
    {
        if (objectUnderCursor)
        {
            Debug.Log($"objectUnderCursor: {objectUnderCursor}");

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                
                var newPos = Camera.main.transform.position + (Camera.main.transform.forward * 0.5f);
                objectUnderCursor.transform.position = newPos;
                objectUnderCursor.transform.rotation = Camera.main.transform.rotation;
            }
        }
        else
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log($"objectUnderCursor: {objectUnderCursor}");
                if (objectUnderCursor)
                {
                    var renderer = objectUnderCursor.GetComponent<Renderer>();
                    renderer.material.SetColor("_Color", Color.red);
                }
                else
                {
                    var objectPos = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
                    GameObject.Instantiate(objectToPlace, objectPos, transform.rotation);
                }

            }
        }

        

        
    }

    void UpdateCursor()
    {

        if (updateScreenHit())
        {
            return;
        }

        updatePlaneHit();


    }

    bool updateScreenHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5.0f))
        {
            if (hit.collider.name == "Screen(Clone)")
            {
                objectUnderCursor = hit.collider.gameObject;
                transform.position = hit.point + (hit.normal * 0.001f);
                transform.rotation = Quaternion.LookRotation(hit.normal);
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                return true;
            }
        }
        objectUnderCursor = null;
        return false;
    }

    void updatePlaneHit()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {           
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }
}
