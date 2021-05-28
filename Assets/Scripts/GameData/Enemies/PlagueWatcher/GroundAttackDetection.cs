using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackDetection : MonoBehaviour
{
	private BlackBoard blackBoard;

	private void Start() {
		blackBoard = GameObject.FindGameObjectWithTag("BlackBoard").GetComponent<BlackBoard>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			blackBoard.fPlayerHealth -= 1;
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			blackBoard.fPlayerHealth -= 1;
		}
	}
}
