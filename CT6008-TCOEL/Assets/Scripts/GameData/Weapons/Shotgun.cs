using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Shotgun : Weapon
{
    private void Awake() {
        //if (File.Exists(Application.persistentDataPath + "/player.sdat")) {
        //    LoadShotgun();
        //} else {
        //    LoadNewShotgun();
        //}

        LoadNewShotgun();
    }

    void Start() {
        fWeaponFireRate = 1.8f;
        fBulletsFired = fOverHeatRate;
        bRecharging = false;
    }


    void Update() {

        if (fBulletsFired > fOverHeatRate) {
            fBulletsFired = fOverHeatRate;
        }

        if (Input.GetKeyDown(KeyCode.F) && weaponManager.go_CurrentWeapon.name == this.name && blackBoard.fPlayerHealth > 0 && fBulletsFired > 0 && bCanShoot == true) {
            fBulletsFired--;
            StartCoroutine("FireRate");
            ShootSound.Play();
            fLastShot = Time.time;
        } else if (!Input.GetKey(KeyCode.F) && fBulletsFired <= fOverHeatRate && bRecharging == false && bCoolDown == false) {
            Debug.Log("Recharging");
            StartCoroutine("Recharge");
        } else if (fBulletsFired <= 0 && bCoolDown == true && bRecharging == false) {
            StartCoroutine("CoolDown");
        }
    }

    IEnumerator FireRate() {
        bCanShoot = false;
        InstantiateBuckshot();
        yield return new WaitForSeconds(1.5f);
        bCanShoot = true;
	}

    IEnumerator Recharge() {
        bRecharging = true;
        yield return new WaitForSeconds(5f);
        fBulletsFired = fBulletsFired + 1;
        bRecharging = false;
    }

    IEnumerator CoolDown() {
        bCoolDown = true;
        yield return new WaitForSeconds(4.8f);
        fBulletsFired = fOverHeatRate;
        bCoolDown = false;

    }

    public void SaveSMG() {
        SaveSystem.SaveShotgun(this);
    }

    void LoadShotgun() {
        ShotgunSaveData data = SaveSystem.LoadShotgun();
        fDamage = data.fShotgunDamage;
        fOverHeatRate = data.fShotgunOverheat;
    }

    void LoadNewShotgun() {
        fDamage = 37.0f;
        fOverHeatRate = 4.0f;
    }
}
