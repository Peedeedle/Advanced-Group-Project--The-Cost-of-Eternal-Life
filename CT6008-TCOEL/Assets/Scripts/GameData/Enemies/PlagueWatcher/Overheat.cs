using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheat : MonoBehaviour
{
	Vector3 v3_LocalScale;
	public GameObject go_Player;
	public GameObject go_CurrentWeapon;
	float fLocalXScale;

	Vector3 v3_BackBarLocalScale;
	public GameObject go_BackBar;


	private void Start() {

		v3_LocalScale = transform.localScale;
		fLocalXScale = transform.localScale.x;
		
		v3_BackBarLocalScale = go_BackBar.transform.localScale;

	}

	private void Update() {
		if (go_CurrentWeapon == null) {
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			go_BackBar.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		} else {
			this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
			go_BackBar.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		}

		go_CurrentWeapon = go_Player.GetComponent<PlayerController>().go_CurrentWeapon;
		//v3_LocalScale.x = go_Enemy.GetComponent<Enemy>().fEnemyHealth / go_Enemy.GetComponent<Enemy>().fEnemyMaxHealth;
		float fBarSize = fLocalXScale / go_CurrentWeapon.GetComponent<Weapon>().fOverHeatRate;
		v3_LocalScale.x = go_CurrentWeapon.GetComponent<Weapon>().fBulletsFired * fBarSize;
		transform.localScale = v3_LocalScale;

		v3_BackBarLocalScale.x = 0.4f;

	}
}
