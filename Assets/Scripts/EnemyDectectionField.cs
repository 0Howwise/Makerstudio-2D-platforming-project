using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDectectionField : MonoBehaviour {

    Transform target;
    public float speed = 200f;
    public float aggroRange = 15;
    
    bool isFollowing;
    bool activated = false;
    private Rigidbody2D rigidbody;
    Animator enemyAnim; 
    Enemy enemy;
    PlayerHealth playerHealth;
    
   
    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
        enemy = GetComponent<Enemy>(); 
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector2 direction = (new Vector2(target.position.x, target.position.y) - rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(activated){
            if(rigidbody.velocity.y != 0){
                force.x = 0;
                force.y = 0;
            }
            
            //check if enemy is dead before it tries to move 
            // if (enemy.isDead != true) {
                // if (playerHealth.playerCurrentHealth != 0 ){
                    rigidbody.velocity = new Vector3(force.x, rigidbody.velocity.y);    
                // }  
            // }           
        }

        //getting the distance between the player and the enemy
        float distance = Vector2.Distance(rigidbody.position, target.position);

        //checking to see if the player is out or in ranged to be followed
        if(distance < aggroRange) {
            activated = true;
        } else {
            activated = false;
        }

        //fliping the sprite depending on the direction of the player
        if(rigidbody.velocity.x >= 0.01f){
            transform.localScale = new Vector3 (-1, 1f, 1f);
        } else if (rigidbody.velocity.x <= -0.01f){
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //checking if the larch is moving
        if(rigidbody.velocity.x != 0){
            isFollowing = true;
            enemyAnim.SetBool("isFollowing", true);
        } else {
            isFollowing = false;
            enemyAnim.SetBool("isFollowing", false);
        }
    }
}
