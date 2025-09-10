using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform SpawnLocation;//where the enemy will spawn
    public GameObject Enemy;//The Enemu GameObject is assigned in the Project inspector.
    public float SpawnDelay;//spawn delay will be used to add a delay to an enemy spawning.       
    GameObject Spawnpoint;
   //This simple spawner script will only be seen in the start menu as a way of giving the player a taster of how the game will play out.

    
    void Start()
    {           
        StartCoroutine(Delay());       
    }

    IEnumerator Delay()//This IEnumerator will run in an endless loop since it's consistently getting called.
    {      
            yield return new WaitForSeconds(SpawnDelay);
            SpawnEnemy();
            StartCoroutine(Delay());              
    }  
    public void SpawnEnemy()
    {        
        Spawnpoint = Instantiate(Enemy, SpawnLocation.position, SpawnLocation.rotation);//This will create a clone of the Enemy.                       
    }
  
}
