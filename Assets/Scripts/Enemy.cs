using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 1;                // The amount of health taken away per attack.
	public int damageOnContact;                 // Damage doen when the player touches the enemy and not being attackted
    public int startingHealth = 100;            // Enemies health when spawned
    public int currentHealth;                   // Health enemy has in it current instance
    //public CircleCollider2D enemyHurtBox;       // Collider for taking damage and damging player
    //public BoxCollider2D enemyDectionField;     // Collider/field in which the player can dectect the player. NOT for collision

    
    Animator animator;                          // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    private Animator enemyAnim;                 // Enmies animator
    
    bool canAttack;                             // Whether player is within the trigger collider and can be attacked.
    bool playerInRange;                         // If player enter the enemyDectionField or not.   
    public bool isDead;                         // checks if enemy is dead
    float timer;   
    public float knockBackAmmount;              // Timer for counting up to the next attack.
    public GameObject deathEffect;  
    Rigidbody2D rigidbody;          //   
    // public BoxCollider2D enemyDectionField;     // This is a trigger collider used to dectect enemies 
    
    // public GameObject enemyDectionField;
    // public CircleCollider2D enemyHurtBox;

    void Awake() {
        //Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        isDead = false;
    }


    void OnTriggerEnter2D(Collider2D collider) {
        // If the entering collider is the player...
        if (collider.gameObject == player)
        {
            // ... the player is in close enough to be attacked.
            canAttack = true;

            //Playing the attack animation           
            // animator.SetTrigger("isAttackingTrigger");
            // animator.SetBool("isAttacking", true);
      
            //If the player walk into this trigger collider the enemy will start to follow the player.
            //animator.SetBool("isFollowing", true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // If the exiting collider is the player...
        if (collider.gameObject == player)
        {
            // ... the player is no longer in range.
            canAttack = false;

            //stoping the attacking animation
            // animator.SetBool("isAttacking", false);
        }
    }
  
    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && canAttack && currentHealth > 0)
        {
            // ... attack.
            Attack();
        }

        //checking if player is alive. If not resetting all animations
        // if(playerHealth.playerCurrentHealth == 0){
        //     // animator.SetBool("isAttacking", false);
        //     // animator.SetBool("idling", true);
        // } 
    }

    //recieves a damage int and 
    public void TakeDamage(int damage)
    {
        //Enemy fliches when taking damage
        animator.SetTrigger("isHit");

        startingHealth -= damage;
        Vector2 direction = (new Vector2(player.transform.position.x, player.transform.position.y) - rigidbody.position).normalized;
        Vector2 force = direction * knockBackAmmount * Time.deltaTime;

        //applying knockback
        rigidbody.AddForce(-force);
       
       //checking if enemy is dead
        if (startingHealth <= 0)
        {
            Die();
        }   
    }

    void Die() {
        isDead = true;
        // animator.SetBool("isDead", true);
        //preforming VFX when the enemy dies
        // Instantiate(deathEffect, transform.position, Quaternion.identity);

        //TODO: Add particle effects to replace this system
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("larchIsDead"))
        {
            //deleting the enemy from the game space
            Destroy(gameObject);
        }
    }

    void Attack() {
        // Reset the timer.
        timer = 0f;

       if(!isDead) {
            // If the player has health to lose...
            if (playerHealth.playerCurrentHealth > 0) {
                // ... damage the player.
                playerHealth.TakeDamage(attackDamage, gameObject);
            }
       }
    }
}
