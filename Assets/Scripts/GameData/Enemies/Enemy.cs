////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// File:                 <Enemy.cs>
// Author:               <Morgan Ellis>
// Date Created:         <25/03/2021>
// Brief:                <This is the base class for the enemies in the game, each enemy will inherit from this class>
// Last Edited By:       <Morgan Ellis
// Last Edited Date:     <17/05/2021>
// Last Edit Brief:      <Updating Comments on the code>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public Animator animator;

	#region // Audio
	public AudioSource AttackSound;
	public AudioSource TakingDamageSound;
	#endregion

	#region // Layer Masks
	public LayerMask platformLayerMask;
	public LayerMask playerLayerMask;
	public LayerMask blockLayerMask;
	#endregion

	#region // Patrol Variables
	[System.NonSerialized] public float fEnemyHealth;
	[System.NonSerialized] public float fLastHealth;
	public float fEnemyMaxHealth;
	[System.NonSerialized] public float fPatrolRaycastDistance;
	[System.NonSerialized] public float fPatrolSpeedOfEnemy;
	[System.NonSerialized] public bool bPatrolRight;
	public Transform tGroundDetection; // transform on the gameobject that will be used for ground detection
	#endregion

	#region // Line of sight variables
	[System.NonSerialized] public float fLoSRaycastDistance;
	[System.NonSerialized] public bool bPlayerSpotted;
	#endregion

	#region // Transforms
	public Transform tPlayer; // transform of our player
	public Transform EnemyTransform; // transform of this enemy
	public Transform RespawnPoint; // transform of where the enemy will respawn
	#endregion

	public int iFuelDropAmmount;

	public GameObject goFuelDrop; // prefab of the fuel drop
	 
	public ParticleSystem DeathXplosionEffect; // particle effect that will play upon death

	/// <summary>
	/// If this class is not overridden than the enemy will patrol left/right or right/left while it can detect the ground beneth it
	/// </summary>
	public void Patrol() {
		RaycastHit2D groundInfo = Physics2D.Raycast(tGroundDetection.position, Vector2.down, fPatrolRaycastDistance, platformLayerMask); // perform a raycast down and see if it collides with our platform layermask

		transform.Translate(Vector2.left * fPatrolSpeedOfEnemy * Time.deltaTime); // This moves the enemy left along the carriage at a predefined speed of fPatrolSpeedOfEnemy

		if (groundInfo.collider == false) { // if there is no collision then we are at the end of the platform
			if (bPatrolRight == true) { // if the enemy is facing right
				transform.eulerAngles = new Vector3(0, -180, 0); // flip the enemy -180 so it is now facing right
				bPatrolRight = false;
			} else {
				transform.eulerAngles = new Vector3(0, 0, 0);
				bPatrolRight = true;
			}
		}
	}

	/// <summary>
	/// This function perfroms raycasts left and right to detect if the player is within attacking range
	/// </summary>
	/// <returns>True if the raycast colliders with the player mask</returns>
	public bool LineOfSightCheck() {
		RaycastHit2D blockedInfoRight = Physics2D.Raycast(transform.position, Vector2.right, 5.1f, blockLayerMask);
		RaycastHit2D blockedInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, 5.1f, blockLayerMask);

		if (blockedInfoRight.collider == null && blockedInfoLeft.collider == null) {
			if (transform.rotation.y == 0) { // if the enemies rotation is 0 than it is facing left
				RaycastHit2D rayCastInfo = Physics2D.Raycast(transform.position, Vector2.left, fLoSRaycastDistance, playerLayerMask); // perform a raycast to the left with a certain distance
				RaycastHit2D rayCastInfoRight = Physics2D.Raycast(transform.position, Vector2.right, fLoSRaycastDistance / 2, playerLayerMask); // perfrom a raycast to the right with half the distance to check th eplayer is too close behind
				if (rayCastInfo.collider == true) { // if the first collider hits the player than the player has been spotted
					bPlayerSpotted = true; // set the bool to true
				} else {
					bPlayerSpotted = false;
				}

				if (rayCastInfoRight.collider == true) { // if the second raycast hits the player 
					transform.eulerAngles = new Vector3(0, -180, 0); // the enemies is flipped -180 to face the player
					bPlayerSpotted = true; // set the bool to true
				}

			} else {
				RaycastHit2D rayCastInfo = Physics2D.Raycast(transform.position, Vector2.right, fLoSRaycastDistance, playerLayerMask);
				RaycastHit2D rayCastInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, fLoSRaycastDistance / 2, playerLayerMask);

				if (rayCastInfo.collider == true) {
					bPlayerSpotted = true;
				} else {
					bPlayerSpotted = false;
				}

				if (rayCastInfoLeft.collider == true) {
					transform.eulerAngles = new Vector3(0, 0, 0);
					bPlayerSpotted = true;
				}
			}
		}
		return bPlayerSpotted; // return the value of bPlayerSpotted
	}

	/// <summary>
	/// If the LineOfSightCheck() returned true than this function is called
	/// </summary>
	public void Attack() {

		float fEnemyChaseSpeed = 4.2f; // how quickly the enemy can move towards the player
		Vector2 v2PlayerPosition = new Vector2(tPlayer.position.x, tPlayer.position.y); // gets the players current position
		Vector2 v2EnemyPosition = new Vector2(transform.position.x, transform.position.y); // gets the enemies current position

		transform.position = Vector2.MoveTowards(v2EnemyPosition, v2PlayerPosition, fEnemyChaseSpeed * Time.deltaTime); // move the enemy towards the player at a certain rate based on the chase speed
	}


	/// <summary>
	/// Drop 5 fuel drops that vary in value based on the value in the inspector
	/// </summary>
	/// <param name="v3FuelDropPosition"></param>
	public void DropFuel(Vector3 v3FuelDropPosition) {
		for(int i = 0; i < 5; i++) {
			goFuelDrop.GetComponent<Fuel>().iFuelDropAmmount = iFuelDropAmmount;
			Instantiate(goFuelDrop, v3FuelDropPosition, Quaternion.identity);
		}
	}
}
