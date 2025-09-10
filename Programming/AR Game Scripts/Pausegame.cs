using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pausegame : MonoBehaviour
{
    private Camera MainCamera = null;
    private bool GamePaused = false;
    public LayerMask layer;
   

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main; 
    }
    // Update is called once per frame
    void Update()
    {
       /* if (Input.touchCount == 0)
        {
            return;
        }
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray PauseRay = MainCamera.ScreenPointToRay(Input.GetTouch(0).position);

            RaycastHit pressedPause;

            if (Physics.Raycast(PauseRay, out pressedPause, layer))
            {
                //Start
                if (GamePaused)
                {
                    Time.timeScale = 1;
                    GamePaused = false;
                }
                else
                {
                    Time.timeScale = 0;
                    GamePaused = true;
                }
                //End
            } 





        }
       */
    }
    public void pauseGame()//This function will be accessed by a button which will allow the player to pause and unpause the game with ease.
    {
       
        if(GamePaused)
        {
            Time.timeScale = 1;
            GamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            GamePaused = true;

        }
            

    }
    public void ResetGame()//This function will reload the scene to restart the main level scene.
    {
        SceneManager.LoadScene("SampleScene");
    }

}
