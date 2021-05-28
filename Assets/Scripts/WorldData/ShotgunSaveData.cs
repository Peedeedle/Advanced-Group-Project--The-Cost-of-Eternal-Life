using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // allows use to save into a file
public class ShotgunSaveData {
	public float fShotgunDamage;
	public float fShotgunOverheat;

	public ShotgunSaveData(Shotgun shotgun) {
		fShotgunDamage = shotgun.fDamage;
		fShotgunOverheat = shotgun.fOverHeatRate;
	}
}