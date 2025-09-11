using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMove : MonoBehaviour
{
   
    public bool activator = false;//This is set to false because we don't want the lift to move to the end point without triggering the button.   
    public float LiftSpeed;//This variable is used to determine the speed of the lift
    public Transform StartPoint;//This variable is used set the lifts Start Point. This is assigned in the inspector
    public Transform EndPoint;//The position of this object is the lifts end point.

    // Update is called once per frame
    public void Update()//This function
    {

        if (activator == true)  
        {

           transform.position = Vector3.MoveTowards(transform.position, EndPoint.transform.position, Time.deltaTime * LiftSpeed);//If an object is on the button trigger, the lift will move towards the EndPoint
            
        }
        else
        {
           transform.position = Vector3.MoveTowards(transform.position, StartPoint.transform.position, Time.deltaTime * LiftSpeed);//If an object is on the button trigger, the lift will move towards the StartPoint

        }
        //link to documentation which helped me setup this script: https://www.youtube.com/watch?v=TJCOC0gcU4k
        //link to youtube tutorial on moving objects to specific positions: https://www.youtube.com/watch?v=TJCOC0gcU4k


    }
}
