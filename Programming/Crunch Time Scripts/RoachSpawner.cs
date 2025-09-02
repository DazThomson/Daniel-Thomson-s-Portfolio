using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachSpawner : MonoBehaviour
{
    public Transform[] SpawnLocation;
    public GameObject Cockroach;
    public float Delay;
    public float AliveTime;
    bool isActive = false;
    public int SpawnCount = 20;
    
  
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        StartCoroutine(Spawn());

    }  
    private void Update()
    {

       
    }
   
    IEnumerator Spawn()//this function behaves the same way as the floppyDisk.
    {
       

        for (int y = 0; y < SpawnCount; y++)
        {
            yield return new WaitForSeconds(2f);           

            int i = Random.Range(0, SpawnLocation.Length);//this makes sure the cockroaches will spawn at a random position every time they're spawned.
            Instantiate(Cockroach, SpawnLocation[i].position, Quaternion.identity);//This spawns at a given position. in this case, a random point in spawnpoints array.
            
            

        }                                                   

        //source, https://answers.unity.com/questions/65011/multiple-spawn-points-without-spawning-in-the-same.html
        
    }
  

    

}
