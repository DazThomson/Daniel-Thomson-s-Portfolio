using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloppySpawn : MonoBehaviour
{
    public Transform[] Spawnpoints;
    private int SpawnCap = 6;
    public GameObject FloppyDisk;
    // Start is called before the first frame update
    void Start()
    {
       

    }
    public void OnEnable()
    {
        Debug.Log("Task Started.");
        Spawn(FloppyDisk);

    }
    // Update is called once per frame
   
    public void Spawn(GameObject floppy)//This function will spawn the floppy disks into the environment. 
    {//this function also prevents the chance of more than one floppy disk spawning on the same point.
        List<Transform> OpenSpawnpoints = new List<Transform>(Spawnpoints);//a temporary list is created in order store all open spawnpoints

        for (int i = 0; i < SpawnCap; i++)//this will spawn 3 FloppyDisks into the environment.
        {
            if (OpenSpawnpoints.Count <= 0)//This if statement checks if there are any slots available. This will only occur if there are more floppy disks than spawn points 
            {
                Debug.Log("Error: no slots available");
                return;
            }

            int index = Random.Range(0, OpenSpawnpoints.Count);//index will pick a random position in the list 
            Transform x = OpenSpawnpoints[index];
            OpenSpawnpoints.RemoveAt(index);
            Instantiate(floppy, x.position, Quaternion.identity);//This spawns at a given position. in this case, a random point in spawnpoints array.



        }
        //link to forum which helped me stop the problem with floppys spawning on same point: https://answers.unity.com/questions/65011/multiple-spawn-points-without-spawning-in-the-same.html





    }
}





    

   

