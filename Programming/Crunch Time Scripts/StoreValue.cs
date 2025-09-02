using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreValue : MonoBehaviour
{
    //This script is designed to store the FloppyDisk once it gets picked up by the player. 
    //the object is destroyed 
    public bool ObjectStored = false;    
    public GameObject floppyDisk;    
    public Transform Startpoint;
    public bool canInteract = false;
    public int current = 0;
    public bool SpawnFloppy = false;
   
    public float range = 2f;
    public LayerMask Interact;
  
    // Start is called before the first frame update  
    void Start()
    {
        

    }
   
    void Update()
    {
        if (ObjectStored == true)//this if statement is used to prevent the object from floppy disk from spawning infinitely.
        {
             
                if (canInteract)//if statement
                {
                    
                    
                    SpawnObject();
                }                                                  
        }
    }   
    // Update is called once per frame       
    public void SpawnObject()
    {       
        if(SpawnFloppy == true)
        {
            Instantiate(floppyDisk, Startpoint.position, Quaternion.identity);
            SpawnFloppy = false;
            

        }
        canInteract = false;
        ObjectStored = false;

    }

    public void Interaction()
    {
        canInteract = true;

    }
    private void OnDisable()
    {
        canInteract = false;
        this.GetComponent<BoxCollider>().enabled = false;
    }

}
