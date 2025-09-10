using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthText : MonoBehaviour
{
   
    public int BaseHealth = 200;
    public TextMesh value;
    // Start is called before the first frame update
    void Start()
    {

        value = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        value.text = "Health: " + BaseHealth;
        
    }
}
