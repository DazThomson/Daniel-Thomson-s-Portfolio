using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARSpawnManager : MonoBehaviour
{

    //Link to tutorial videos which helped me setup spawning an object within a plane: https://www.youtube.com/watch?v=VMjZ70PmnPs
    private ARRaycastManager ray;//the raycast manager will make sure objects can only be spawned on a plane.
    private GameObject spawnedPrefab;// this variable stores the Gameobject that has spawned.
    public ARPlaneManager planeManager;
    [SerializeField]
    private GameObject placedPrefab;//This will store the selected prefab that will spawn.

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()//ARRaycastManager is called before the first frame.
    {
        ray = GetComponent<ARRaycastManager>();
    }
    //START
    bool GetPos(out Vector2 touchPos)//This function handles the players position when they touch the screen.
    {//sends out touchPos for the raycast.
        if(Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;/*touchPos is also referenced in the second if statement in the update function.
                                                   * touchPos will retrieve the position where the player has touched.*/
            return true;
        }

        touchPos = default;
        return false;
    }
    //END:https://www.youtube.com/watch?v=QbhuJwsC22Q

    private void Update()
    {
        /*The update function handles what happens when the user touches the plane.
         * for example, if the player presses on a plane, it will spawn an object which has been assigned in the placedPrefab variable.
         * the raycast is there to make sure the player can't spawn anything outside of the plane. */
        if(!GetPos(out Vector2 touchPos))
        {
            return;
        }       
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)//if the user stops touching within an Ar plane, it will spawn the placedPrefab(In this case, the Tower Defense Game).
        {
            if (ray.Raycast(touchPos, s_Hits, TrackableType.PlaneWithinPolygon))//if the player touches within the plane, then an object will spawn.
            {

                var positionHit = s_Hits[0].pose;//temporary variable which stores information on when the user touches within the raycast(the automatically generated plane)
                //START
                foreach(var plane in planeManager.trackables)//This foreach loop is used to loop through every trackable plane and disable them once the user meet if Statement 
                {
                    plane.gameObject.SetActive(false);//disables the AR plane manager after the environment is spawned.
                }
                planeManager.enabled = false;//to make sure the plane doesn't reappear, the plane will be set to false.
                //END: Source: https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.0/manual/plane-manager.html

                if (spawnedPrefab == null)
                {
                    spawnedPrefab = Instantiate(placedPrefab, positionHit.position, positionHit.rotation);//This will spawn the Tower Defense Game into the Environment based on the players touch position
                }               
            }
        }        
        }
    }

    

