using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachAI : MonoBehaviour
{
   
    public float Speed = 0;
    public int current;//current set to 0 automatically at start.
    Transform pos;//variable stores waypoints array.  
    public GameObject Player;
    public float DespawnCountdown = 5f;
    public bool spawnhit = false;
    public LayerMask Layer;
    public float hitbox;
    public bool path = false;
    public AudioSource squish;
    public float flee;
   

    // Start is called before the first frame update

    void Start()
    {
        
        Speed = Random.Range(1, 4);
        current = 0;
        if(current<points.Waypoints.Length )
        {
            pos = points.Waypoints[current];//direct reference to the static variable called Waypoints in points script. 
        
       
        transform.LookAt(pos.position);
        squish = GetComponent<AudioSource>();
        path = true;

        }


    }

    // Update is called once per frame
    void Update()//This update function will handle the cockroach's usual path and what happens if the player is within range of the cockroach
    {
        RaycastHit ray;
       if(path)
        {
            if (Vector3.Distance(transform.position, pos.position) < 0.3f)//i
            {

                if (current >= points.Waypoints.Length - 1)
                {
                    current = 0;
                    return;
                }
                current++;
                pos = points.Waypoints[current];
                transform.LookAt(pos.position);
            }
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        }
           
            if (Physics.Raycast(transform.position, transform.forward, out ray, hitbox, Layer))//raycast used as a hitbox for the cockroach. if the player walks over the cockroach, audio will play.
            {
                ScurryAway();

            }
           
            //source: https://www.youtube.com/watch?v=22PZJlpDkPE      


    }
    public void CockroachDeath()
    {
        squish.Play();
        Destroy(gameObject, 0.3f);

    }
    public void ScurryAway()//function created which gets the cockroach to scurry away from the player. 
    {
       
            DespawnCountdown -= Time.deltaTime;//countdown begins which despawns the cockroach.                 
            transform.position = Vector3.back * flee * Time.deltaTime; 
            if(DespawnCountdown <= 0)//once the countdown reaches 0, the cockroach is destroyed.
            {
            CockroachDeath();

            }              
    }
   private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CockroachDeath();
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, hitbox);
    }
   
}
