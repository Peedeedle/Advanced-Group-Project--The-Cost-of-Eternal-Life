using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : MonoBehaviour
{
    public float fBulletSpeed;
    public Rigidbody2D bulletRigidBody2D;
    private GameObject go_PlayerController;
    PlayerController playerController;
    public Shotgun shotgun;

    private float fLiveTime = 1.3f;

    void Start() {
        shotgun = GameObject.Find("Shotgun").GetComponent<Shotgun>();
        go_PlayerController = GameObject.FindGameObjectWithTag("Player");
        playerController = go_PlayerController.GetComponent<PlayerController>();
        if (playerController.bFacingRight == true) {
            bulletRigidBody2D.velocity = Vector2.right * fBulletSpeed;
        } else {
            bulletRigidBody2D.velocity = Vector2.right * -fBulletSpeed;
        }
    }

    private void Update() {
        fLiveTime -= Time.deltaTime;
        if (fLiveTime <= 0f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
            collision.GetComponent<Enemy>().fEnemyHealth -= shotgun.fDamage;

            if (collision.GetComponent<Enemy>().bPatrolRight == false) {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, collision.transform.position.y);
            } else {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.5f, -collision.transform.position.y);
            }

        }
        if (collision.gameObject.tag == "Wall") {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Plague") {
            Destroy(gameObject);
            collision.GetComponent<PlagueWatcher>().fEnemyHealth -= shotgun.fDamage;

        }

        // If the bullet collides with a blocker then destroy the bullet
        if (collision.gameObject.tag == "EnemyBlocker") {
            Destroy(gameObject);
        }


    }
    
}
