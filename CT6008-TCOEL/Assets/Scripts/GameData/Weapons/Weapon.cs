using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public float fWeaponFireRate;
    [HideInInspector] public float fOverHeatRate;
    [HideInInspector] public float fBulletsFired;
    [HideInInspector] public float fDamage;
    [HideInInspector] public float fLastShot = 0f;
    [HideInInspector] public bool bRecharging;
    [HideInInspector] public bool bCoolDown;
    public bool bCanShoot;

    public Transform firePoint;
    public GameObject go_Bullet;
    public GameObject go_Buckshot;

    public WeaponManager weaponManager;
    public GameObject go_WeaponManger;

    public BlackBoard blackBoard;

    public AudioSource ShootSound;

	private void Start() {

    }

	void Update()
    {
        
    }

    public void InstantiateBullet() {
        Instantiate(go_Bullet, firePoint.position, firePoint.rotation);
    }

    public void InstantiateBuckshot() {
         GameObject buckShot = Instantiate(go_Buckshot, firePoint.position, firePoint.rotation);
    }
}
