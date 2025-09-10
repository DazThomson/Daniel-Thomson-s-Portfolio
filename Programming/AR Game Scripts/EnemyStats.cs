using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    
    public int health;//Health assigned to the Enemy. This can be changed in the project inspector
    int currentHealth;//The 
    int counter;
    PlayerBase healthValue;
    HealthText HealthUI;
    public int EnemyDamage;   
    [SerializeField]
    private WaveSpawner spawn;    
  
    // Start is called before the first frame update
    void Start()
    {
        
        currentHealth = health;
        spawn = GetComponent<WaveSpawner>();
      
                 
    }
    // Update is called once per frame
    public void CheckHealth(int damagetaken)//This script is used to check how much health the enemy is on
    {//This particular function is called in the bullet script which will allow the enemy to take damage.
        currentHealth -= damagetaken;//This makes sure currenthealth is always -= to how much damage the bullet deals
        if(currentHealth < 1)//if the health is below 1, then the EnemyDied script is called which gices score to the player and adds 1 to the enemies killed counter in the WaveSpawner script.
        {
            currentHealth = 0;
            EnemyDied();
        }
    }
    public void EnemyDied()
    {
        GameObject y = GameObject.Find("ScoreAdd");//finds the GameObject called ScoreAdd which holds the Score script.
        GameObject x = GameObject.Find("Spawnpoints");
        WaveSpawner spawn = x.GetComponent<WaveSpawner>();
        Score AddScore = y.GetComponent<Score>();
        spawn.EnemiesKilled+=1;//adds 1 to Enemies killed integer in WaveSpawner script.
        AddScore.ScoreCounter += 10;//10 score is added when an enemy is killed.
        Destroy(gameObject);//GameObject is destroyed after AddScore to make sure the points added before it's destroyed.
     
     

    }
    private void OnTriggerEnter(Collider other)//This scripts detects for any object with the tag Player and deals damage to the player
    {
        if(other.tag == "Player")
        {
            Damage();

        }
    }
    public void Damage()
    {
        GameObject DamagePlayer = GameObject.Find("Home");
        GameObject LosePlayerHealth = GameObject.Find("Health");
        healthValue = DamagePlayer.GetComponent<PlayerBase>();
        HealthUI = LosePlayerHealth.GetComponent<HealthText>();
        healthValue.Health(EnemyDamage);//EnemyDamage can be changed due to different any types in the game.
        HealthUI.BaseHealth -= EnemyDamage;//This will show health getting taken off the player by referncing the BaseHealth int in HealthUI.     
    }
}
