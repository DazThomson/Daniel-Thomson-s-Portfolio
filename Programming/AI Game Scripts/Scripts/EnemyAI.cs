using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Name of Video: A* Pathfinding (E05: units);
    //Source to Tutorial used: https://www.youtube.com/watch?v=dn1XRIaROM4&t=157s
    //Date Accessed: 29th June 2022.
    public Transform Player;//Enemy's target is the player.
    public Vector2[] pathInfo;//empty Vector2 array which will be used to initialise the Enemy's position within the grid
    int waypointIndex;// the waypointIndex will be added within the while loop in Follow
    public int Enemyspeed;//this will allow me to alter the speed of the AI in the inspector
    bool complete;
   // 
    void Start()//I'm calling the path calculation 
    {
        //StartCoroutine(RecalculatePath());
        Setup();//Setup is called first which handles the coroutine. This allows the 
    }

    //IEnumerator is used to repeat the path request over time. In this case, Path Request will be repeated every .5 seconds which is better than
    //Repeating it every frame. I initially but the pathManager on the Update function but it ended up causing issues
    //any value lower than this will cause the AI to stutter and could cause performance issues

    void Setup()
    {
        StartCoroutine(RecalculatePath());
    }
    IEnumerator RecalculatePath()
    {              
            yield return new WaitForSeconds(.2f);
            RequestPathManager.PathRequest(transform.position, Player.position, pathFound);                  
    }
     


    public void pathFound(Vector2[] newPathLayout, bool pathComplete)
    {
        if(pathComplete)//if initial path is complete, then the follow coroutine will be stopped then started to avoid data overflow.
        {
            pathInfo = newPathLayout;
            waypointIndex = 0;
            StopCoroutine(Follow());
            StartCoroutine(Follow());//start 
        }
    }
    IEnumerator Follow() //
    {
        Vector2 current = pathInfo[0];
        while (true)
        {
            if((Vector2)transform.position == current)
            {
                waypointIndex++;
                if (waypointIndex >= pathInfo.Length)
                {
                    Setup();//calls setup function which starts 
                    yield break;
                }
                current = pathInfo[waypointIndex];
            }
            transform.position = Vector2.MoveTowards(transform.position, current, Enemyspeed * Time.deltaTime);/*This line of code will allow the Enemy to move towards the player based on it's
                                                                                                      * current position in the grid and the speed in which it's going. */          
            yield return null;                       
           
        }
       
    }
    public void OnDrawGizmos()//This Draw gizmos function will  visualize how the AI is behaving (set path)
    {
        if(pathInfo!= null)
        {
            for (int i = waypointIndex; i < pathInfo.Length; i++)//This for loop is used to check the waypointIndex and check if the value is lower than the Vector2 Coordinates
            {
                Gizmos.color = Color.green;//The gizmos line is set to cyan to give a clear distinction between the colors in debug mode

                if (i == waypointIndex)//checks whether the index is equal to i then it will draw the line between the player and enemy
                {
                    Gizmos.DrawLine(transform.position, pathInfo[i]);

                }
                else
                {
                    Gizmos.DrawLine(pathInfo[i - 1], pathInfo[i]);
                }

            }
            
                    
        }
    }

}
