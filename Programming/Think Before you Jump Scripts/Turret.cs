using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject firePoint;//this game object is used to spawn the nailPrefab which will hit the player
    public GameObject nailPrefab;
    public float speed = 20f;//determines the speed of the projectile
    public ParticleSystem Spark;
    
    


     void Start()
     {
        StartCoroutine(fireRate()); //starts the timer for the turret
     }

    IEnumerator fireRate()
     {   
        yield return new WaitForSeconds(5f);
        Shoot();
        Debug.Log("Shot Fired");
        StartCoroutine(fireRate());
    }




    void Shoot()
    {     
        Spark.Play();
        GameObject nailStart = Instantiate(nailPrefab, firePoint.transform.position, firePoint.transform.rotation);//this code will allow the nailPrefab to spawn 
        Rigidbody nailProjectile = nailStart.GetComponent<Rigidbody>();//This calls for the nailStarts(nailPrefab) Rigidbody so we can handle the force and speed of the projectile.
        nailProjectile.AddForce(Vector3.right * speed);//This handles the speed of the nailPrefab projectile       
        //link to documentation for Vector3.right: https://docs.unity3d.com/ScriptReference/Vector3-right.html
    }



















}
