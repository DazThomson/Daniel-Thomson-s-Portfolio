using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //this script moves the floppy disk into the driver.
    Transform end;
    public float Speed;//speed of the floppydisk moving.
    Rigidbody moveFloppy;
    bool done;
    public string Tag;//can be set in project inspector

    // Start is called before the first frame update
     void Start()
    {
        /*i am calling both a static Transform called point because it's something which is never changing.
         * also, without the check script in the scene, the object which is spawned in won't behave as intended. 
         * for example, it will most likely not move towards the point.*/      
        end = points.point;
        transform.LookAt(end.position);//makes sure the object is always looking towards the point.       
       


    }

    // Update is called once per frame
    public void Update()
    {
       //if the floppy disk clone is within 0.2 range of the point, the object will disappear and a log will be sent to the console which says its backed up game.
        if(Vector3.Distance(transform.position, end.position) <= 0.1f)
        {
            transform.LookAt(end.position);//will always look at end.
            Debug.Log("Game backed up");//sent to console 

           
            DestroyFloppy();
            StartCoroutine(TaskDone(done));//once the floppy disk reaches the point, the task is completed.
            Destroy(gameObject);//gameObject will be destroyed after reaching destination.
        }
        transform.LookAt(end.position);//This ensures that the object is always looking it's target
        transform.position = (Vector3.MoveTowards(transform.position, end.position, Time.deltaTime * Speed));//Vector3.MoveTowards will move the object towards the 
        //moveFloppy.AddForce(Vector3.forward * Speed);//will move the object towards the end position(object called end)
        

        
    }
    void DestroyFloppy()
    {
        GameObject[] Disks = GameObject.FindGameObjectsWithTag(Tag);//temporary array which finds all game objects with the tag "Floppy"
        foreach(GameObject floppy in Disks)//foreach loop which loops through all "Floppy" tags and destroys them.
        {
            Debug.Log("All floppys destroyed");
            Destroy(floppy);//floppy disks are destroyed to prevent null reference exceptions.
        }

    }
    IEnumerator TaskDone(bool failed)//Method which completes the task.
    {
        while (GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<TaskOrganizer>().busy)//calls for tags that have 
        {
            yield return null;
        }
        GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<TaskOrganizer>().RemoveTask(GameObject.Find("floppy reader"), failed);

    }

}
