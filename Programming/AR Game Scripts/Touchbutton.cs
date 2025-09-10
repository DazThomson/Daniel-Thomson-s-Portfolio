using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchbutton : MonoBehaviour
{
    // Start is called before the first frame update\

    private Touch touch;//Touch variable which allows player to use touch as input.
    public float speed;//controls the speed in which the player can move the object with their finger.    
    // Update is called once per frame
    public bool canMove;
    private Camera GameCamera;
    void Start()
    {
        GameCamera = Camera.main;
        
    }
    void Update()/*This will let the player move an object around the environment
                  * Link to video which helped me understand how to achieve this: https://youtu.be/3_CX-KtsDic */
    {
        //if (canMove == true)
       // {


            //Start

            if (Input.touchCount > 0)//if there is more than one touch count, then Input get touch is called.
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)//this will allow an Object to move around the level .
                {
                    transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speed,  
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * speed);//This code here handles the way in which the Object will move based on the value of speed.

                }
            }

       // }
       // else
       // {
          //  return;
       // }
        //End: Source https://youtu.be/3_CX-KtsDic
    }
}
