using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTower : MonoBehaviour
{
    public GameObject Tower;
    private Camera arCamera = null;
    private Touch touch;
    public float speed;
    public GameObject moveto;
    
    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    void Move()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            Ray raycastInfo = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(raycastInfo, out hit))
            {
                transform.position = new Vector3(

                transform.position.x + hit.point.x * speed,
                transform.position.y,
                transform.position.z + hit.point.y * speed);
            }
        }


    }
}
