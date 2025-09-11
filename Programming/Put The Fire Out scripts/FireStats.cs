using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStats : MonoBehaviour
{
    PlayerStats playerstats;
    public int Health;//Stores the value of the fire's health. can be changed in project inspector      
    float Delay = 1.0f;//adds a delay to the fire's damage over time. 
    public GameObject smokeeffect;
    bool damaged = false;//set to continue loop
    AudioSource firesound;
    public GameObject scorePoint;

    private void Start()
    {
        firesound = GetComponent<AudioSource>();
        playerstats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }


    IEnumerator DamDelay(HouseHealth hit)//This contains a while loop will loop the HouseDamageTaken reference until the house is destroyed. 
    {
        while(damaged)
        {
            
            yield return new WaitForSeconds(Delay);//every 2 seconds, damage is ticked.
            hit.HouseDamageTaken(1);//References the HouseDamageTaken method in the HouseHealth script.
            Debug.Log("die");
            if (hit.HouseHP <= 0)
            {
            
                GameObject.Find("FireSpawner").SendMessage("CheckSpawners", new Vector2(transform.position.x,transform.position.y) );
                hit.SaveSpawner(transform.position);
                Health = 99;
                StartCoroutine(Dying());
            }
            
        }
    }
    private void Update()
    {
        if(Vector2.Distance( playerstats.transform.position,transform.position) < 1.5f )
        {
            firesound.Play();
        }
        else
        {
            firesound.Stop();

        }
    }

    public void Damagetaken(int Damage)//Function which allows the object to take damage.
    {           //This method should be referenced in the player Health script.
            Health -= Damage;             
        if (Health <= 0)//if the value of Health is less than 0, then the Fire will be Destroyed
        {
            Instantiate(scorePoint, transform.position, Quaternion.identity);
            Health = 99;
            StartCoroutine(Dying());
            //here michal 
            playerstats.addHealth();
            playerstats.addPoints(20);
        }
    }
    IEnumerator Dying()
    {
        while(transform.localScale.x > .2f)
        {
            yield return null;
            transform.localScale = new Vector3(transform.localScale.x - .1f, transform.localScale.y - .1f, transform.localScale.z);
        }
        Instantiate(smokeeffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        OnDestroy();//Calls the OnDestroy method which sends a message to decrease the cap on the amount of fires that can spawn.
    }

    private void OnTriggerEnter2D(Collider2D houseHit)
    {
        if (houseHit.tag == "House")//if an object with the tag "house" is hit, then it will call the method DamDelay
        {
            damaged = true;
            StartCoroutine(DamDelay(houseHit.gameObject.GetComponent<HouseHealth>()));/*This script will reference the HouseHealth script.    
                                                                                       * StartCoroutine will be handle the damage ticks within the DamDelay Inumerator script*/
        }
        

    }
    private void OnTriggerExit2D(Collider2D collision)//The OnTriggerExit2D is called when the house has been destroyed.
    {
        if(collision.tag == "House")/* if the trigger collides with an object which has a house tag on the frame it's destroyed,.
                                     * Then the damaged variable will be set to false. this will stop the fire from damaging. */
        {
            damaged = false;
        }
    }
    private void OnDestroy()
    {
        if (GameObject.Find("FireSpawner"))
        {
            GameObject.Find("FireSpawner").SendMessage("LessCap");
        }

    }


}
