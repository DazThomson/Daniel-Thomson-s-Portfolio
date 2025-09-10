using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //This Script is used to handle what happens if an enemy is within a close proximity to the turret
    public float Firerate;//the rate in which a bullet will spawn into the game.
    public GameObject bullet;
    public Transform Spawnpoint;//the location of where the bullet will spawn.      
    GameObject BulletClone;  
    // Start is called before the first frame update
    public void Start()
    {
       
       
    }
    // Update is called once per frame   
    /*If an enemy enters the box collider trigger surrounding the towers, then a coroutine will start which will handle the rate in which a bullet will spawn in the turret */
    private void OnTriggerEnter(Collider other)
    {        
        if (other.tag == "Enemy")
        {
           
            StartCoroutine(Rate());
            Debug.Log("Trigger hit");
        }       
    }  
    IEnumerator Rate()
    {      
            yield return new WaitForSeconds(Firerate);
            Spawn();
           
    }
    void Spawn()
    {     
       BulletClone = Instantiate(bullet, Spawnpoint.position, Spawnpoint.rotation);
       
    }
}
