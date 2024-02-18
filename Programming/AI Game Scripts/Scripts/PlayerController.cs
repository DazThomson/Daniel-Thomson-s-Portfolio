using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody2D;
    private Vector3 Direction;
    public float speed;
    // Start is called before the first frame update
    void Awake()
    {

        rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0.0f;
        float y = 0.0f;

        if(Input.GetKey(KeyCode.W))
        {
            y = +1f;

        }
        if (Input.GetKey(KeyCode.A))
        {
            x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x = +1f;
        }

        Direction = new Vector3(x, y).normalized;

    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Direction * speed;
    }
}
