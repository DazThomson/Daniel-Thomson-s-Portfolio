using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject start;
    public Transform Target;
    public float Bulletspeed;
    public bool canDamage = false;
    public EnemyStats Damage;
    public int BulletDamage;
  
    

    // Start is called before the first frame update
    void Start()
    {
        //Enemy = GameObject.FindGameObjectsWithTag("Enemy");//Calls for any object with the tag Enemy. This gives the user the oppurtunity
       
        InvokeRepeating("UpdateEnemy", 0f, 0.5f);
        //StartCoroutine(Destroybullet());
      
    }
    void UpdateEnemy()//This function is used to make sure the bullet that's spawned is always tracking the enemy closest to the turret. This makes the game fair for both the player and it prevents the game from being too easy
    {
        //Start
        GameObject[] All = GameObject.FindGameObjectsWithTag("Enemy");
        float ClosestEnemy = Mathf.Infinity;
        GameObject nearbyEnemy = null;
        foreach(GameObject enemy in All)//Foreach enemy with the tag "Enemy", the bullet will check for the enemy's position and bullets position.
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < ClosestEnemy)
            {
                ClosestEnemy = distance;
                nearbyEnemy = enemy;
            }
        }
        if (nearbyEnemy != null)
        {
            Target = nearbyEnemy.transform;
        }

        //End Source: Brackeys: https://www.youtube.com/watch?v=QKhn2kl9_8I&t=449s

    }
    IEnumerator Destroybullet()//This functions purpose is to destroy the bullet after a certain amount of time. This will prevent bullets from staying in the environment even when there are no enemies left.
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
        StartCoroutine(Destroybullet());
    }


    // Update is called once per frame
    void Update()
    {
        if(Target == null)//if there is no target detected, then the object will get destroyed.
        {
            Destroy(gameObject);
            return;
        }
        
        start.transform.LookAt(Target.transform.position);//the projectile will always be looking at the Enemy's position on the game.
        //START
        if (Vector3.Distance(Target.position, start.transform.position) <= 0.2f)
        {
            start.transform.position += (Target.transform.position - start.transform.position).normalized * Bulletspeed * Time.deltaTime;//This will move the Projectile towards based on the value entered in BulletSpeed.
           
            start.transform.LookAt(Target.transform.position);
           

        }
        transform.Translate(Vector3.forward * Bulletspeed * Time.deltaTime);//this line of code will move the bullet towards the player after the if statement has passed.

        //END Source: https://www.youtube.com/watch?v=ZXtyh43AZlU
    }
    private void OnTriggerEnter(Collider other)//OnTrigger will check for any object which has the Enemy tag on it. if it does, then canDamage is set to true 
    {    
        if(other.tag == "Enemy")
        {
            canDamage = true;
            DamageEnemy(other.gameObject.GetComponent<EnemyStats>());//calls for the EnemyStats script in order to deal damage to the enemy.

        }
                       
    }
    

    void DamageEnemy(EnemyStats Damage)//EnemyStats is called to handle what happens when the projectile touches the Enemy.
    { 
      if(canDamage == true)
      {
            
        Damage.CheckHealth(BulletDamage);//every 0.5 seconds, the bullet will deal 30 damage. 
        Destroy(gameObject);
      }
      if(canDamage == false)
      {
            Destroy(gameObject);
            
      }
     
      
    }
}
