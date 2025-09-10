using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    public TextMesh Wave;
    public int WaveNumber = 1;
    // Start is called before the first frame update
    //S
    void Start()
    {
        Wave = GetComponent<TextMesh>();        
    }

    // Update is called once per frame
    void Update()
    {
        Wave.text = "Wave: " + WaveNumber;        
    }
}
