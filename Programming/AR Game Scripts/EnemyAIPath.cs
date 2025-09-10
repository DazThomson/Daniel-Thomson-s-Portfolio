using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIPath : MonoBehaviour
{
    private int currentWaypoint;//a private counter which increase once the enemy reaches a waypoint   
    public float EnemySpeed;//this value can be assigned in the inspector. used to determine the speed of the Enemy
    Transform point; //point is used to store information for the next waypoint they're going to .  
    Vector3 moveposition;
    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;

        point = CheckWaypoints.Waypoints[currentWaypoint];//point checks for the first waypoint it's supposed to go towards.
        transform.LookAt(point.position);//makes sure the enemy is always looking at the waypoint it's going to.
                     
    }

    // Update is called once per frame
    void Update()
    {
        //START
        if (Vector3.Distance(transform.position, point.position) <= 0.1f)
        {
                  
            if (currentWaypoint >= CheckWaypoints.Waypoints.Length - 1)
            {
                Destroy(gameObject);
                return;
            }
            currentWaypoint++;
            point = CheckWaypoints.Waypoints[currentWaypoint];
            transform.LookAt(point.position);
        }
        //transform.position = Vector3.MoveTowards(transform.position, point.position, Time.deltaTime * EnemySpeed);
        transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
        //END: Source: Super Easy Patrolling AI | Unity Tutorial: https://www.youtube.com/watch?v=22PZJlpDkPE 
        //2nd Source: How to make a Tower Defense Game (E02 Enemy AI) - Unity Tutorial: Time: 18:25 https://youtu.be/aFxucZQ_5E4?t=1115
        //The script created here has been created with the help of these 2 tutorials. the first link helped me by getting the enemy to move around the scene.
        // the second link helped me by making sure the enemy is moving to the right positions.
    }



}
