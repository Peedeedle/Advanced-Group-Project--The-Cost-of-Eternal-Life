using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    #region Stamina Upgrade Variables
    private float fStaminaUpgradeMultiplier = 1.07f;
    private float fStaminaCostMultiplier = 4f;
    private float fNewStaminaValue;
    private float fStaminaUpgradeCost;

    public Text StaminaUpgradeText;
    public Text StaminaUpgradeCostText;
    public Slider staminaBarSlider;
    #endregion

    #region SMG Upgrade Variables
    private float fSMGDamageUpgradeMultiplier = 1.11f;
    private float fSMGDamageCostMultiplier = 7.28f;
    private float fNewSMGDamageValue;
    private float fSMGDamageUpgradeCost;

    public Text SMGDamageUpgradeText;
    public Text SMGDamageUpgradeCostText;

    private float fSMGOverheatUpgradeMultiplier = 1.37f;
    private float fSMGOverheatCostMultiplier = 9.45f;
    private float fNewSMGOverheatValue;
    private float fSMGOverheatUpgradeCost;

    public Text SMGOverheatUpgradeText;
    public Text SMGOverheatUpgradeCostText;

    public SMG smg;
    #endregion

    public BlackBoard blackBoard;

    public PlayerController playerController;

    void Start()
    {
        smg = GameObject.Find("MP5").GetComponent<SMG>();
    }

    // Update is called once per frame
    void Update()
    {
        UpgradeStaminaUI();
        UpgradeSMGUI();
    }

    public void UpgradeSMGUI() {
        fSMGDamageUpgradeCost = Mathf.RoundToInt(smg.fDamage * fSMGDamageCostMultiplier);

        fNewSMGDamageValue = Mathf.RoundToInt(smg.fDamage * fSMGDamageUpgradeMultiplier);

        SMGDamageUpgradeText.text = "Damage: " + Mathf.RoundToInt(smg.fDamage).ToString() + " > " + fNewSMGDamageValue.ToString();
        SMGDamageUpgradeCostText.text = fSMGDamageUpgradeCost.ToString();

        fSMGOverheatUpgradeCost = Mathf.RoundToInt(smg.fOverHeatRate * fSMGOverheatCostMultiplier);

        fNewSMGOverheatValue = Mathf.RoundToInt(smg.fOverHeatRate * fSMGOverheatUpgradeMultiplier);

        SMGOverheatUpgradeText.text = "Overheat: " + Mathf.RoundToInt(smg.fOverHeatRate).ToString() + " > " + fNewSMGOverheatValue.ToString();
        SMGOverheatUpgradeCostText.text = fSMGOverheatUpgradeCost.ToString();
    }

    public void UpgradeSMGDamage() {
        if (blackBoard.iFuelCount > fSMGDamageUpgradeCost) {
            smg.fDamage = Mathf.RoundToInt((smg.fDamage * fSMGDamageUpgradeMultiplier));
            blackBoard.iFuelCount = blackBoard.iFuelCount - Mathf.RoundToInt(fSMGDamageUpgradeCost);
        }
    }

    public void UpgradeSMGOverheat() {
        if (blackBoard.iFuelCount > fSMGOverheatUpgradeCost) {
            smg.fOverHeatRate = Mathf.RoundToInt((smg.fOverHeatRate * fSMGOverheatUpgradeMultiplier));
            blackBoard.iFuelCount = blackBoard.iFuelCount - Mathf.RoundToInt(fSMGOverheatUpgradeCost);
        }
    }

    public void UpgradeStaminaUI() {
        fStaminaUpgradeCost = Mathf.RoundToInt(blackBoard.fMaxStamina * fStaminaCostMultiplier);

        fNewStaminaValue = Mathf.RoundToInt(blackBoard.fMaxStamina * fStaminaUpgradeMultiplier);

        StaminaUpgradeText.text = Mathf.RoundToInt(blackBoard.fMaxStamina).ToString() + " > " + fNewStaminaValue.ToString();
        StaminaUpgradeCostText.text = fStaminaUpgradeCost.ToString();
    }

    public void UpgradeStamina() {
        if(blackBoard.iFuelCount > fStaminaUpgradeCost) {
            blackBoard.fMaxStamina = Mathf.RoundToInt((blackBoard.fMaxStamina * fStaminaUpgradeMultiplier));
            blackBoard.fCurrentStamina = Mathf.RoundToInt(blackBoard.fMaxStamina);
            blackBoard.iFuelCount = blackBoard.iFuelCount - Mathf.RoundToInt(fStaminaUpgradeCost);

            blackBoard.fCurrentStamina = blackBoard.fMaxStamina;
            staminaBarSlider.maxValue = blackBoard.fMaxStamina;
            staminaBarSlider.value = blackBoard.fMaxStamina;

            playerController.fMaxStamina = blackBoard.fMaxStamina;
        }
	}
}
