using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	Vector3 v3_LocalScale;
	public GameObject go_Enemy;

	//Vector3 v3_SecondHealthBarLocalScale;
	//public GameObject SecondHealthBar;

	private void Start() {
		v3_LocalScale = transform.localScale;
	}

	private void Update() {
		v3_LocalScale.x = go_Enemy.GetComponent<Enemy>().fEnemyHealth / go_Enemy.GetComponent<Enemy>().fEnemyMaxHealth;
		transform.localScale = v3_LocalScale;

		//v3_SecondHealthBarLocalScale = transform.localScale;
		//v3_LocalScale.x = go_Enemy.GetComponent<Enemy>().fEnemyMaxHealth;
		//SecondHealthBar.transform.localScale = v3_SecondHealthBarLocalScale;
	}
}
