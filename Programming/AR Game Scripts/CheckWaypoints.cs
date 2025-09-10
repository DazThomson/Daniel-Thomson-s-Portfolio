using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWaypoints : MonoBehaviour
{
    /*This script is very important because it make sure when the enemy spawns into the environment, it will always move towards the waypoints.
     * Before, i tried to assign the waypoints from the Waypoints prefab folder but the enemy would move away from the level
     * eventhough the positions of the waypoints had the same values as the waypoints within the tower Defense game prefab. 
     * This script i found on a tutorial stopped this problem*/
     
    // Start is called before the first frame update
    //START
    public static Transform[] Waypoints;//the waypoints are static because they'll always be the same.
    //This variable can be refernced in EnemyAIPath.
   

     void Awake()
    {

        Waypoints = new Transform[transform.childCount];//Waypoints takes the childcount of enemy and AI manager gameobject
        for(int i = 0; i  < Waypoints.Length; i++)//the for loop is used to go through all points within waypoint array
        {
            Waypoints[i] = transform.GetChild(i);//this gets the children of the spawn and ai manager script.
        }

    }
    //END: Source https://youtu.be/aFxucZQ_5E4?t=784

}
