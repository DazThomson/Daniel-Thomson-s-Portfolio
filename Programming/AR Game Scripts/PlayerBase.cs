using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour
{
    /*Although the script is similar to the EnemyStats script, the player stats must be seperate from each other because
     * they behave differently */
    public int MaxHealth = 200;//Max health set to 200.
    int CurrentHealth;//current health will always be equal to maxhealth. This will allow health to be taken from the base.
    public HealthText HealthCounter;
    public GameObject HealthObject;//A visual representation of how much health the player has.
    public void Start()
    {

        HealthCounter = HealthObject.GetComponent<HealthText>();
        CurrentHealth = HealthCounter.BaseHealth;
    }
    private void OnTriggerEnter(Collider other)//This Trigger checks whether anything with the tag "Enemy" interacts with it
    {
        if (other.tag == "Enemy")
        {

            GameObject counter = GameObject.Find("ScoreAdd");//The gameObject counter has the Score script Attached to it.
            GameObject x = GameObject.Find("Spawnpoints");//Spawnpoints has the WaveSpawner script attached to it.
            Score TakeScore = counter.GetComponent<Score>();
            WaveSpawner spawn = x.GetComponent<WaveSpawner>();
            spawn.EnemiesKilled += 1;//if an enemy reaches the final waypoint(homebase), then the EnemiesKilledCounter will increaseand 
            TakeScore.ScoreCounter -= 10;//10 score is taken off the player if the enemy reaches the base.
           //the gameObject tagged with "Enemy" will get destroyed in order to avoid performance issues.
        }
    }
    public void Health(int DamageTaken)//This function handles what will happen of the players health is less than 0.
    {
        CurrentHealth -= DamageTaken;//similar to the Enemy stats, Health is taken off the player depending on how much damage the Enemy deals to them.
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            BaseDestroyed();


        }


    }
    public void BaseDestroyed()//after the base is destroyed, GameOver scene will load.
    {        
        Destroy(gameObject, 0.5f);//Home base is destroyed after 0.5 seconds.
        SceneManager.LoadScene("GameOver");

    }
}
