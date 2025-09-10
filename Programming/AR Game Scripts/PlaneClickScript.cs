using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneClickScript : MonoBehaviour
{
    public ARRaycastManager ray;
    public ARPlaneManager planeManager;
    public Camera arCamera;

    public GameObject Tower;
    public GameObject Player;
    public GameObject spawnedObject;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();
   
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        if (ray.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
        {
            ARPlane plane = planeManager.GetPlane(hits[0].trackableId);

            if(Input.GetTouch(0).phase == TouchPhase.Stationary && spawnedObject == null)
            {
                spawnedObject = Instantiate(Tower, hits[0].pose.position, Quaternion.identity);

                plane.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }

            else if(Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = hits[0].pose.position;

                plane.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;

                plane.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            }

            else if(Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                spawnedObject = Instantiate(Player, hits[0].pose.position, Quaternion.identity);

            }
        }
       
    }
}
