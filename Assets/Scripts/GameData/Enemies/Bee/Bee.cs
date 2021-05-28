////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// File:                 <Bee.cs>
// Author:               <Morgan Ellis>
// Date Created:         <08/05/2021>
// Brief:                <The script for the bee enemy that is inherited from enemy>
// Last Edited By:       <Morgan Ellis
// Last Edited Date:     <17/05/2021>
// Last Edit Brief:      <Updating Comments on the code>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
	public float fBeePatrolHeight;

	public BlackBoard blackboard;

	public AudioSource BuzzingSound;

	private float fSpeedOfEnemy;

	private bool bAttackEnemy;


	void Start() {
		fLastHealth = fEnemyHealth;
		fEnemyHealth = 300;
        fEnemyMaxHealth = 300;
        fPatrolRaycastDistance = 4f;
        fPatrolSpeedOfEnemy = 2f;
        bPatrolRight = true;
        fLoSRaycastDistance = 2f;
        tPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		fSpeedOfEnemy = 4f;
		bAttackEnemy = true;
		blackboard = GameObject.FindGameObjectWithTag("BlackBoard").GetComponent<BlackBoard>();
	}

	void Update() {

		// if the enemy has taken damage than play the corresponding sound
		if (fEnemyHealth < fLastHealth) {
			TakingDamageSound.Play();
			fLastHealth = fEnemyHealth;
		} else {
			fLastHealth = fEnemyHealth;
		}


		if (fEnemyHealth > 0) { // if the enemies health is greater than zero this logic will be executed
			if (LineOfSightCheck() == true && tPlayer.GetComponent<PlayerController>().bBossFightCameraActive == false) { // if the line of sight returns true or the enemy is damaged
				if (AttackSound.isPlaying == false) { // check to see if the attack sound is playing because we dont want overlaping sounds
					AttackSound.Play();
				}
				Attack(); // the enemy attacks the player
			} else {
				Patrol(); // if the enemy is spotted than the enemy goes back to patrolling 
				AttackSound.Stop(); // stop playing the attack sound if it is playing
			}
		} else { // if the health is less than zero than the enemy is dead
			Vector3 v3DeathPosition = transform.position; // set the death position 
			DropFuel(v3DeathPosition); // drop the fuel on this position
			Instantiate(DeathXplosionEffect, transform.position, transform.rotation); // create a particle system that plays on awake
			Destroy(gameObject); // destroy this enemy
		}

		float fDistanceBetweenThisAndPlayer = Vector3.Distance(transform.position, tPlayer.transform.position);

		if(fDistanceBetweenThisAndPlayer < 12f) {
			if(BuzzingSound.isPlaying == false){
				BuzzingSound.Play();
			}
			BuzzingSound.volume = Mathf.Clamp((1 * fDistanceBetweenThisAndPlayer), 0.01f, 0.065f);
		} else {
			BuzzingSound.Stop();
		}
	}

	new public void Patrol() {
		if (transform.position.y < fBeePatrolHeight) {
			transform.Translate(Vector2.up * fSpeedOfEnemy * Time.deltaTime);
			transform.Translate(Vector2.left * fSpeedOfEnemy * Time.deltaTime);
		}

		GetComponent<Renderer>().material.color = Color.green;
		RaycastHit2D groundInfo = Physics2D.Raycast(tGroundDetection.position, Vector2.up, fPatrolRaycastDistance, platformLayerMask);
		Color rayColor;
		if (groundInfo.collider != null) {
			rayColor = Color.green;
		} else {
			rayColor = Color.red;
		}

		transform.Translate(Vector2.left * fPatrolSpeedOfEnemy * Time.deltaTime);

		Debug.DrawRay(tGroundDetection.position, Vector2.down, rayColor);

		if (groundInfo.collider == false) {
			if (bPatrolRight == true) {
				transform.eulerAngles = new Vector3(0, -180, 0);
				bPatrolRight = false;
			} else {
				transform.eulerAngles = new Vector3(0, 0, 0);
				bPatrolRight = true;
			}
		}
	}

	new public bool LineOfSightCheck() {
		if (bAttackEnemy == true) {
			RaycastHit2D rayCastInfo = Physics2D.Raycast(transform.position, Vector2.down, fLoSRaycastDistance, playerLayerMask);
			//RaycastHit2D rayCastInfoLeft = Physics2D.Raycast(new Vector2(transform.position.x -2f, transform.position.y), Vector2.down, fLoSRaycastDistance, playerLayerMask);
			//RaycastHit2D rayCastInfoRight = Physics2D.Raycast(new Vector2(transform.position.x + 2f, transform.position.y), Vector2.down, fLoSRaycastDistance, playerLayerMask);
			if (rayCastInfo.collider == true) {
				bPlayerSpotted = true;
			} else {
				bPlayerSpotted = false;
			}
		} else {
			bPlayerSpotted = false;
		}
		return bPlayerSpotted;
	}

	new public void Attack() {
		float fEnemyChaseSpeed = 10f;
		Vector2 v2PlayerPosition = new Vector2(tPlayer.position.x, tPlayer.position.y);
		Vector2 v2EnemyPosition = new Vector2(transform.position.x, transform.position.y);

		transform.position = Vector2.MoveTowards(v2EnemyPosition, v2PlayerPosition, fEnemyChaseSpeed * Time.deltaTime);

		float fDistanceBtwnEnemyAndPlayer = Vector2.Distance((new Vector2(transform.position.x, transform.position.y)), (new Vector2(tPlayer.position.x, tPlayer.position.y)));
		Debug.Log(fDistanceBtwnEnemyAndPlayer);

		if (fDistanceBtwnEnemyAndPlayer < 0.5f) {

			gameObject.GetComponent<PolygonCollider2D>().enabled = false;
			GetComponent<Renderer>().material.color = Color.red;
			StartCoroutine("AttackRecharge");
		}
	}

	public IEnumerator AttackRecharge() {
		blackboard.fPlayerHealth -= 1;
		bAttackEnemy = false;
		yield return new WaitForSeconds(1.0f);
		bAttackEnemy = true;
		gameObject.GetComponent<PolygonCollider2D>().enabled = true;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player" && bAttackEnemy == true) {
			Debug.Log("Player");
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			Debug.Log("NoPlayer");
		}
	}

	new public void DropFuel(Vector3 v3FuelDropPosition) {
		for (int i = 0; i < 5; i++) {
			goFuelDrop.GetComponent<Fuel>().iFuelDropAmmount = 70;
			Instantiate(goFuelDrop, v3FuelDropPosition, Quaternion.identity);
		}
	}
}
