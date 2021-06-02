using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // allows use to save into a file
public class SMGData
{
	public float fSMGDamage;
	public float fSMGOverheat;

	public SMGData(SMG smg) {
		fSMGDamage = smg.fDamage;
		fSMGOverheat = smg.fOverHeatRate;
	}
}
