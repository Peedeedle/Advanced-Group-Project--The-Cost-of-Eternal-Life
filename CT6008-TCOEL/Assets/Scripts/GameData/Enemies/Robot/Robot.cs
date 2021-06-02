using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy
{
    void Start()
    {
        fEnemyHealth = fEnemyMaxHealth;
        fLastHealth = fEnemyHealth;
        fPatrolRaycastDistance = 1f;
        fPatrolSpeedOfEnemy = 2f;
        bPatrolRight = true;
        fLoSRaycastDistance = 5f;
        tPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (fEnemyHealth < fLastHealth) {
            TakingDamageSound.Play();
            fLastHealth = fEnemyHealth;
        } else {
            fLastHealth = fEnemyHealth;
        }

        if (fEnemyHealth > 0) {
            RaycastHit2D blockedInfoRight = Physics2D.Raycast(transform.position, Vector2.right, 5.1f, blockLayerMask);
            RaycastHit2D blockedInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, 5.1f, blockLayerMask);
            if (LineOfSightCheck() == true && tPlayer.GetComponent<PlayerController>().bBossFightCameraActive == false || fEnemyHealth < fEnemyMaxHealth) {
                animator.SetBool("AttackPlayer", true);
                //Debug.Log("Player Sighted");
                if(AttackSound.isPlaying == false) {
                    AttackSound.Play();
				}
                Attack();
            } else {
                animator.SetBool("AttackPlayer", false);
                AttackSound.Stop();
                Patrol();
                //Debug.Log("Patrol");
            }
        } else {
            Vector3 v3DeathPosition = transform.position;
            DropFuel(v3DeathPosition);
            Instantiate(DeathXplosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
