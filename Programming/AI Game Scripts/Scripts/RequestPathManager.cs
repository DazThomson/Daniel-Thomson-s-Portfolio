using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RequestPathManager : MonoBehaviour
{
    //With the help of  
    Queue<RequestingPath> pathQueue = new Queue<RequestingPath>();/*We use a Queue to recieve all
                                                                   * all data types inside the struct which allows the program to process the information it's been provided
                                                                   * In this case, the queue is receiving both the enemy's starting point of the path and it's end point
                                                                   * which is the players last known location. */ 
    RequestingPath currentPath;//called to reference certain datatypes in the struct.
    // Start is called before the first frame update
    static RequestPathManager WaypointInstance;
    PathFinding pathfinder;
    bool ProcessPath;// a bool which checks whether to process the path. this is set to false if the path is  completed 
    //and set to true to process another path request given.
    

    private void Awake()
    {
        WaypointInstance = this;
        pathfinder = GetComponent<PathFinding>();
    }


    public static void PathRequest(Vector2 start, Vector2 end, Action<Vector2[], bool> callback)
    {
        RequestingPath newPath = new RequestingPath(start, end, callback);//calls for argument in Requesting path struct and retrieves vector2 start, end and bool callback
        WaypointInstance.pathQueue.Enqueue(newPath);
        WaypointInstance.NextProcess();

    }

    public void NextProcess()//
    {
        if (!ProcessPath && pathQueue.Count > 0)
        {
            currentPath = pathQueue.Dequeue();//takes item out of queue
            ProcessPath = true;
            pathfinder.StartPath(currentPath.startPath, currentPath.EndPath);//call StartPath in pathfinding script.
        }
    }

    //Once the Enemy AI has completed processing the path in the grid, the Next process is called, creating a loop in which the Pathfinder (Enemy) is constantly calling for the players current position.
    public void FinishedPathProcess(Vector2[] path, bool success)
    {
        currentPath.callback(path, success);
        ProcessPath = false;
        NextProcess();

    }


    //I am using a struct in this program becasue it's a great way of storing data structures which have static values. this way is easier to call strcutures over classes because 
    //data is stored on a stack compared to classes are stored on a heap.
    //This will help with holding important information for the AI, their start point and end point
    //Simpler way of storing value types could improve performance.
    struct RequestingPath
    {
        public Vector2 startPath;
        public Vector2 EndPath;
        public Action<Vector2[], bool> callback;//the action takes 2 parameter types vector2[] array and a bool to check position of the enemy in the path and to check whether the path has been processed successfully.

        public RequestingPath(Vector2 _start, Vector2 _end, Action<Vector2[], bool> _callback)
        {
            startPath = _start;
            EndPath = _end;
            callback = _callback;
        }

    }

}
