using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArCursor : MonoBehaviour
{
    public GameObject cursorChildObject;
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private Collider colliderUnderCursor = null;
    public Texture2D originalTexture;
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
        if (colliderUnderCursor)
        {
            Debug.Log($"objectUnderCursor: {colliderUnderCursor}");

            if (colliderUnderCursor.gameObject.name == "Bar")
            {
                var screen = colliderUnderCursor.gameObject.transform.parent;
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
                {

                    var newPos = Camera.main.transform.position + (Camera.main.transform.forward * 1.0f);
                    var p = colliderUnderCursor.gameObject.transform.position - screen.position;

                    screen.transform.position = newPos - p;
                    screen.transform.rotation = Camera.main.transform.rotation;
                }
                return;
            }
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
                {
                    Renderer rend = hit.transform.GetComponent<Renderer>();

                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    Debug.Log(meshCollider);

                    if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                        return;

                    Texture2D tex = rend.material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    pixelUV.x *= tex.width;
                    pixelUV.y *= tex.height;

                    tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
                    tex.SetPixel((int)pixelUV.x, (int)pixelUV.y + 1, Color.red);
                    tex.SetPixel((int)pixelUV.x, (int)pixelUV.y - 1, Color.red);
                    tex.SetPixel((int)pixelUV.x + 1, (int)pixelUV.y, Color.red);
                    tex.SetPixel((int)pixelUV.x - 1, (int)pixelUV.y, Color.red);

                    tex.Apply();
                }
            }

        }
        else
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {

                var objectPos = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
                var obj = GameObject.Instantiate(objectToPlace, objectPos, transform.rotation);
                obj = obj.transform.GetChild(1).gameObject;
                var renderer = obj.GetComponent<Renderer>();
                Texture2D copyTexture = new Texture2D(originalTexture.width, originalTexture.height);
                copyTexture.SetPixels(originalTexture.GetPixels());
                copyTexture.Apply();
                renderer.material.EnableKeyword("_NORMALMAP");
                renderer.material.SetTexture("_MainTex", copyTexture);


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

    bool colliderIsScreen(Collider collider)
    {
        if (collider.gameObject.transform.parent)
        {
            if (collider.gameObject.transform.parent.name == "Screen(Clone)")
            {
                return true;
            }
        }
        return false;
    }

    bool updateScreenHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5.0f))
        {
            if (colliderIsScreen(hit.collider))
            {
                colliderUnderCursor = hit.collider;
                transform.position = hit.point + (hit.normal * 0.001f);
                transform.rotation = Quaternion.LookRotation(hit.normal);
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                return true;
            }
        }
        colliderUnderCursor = null;
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
