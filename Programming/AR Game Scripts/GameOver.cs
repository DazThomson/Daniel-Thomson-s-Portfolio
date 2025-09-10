using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private Camera GameOverCamera;
    //public LayerMask layer;//this ensures that the raycast applied to the button can only interact with things on a certain layer.
    //This script will allow the player to interact with 3D objects in the environment through a raycast.
    // Start is called before the first frame update
    void Start()
    {
        GameOverCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray PauseRay = GameOverCamera.ScreenPointToRay(Input.GetTouch(0).position);//uses camera to detect where the player is touching on screen to see if it's hitting a ray or not.

            RaycastHit pressedPause;

            if (Physics.Raycast(PauseRay, out pressedPause))//if the players finger hits a ray, then it will load a the sample scene again.
            {
                SceneManager.LoadScene("SampleScene"); 
            }


        }
    }
}
