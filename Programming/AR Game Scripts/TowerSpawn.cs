using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class TowerSpawn : MonoBehaviour
{
    private Touch touch;    
    [SerializeField]
    private GameObject Tower;//object is picked in inspector   
    public LayerMask layer;//LayerMask is used to make sure the tower can only spawn in a certain layer
    
    //public GameObject ObjectSpawned;
    private Camera arCamera = null;
    //public GameObject positionmove;
    private GameObject spawnedTower;//variable which stores the spawned tower.
    private List<GameObject> TowerAmount = new List<GameObject>(); //list used to store how many towers have been spawned into the game.
    [SerializeField]
    private int maxcap = 4;//this sets the tower spawn cap to 4
    private int currentamount;//an unassigned variable which changes based on whether a tower has spawned or not.
    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;//when the environment is spawned by the user, this will be called to check for a camera in the scene with the MainCamera tag attached to it.
    }
    // Update is called once per frame
    void Update()
    {

        SpawnAtTouchPos();//calls for this method when the player touches within the radius
    }
    void SpawnAtTouchPos()
    {
        if(Input.touchCount == 0)
        {
            return;
        }
        /*Raycast is used to make sure the player can't spawn anything outside the designated area.
         * ray. checks whether the user has touched within the raycasts limits. if it has, then the tower will spawn. */
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Start
            Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);//ScreenPointToRay is used to return a ray from a camera from where the user touches on the screen.
            RaycastHit hit;
            //End: link to Tutorial video which helped me create touch to spawn script using raycast: https://www.youtube.com/watch?v=JfpMIUDa-Mk
            
            if (Physics.Raycast(ray, out hit, layer))
            {
                //start
                if (currentamount < maxcap)//the if statement will prevent the user from building too many towers for performance.
                {
                    spawnedTower = Instantiate(Tower, hit.point, Quaternion.identity);
                    TowerAmount.Add(spawnedTower);
                    currentamount++;
                }
                //if(Input.touches[0].phase == TouchPhase.Moved)
               // {
                 //    transform.position = new Vector3(
                 //    transform.position.x + Tower.transform.position.x * speed * Time.deltaTime,
                 //    transform.position.y,
                 //    transform.position.z + Tower.transform.position.y * speed * Time.deltaTime);
               // }
                //End: link video which talks how to implementing a spawn cap. Starts at 5:47; https://youtu.be/phDbAMYVkzw?t=347
            }
        }
       
    }
    //link to unity documentation on ScreenPointToRay: https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html

    //START
    public void ChangeTowerType(GameObject TowerType)//This particular function is attached to buttons which allows the player to switch between the two tower types
   {
        Tower = TowerType;   
   }
    //ENDhttps://www.youtube.com/watch?v=phDbAMYVkzw&t=68s
}
