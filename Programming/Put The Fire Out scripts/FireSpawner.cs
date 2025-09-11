using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireSpawner : MonoBehaviour
{
    //This script handles the spawn points for the Fire Temp Prefab.
    public Transform[] spawner;//An Array which stores spawn points for the Fire
    //public Colldier2D collider;
    public GameObject[] Fire;//Will store the FireTemp Prefab.


    public float Delaytime;/*This handles the time in which the fire spawns. This value is decreased based on value in SpawnrateIncreaseTime.
                            * lower the value, higher spawn rate*/
    public float SpawnrateIncreaseTime;/*handles the time it takes for spawn rate to increase. This is linked with line 60 value.
                                        * For example, if it's been 20 seconds, Delaytime value is decreased. This will increase the spawn rate for the fire */

    [SerializeField] float minspawndistance;
    [SerializeField] float decreaseamout; //the amout of seconds that delay will be reduced each time the spawn rate time ticks
    [SerializeField] bool Canoverlap; // if this is ticked it will allow for objects to spawn within 
    [SerializeField] int maxcap;
       int currentcap=0;
    

  public GameObject[] currententities;

    private Vector3 location;
    float lastlocation = 0;
    // Start is called before the first frame update
    void awake()
    {
       // spawner = GameObject.FindGameObjectsWithTag("FireSpawn");//calls for spawnpoints.
    }
    public void RestoreSpawner(Vector2 pos)
    {
        for (int i = 0; i < spawner.Length; i++)
        {
            if (pos == new Vector2(spawner[i].position.x, spawner[i].position.y))
            {
                spawner[i].gameObject.SetActive(true);
            }
        }
    }

    public void CheckSpawners(Vector2 pos)
    {
        for(int i=0;i<spawner.Length;i++)
        {
            if(pos == new Vector2( spawner[i].position.x, spawner[i].position.y ) )
            {
                spawner[i].gameObject.SetActive(false);
            }
        }


    }


    public void LessCap()
    {
        currentcap--;
    }

    void Start()
    {
       


        if(!Canoverlap)
        {
            maxcap = spawner.Length;
            currententities = new GameObject[maxcap];
            SpawnFire();
        
        }
        if(Delaytime > 0)
        {
            StartCoroutine(Spawnrate(Delaytime));//References Spawnrate method.      

        }
        //location = Fire[Random.Range(0,Fire.Length)].transform.position;//position is changed based on spawn point.



    }
  
    // Update is called once per frame

    private void SpawnFire()//Method which picks a random spawnpoint within the array and spawns the Fire.
    {
        if(currentcap < maxcap)
        {
 
            for(int i=0;i<20;i++)
            {
                int spawnpoint = Random.Range(0, spawner.Length);
            
                if (!Canoverlap)
                {
                
                    //if there isnt an entity there and the distance between the spawnpoint and the player is greater than the value set, spawn, else look for a different spawn point
                    if (!currententities[spawnpoint] && Vector2.Distance (spawner[spawnpoint].position,GameObject.FindGameObjectWithTag("Player").transform.position) > minspawndistance && spawner[spawnpoint].gameObject.activeSelf) 
                    {
               
                        currententities[spawnpoint] = Instantiate(Fire[Random.Range(0, Fire.Length)], spawner[spawnpoint].position, Quaternion.identity);
                      
                   
                        i = 20;
                        currentcap++;
                    }
                     
                   
                }
                else
                {
                    if (Vector2.Distance(spawner[spawnpoint].position, GameObject.FindGameObjectWithTag("Player").transform.position) > minspawndistance && lastlocation != spawnpoint)
                    {
                        lastlocation = spawnpoint;
                        currentcap++;
                        Instantiate(Fire[Random.Range(0, Fire.Length)], spawner[spawnpoint].position, Quaternion.identity);
                        i = 20;
                    }
                }

            }



            }


        }
           
        
       
      

    


    IEnumerator Spawnrate(float Delay)//Method which handles the rate in which the fire spawns and the time in which it spawns.
    {
        float RateCountdown = SpawnrateIncreaseTime;
        float spawnCountdown = Delay;//Variable which handles the 
        while (true)
        {
            yield return null;
            RateCountdown -= Time.deltaTime;//Counts down from spawn
            spawnCountdown -= Time.deltaTime;

            
            if (spawnCountdown < 0)
            {
                spawnCountdown += Delaytime;
                SpawnFire();
            }

           
            if (RateCountdown < 0 && Delaytime > 3)//increases rate in which fire spawns
            {
                RateCountdown += SpawnrateIncreaseTime;
                Delaytime -= decreaseamout;
            }
        }
        //Forum link which helped me create this script: https://answers.unity.com/questions/1630210/how-to-increase-the-spawn-rate-of-an-object-over-t.html
    }
}