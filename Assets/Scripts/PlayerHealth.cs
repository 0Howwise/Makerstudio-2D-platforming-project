using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	//Tracking our current health so we know how many health containers to show
	public int playerCurrentHealth;
	//Tracking our total health so we know how many total to show, or how many empty to show when damaged
	//this will also ensure we don't ever heal over our maximum health
	public int playerTotalHealth;

	// flag to ensure death only occurs once
	public bool canDie = true;

	//checks if player is dead
	public bool isDead;

	//Graphic for our full container. I like to use game object in case later we want to make
	//it fancier or add a particle effect, we can just add it to the game object
	public GameObject healthContainerFull;
	//Same thing but for the empty graphic
	public GameObject healthContainerEmpty;
	//Reference to our health bar in the game. I use rectTransform so that we know it isn't just any old game object,
	//but that it's a UI element. Unity won't let us assign anything but a UI element here now.
	public RectTransform healthBar;
	//Keep track of how many health slots we have and which ones are full or empty
	public List<GameObject> healthSlots;
	//keep a reference to the objects we spawned in to our health bar (full/empty)
	List<GameObject> spawnedSlots = new List<GameObject>();

	public GameObject death;

	private Animator playerAnimator;

	private Animation deathAnim;

	Player player;

	GameObject playerGO;


	private void Awake() {
		//Populate our health bar at the start
		CalculateHealthBar();
	}

	void Start() {
		playerAnimator = GetComponent<Animator>();	
		player = GetComponent<Player>();
		playerGO = transform.gameObject;
	}

	void Update() {
		if (playerCurrentHealth <= 0 && canDie) {
			Die();
		}
	}

	//This could probably be split and optimized, but for now it works fairly modularly
	public void CalculateHealthBar() {
		//First, remove any health containers we've spawned in so we can start fresh
		foreach (GameObject g in spawnedSlots) {
			Destroy(g);
		}
		//clear our list so we can start over and repopulate
		healthSlots.Clear();
		//first determine our total health slots and pass in the empty container
		for (int i = 0; i < playerTotalHealth; i++) {
			healthSlots.Add(healthContainerEmpty);
		}
		//then replace all of the empties with full ones based on current health, leaving the empties if we aren't at max health
		for (int i = 0; i < playerCurrentHealth; i++) {
			healthSlots[i] = healthContainerFull;
		}
		//for all of our slots, both empty and full, determine which one we should spawn into our health bar
		for (int i = 0; i < healthSlots.Count; i++) {
			//if the current slot has a full container, spawn one
			if (healthSlots[i] == healthContainerFull) {
				//Instantiating and assigning to a game object lets us still do stuff to it after it spawns
				//since now we have a reference to it
				GameObject hcf = Instantiate(healthContainerFull, transform.position, Quaternion.identity);
				//Set it's parent to our health bar so our horizontal layout group can take effect
				hcf.transform.SetParent(healthBar, false);
				//add it to our reference list so we can destroy it later
				spawnedSlots.Add(hcf);
			}
			//if the current slot has an empty container, spawn one
			if (healthSlots[i] == healthContainerEmpty) {
				//Instantiating and assigning to a game object lets us still do stuff to it after it spawns
				//since now we have a reference to it
				GameObject hce = Instantiate(healthContainerEmpty, transform.position, Quaternion.identity);
				//Set it's parent to our health bar so our horizontal layout group can take effect
				hce.transform.SetParent(healthBar, false);
				//add it to our reference list so we can destroy it later
				spawnedSlots.Add(hce);
			}
		}
	}

	//This can be called from anything from an enemy to a hazard, so it's modular
	public void TakeDamage(int amount, GameObject enemyResponsible) {
		//Stop the player from dashing or whatever they might be doing
		//GetComponent<PlayerMovement>().StopAllCoroutines();
		//Tell the player movement to knock us back, passing in the enemy responsible so we can calculate which direction
		//we were hit from and which way we should knock back
		//StartCoroutine(GetComponent<PlayerMovement>().Knockback(enemyResponsible));
		//remove player health
		playerCurrentHealth -= amount;
		//if we drop below zero, dead
		if (playerCurrentHealth <= 0) {
			//Game Over
			//make sure to cap so that our health bar doesn't go crazy with a negative number
			playerCurrentHealth = 0;
		}
		//update our health bar visuals
		CalculateHealthBar();
	}

	public void Die() {
		//hide arm
		// armFunction.gameObject.SetActive(false);	
	
		//playing death screen animation
		Animator deathAnim = death.GetComponent<Animator>();

		// play character death animation
		playerAnimator.SetBool("isDead", true);
			
		//Set movement speed to 0
		// player.velocity.x = 0;

		// saying if the player can die. can't die if already dead
		canDie = false;

		// disabling player function after death
		gameObject.GetComponent<PlayerInput>().enabled = false;

		//transition screens	
		//playing death screen animation
		deathAnim.SetBool("dead", true);

		// playerGO.SetActive(false);
	}
	

	//Can be called from anywhere from health pickups to save stations, etc.
	public void Heal(int amount) {
		//add to player health
		playerCurrentHealth += amount;
		//if we over heal, cap the value to our total
		if(playerCurrentHealth > playerTotalHealth) {
			playerCurrentHealth = playerTotalHealth;
		}
		//update health visuals
		CalculateHealthBar();
	}

	//if we want to add another health tank to our total, do so and update the visuals
	public void AddMaxHealth() {
		playerTotalHealth++;
		CalculateHealthBar();
	}

}
