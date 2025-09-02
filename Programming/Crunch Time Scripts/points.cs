using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    //script used to store information on endPoint's position in the scene.
    public static Transform point;
    public Transform endPoint;
    public static Transform[] Waypoints;
    public Transform[] AllPoints;
    // Start is called before the first frame update
    void Awake()
    {
        point = endPoint;
        Waypoints = AllPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
