using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleRobotAttack : MonoBehaviour
{
    public BlackBoard blackboard;
    public Transform tPlayer;

	private void Start() {
        blackboard = GameObject.FindGameObjectWithTag("BlackBoard").GetComponent<BlackBoard>();
        tPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
    }

	private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            transform.GetComponent<EdgeCollider2D>().enabled = false;

            tPlayer.gameObject.GetComponent<Renderer>().material.color = Color.red;
            if(transform.parent.gameObject.GetComponent<Enemy>().bPatrolRight == false) {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(7f, -collision.transform.position.y);
            } else {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(7f, collision.transform.position.y);
            }

            Debug.Log("Attack");

            StartCoroutine("WaitForAttackRecharge");
            blackboard.fPlayerHealth -= 1;
            tPlayer.gameObject.GetComponent<PlayerController>().InjuredGaspSound.Play();
            transform.GetComponent<EdgeCollider2D>().gameObject.SetActive(true);
        }
    }

	private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {

        }
    }

    public IEnumerator WaitForAttackRecharge() {
        yield return new WaitForSeconds(1.5f);
        transform.GetComponent<EdgeCollider2D>().enabled = true;
    }
}
