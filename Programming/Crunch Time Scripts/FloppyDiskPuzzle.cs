using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppyDiskPuzzle : MonoBehaviour
{
   
    bool IsPickedUp = false;
    public Transform PlayerPos;
    public float pickupSpeed;
    public StoreValue store;
    public GameObject FloppyDisk;
    int pickupCap = 1;
    int currentCap;
    private List<GameObject> floppyAmount = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        FloppyDisk = GameObject.Find("floppy disk");                      
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPickedUp)
        {
            pickupObject();           
        }         
    }
    public void pickupObject()
    {
       
        FloppyDisk = GameObject.Find("floppy reader");
        FloppyDisk.GetComponent<Collider>().enabled = true;
        store = FloppyDisk.GetComponent<StoreValue>();
        currentCap += 1;               
        store.ObjectStored = true;
        store.SpawnFloppy = true;
        Debug.Log("Floppy Disk Picked up!");
        Destroy(gameObject);
        IsPickedUp = false;

        
                 
        
    }

    public void Interaction()
    {
        IsPickedUp = true;
    }
}
