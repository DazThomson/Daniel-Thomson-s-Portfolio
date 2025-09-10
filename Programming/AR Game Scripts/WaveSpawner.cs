using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [Header("Enemies")]
    public GameObject[] Enemy;//GameObject array which stores all Enemy types. This will be used in the line of code which Instantiates the enemy into the environment
    [Header("Spawns and Stats")]
    public Transform Spawnpoint;   
    public int EnemyCap = 0;//cap inside each method which handles the wave.
    public int EnemiesKilled = 0;//This is always set to 0 and will only change when an enemy either reaches the base or is killed by a tower
    public int wave = 0;//the wave number will increase within the countdown function.
    public bool GameStart = false;
    [Header("Spawn Delays and Countdowns")]
    public float WavePrepTime;//this variable changes based on 
    public float Delay;//Spawn delay on Enemy.
    public float WaveCooldown = 10f; //this value is how much time it will take for the first wave to spawn.     
    bool waveCountdown = true;
    [Header("Other")]
    public WaveCounter addWaveValue;//Wavecounter is a script which gives the player a reminder of what wave they're on.
    public bool hasWavePrepped = false;//This is always set to false at runtime in order to make sure the EnemiesKilled if statement in Update is only ever active once the player gets into the scene.   
    public Touchbutton moveTowers;
    void Start()
    {
        //StartCoroutine(FirstWave());//First wave is called instantly.  
        StartCoroutine(WavePrep());
        GameObject findTower = GameObject.FindGameObjectWithTag("Tower");
       // moveTowers = findTower.GetComponent<Touchbutton>();


    }
    void Update()
    {
       if(hasWavePrepped == true)
        {
            if (EnemiesKilled >= EnemyCap)//if Enemies killed is more than the EnemyCap(EnemyCap changes based on what wave the user is in) CountDown is called.
            {
                CountDown();
            }

        }        
    }
    IEnumerator WavePrep()
    {
        yield return new WaitForSeconds(WavePrepTime);
        wave1();
        hasWavePrepped = true;//checkEnemiesKilled is set to true in order to access EnemiesKilled >= EnemyCap method within the if statement.
    }
    void CountDown()//This function is called whenever the if statement in the update function is met. 
    {
        WaveCooldown -= Time.deltaTime;//This allows the number assigned to countdown to 0. This can be seen in the Project inspector.
        if (waveCountdown == true)
        {
           
            //moveTowers.canMove = true;

            //if the wavecooldown gets to 0 or below, it will call the WaveManager Method.
            if (WaveCooldown <= 0)
            {
                GameObject addValue = GameObject.Find("Wave");//Once the method is called, the user 
                addWaveValue = addValue.GetComponent<WaveCounter>();
                addWaveValue.WaveNumber++;

                wave++;//the value of wave is added up by 1 in order to access the if statements in the WaveManager method
                WaveManager();
                WaveCooldown = 10f;//This resets the counter back to it's original number.
                              
            }           
        }
    }
    void wave1()//this method is called within the WaveManager Method.
    {
        //moveTowers.canMove = false;
        StartCoroutine(FirstWave());
    }
    void Wave2()//this method is called within the WaveManager Method.
    {
        //moveTowers.canMove = false;
        StartCoroutine(SecondWave());
    }
    void Wave3()//This is called within the WaveManager Method
    {
        //moveTowers.canMove = false;
        StartCoroutine(ThirdWave());
    }
    void Wave()//This is called within the WaveManager Method
    {
       // moveTowers.canMove = false;
        StartCoroutine(NewWave());
    }

    void WaveManager()//the wave Manager handles which wave the player id supposed to be on.
    {
        /*Wave one is never mentioned in this method because the player 
         * is automatically put into this wave after 15 seconds of prep.*/
        if(wave == 2)
        {
            Wave2();
        }       
        if (wave == 3)
        {
            Wave3();
        }
        else if(wave > 3)
        {
            Wave();
        }
    }   
    void EnemySpawn()//Method designed to handle the enemies spawning in the map.
    {
        Instantiate(Enemy[Random.Range(0, Enemy.Length)], Spawnpoint.position, Quaternion.identity);
        /*This will spawn an Enemy from the Enemy Array from random position within the array.
         * This means that each wave will be random which makes for more dynamic gameplay */
    }

    /*All IEnumerator functions below behave the same way. The only difference is the amount of enemies which spawn in each wave.
     * the function called NewWave will always be called after the 3rd wave (stated in the WaveManager script) */
    IEnumerator FirstWave()
    {
        
       

        //START             
        wave = 1;//wave number is set to 1
        EnemyCap = 3;//cap is set 3. this means only 3 enemies will be able to spawn in this wave.
        EnemiesKilled = 0;//EnemiesKilled is always reset to 0 to prevent the if statement in the update from calling every frame.

        for (int i = 0; i < EnemyCap; i++)//this for loop is used to control the amount of enemies can spawn in the wave. at the moment, only 3 random enemies should spawn in the first wave.
        {
            EnemySpawn();
            yield return new WaitForSeconds(2f);
            
        }                          
        //END: Source: https://www.youtube.com/watch?v=gtVQDqFdabs&t=117s
    }        
        IEnumerator SecondWave()//The second wave is only called once the player has defeated the first wave of enemies.
        {            
            wave = 2;
            EnemyCap = 5;
            EnemiesKilled = 0;
           
            for (int i = 0; i < EnemyCap; i++)
            {
            yield return new WaitForSeconds(2f);
            EnemySpawn();
           
            }                   
        }
        IEnumerator ThirdWave()
        {
            wave = 3;
            EnemyCap = 9;
            EnemiesKilled = 0;
            for (int i = 0; i < EnemyCap; i++)
            {
                EnemySpawn();
                yield return new WaitForSeconds(2f);
            }
           
        
        }
        IEnumerator NewWave()
        {
            EnemyCap += 1;
            EnemiesKilled = 0;
            for (int i = 0; i < EnemyCap; i++)
            {
                EnemySpawn();
                yield return new WaitForSeconds(2f);

            }           
        }
    }

