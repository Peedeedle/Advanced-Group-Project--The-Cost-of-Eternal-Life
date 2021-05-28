using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SMG : Weapon
{
	private void Awake() {
        //if (File.Exists(Application.persistentDataPath + "/player.sdat")) {
        //          LoadSMG();
        //} else {
        //	LoadNewSMG();
        //}

        LoadNewSMG();
	}

    void Start() {
        fBulletsFired = fOverHeatRate;
        bRecharging = false;
    }

    void Update() {
        
        if(fBulletsFired > fOverHeatRate) {
            fBulletsFired = fOverHeatRate;
		}


        if (Input.GetKey(KeyCode.F) && weaponManager.go_CurrentWeapon.name == this.name && blackBoard.fPlayerHealth > 0 && fBulletsFired > 0 && bCanShoot == true) {
            fBulletsFired--;
            StartCoroutine("FireRate");
            ShootSound.Play();
            fLastShot = Time.time;
        } else if(!Input.GetKey(KeyCode.F) && fBulletsFired <= fOverHeatRate && bRecharging == false) {
            Debug.Log("Recharging");
            StartCoroutine("Recharge");
        } else if (fBulletsFired <= 0) {
             StartCoroutine("CoolDown");
        }
    }

    IEnumerator FireRate() {
        bCanShoot = false;
        InstantiateBullet();
        yield return new WaitForSeconds(0.14f);
        bCanShoot = true;
    }

    IEnumerator Recharge() {
        yield return new WaitForSeconds(0.6f);
        fBulletsFired = fBulletsFired + 1;
    }

    IEnumerator CoolDown() {
        bRecharging = true;
        yield return new WaitForSeconds(8f);
        fBulletsFired = fOverHeatRate;
        bRecharging = false;

    }

    public void SaveSMG() {
        SaveSystem.SaveSMG(this);
	}

    void LoadSMG() {
        SMGData data = SaveSystem.LoadSMG();
        fDamage = data.fSMGDamage;
        fOverHeatRate = data.fSMGOverheat;
	}

    void LoadNewSMG() {
        fDamage = 17.0f;
        fOverHeatRate = 20.0f;
	}
}
