using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int ScoreCounter = 0;//score counter set to zero
    public TextMesh TotalScore;//
    // Start is called before the first frame update
    
    //START
    void Start()
    {
        TotalScore = GetComponent<TextMesh>();//This will allow the score text to be altered. without this, the score can't be added or subtracted from the score counter.
        
    }
    // Update is called once per frame
    void Update()
    {
        TotalScore.text = "Score: " + ScoreCounter;//The score is updated when either an enemy is destroyed by the tower or if an enemy reaches the base.    
    }
    //END: Source: https://www.youtube.com/watch?v=QbqnDbexrCw
}
