using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Integer values for the health and number of hearts
    //public int iHealth;
    public int iNumOfHearts;

    public Image[] hearts;
    public Sprite sFullHeart;
    public Sprite sBrokenHeart;

    public BlackBoard blackBoard;

    // Public slider / stamina bar
    public Slider staminaBar;

    // Wait for seconds (regenTick)
    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);

    // regen cororutine
    private Coroutine regen;

    public Text fuelText;

    public Text healCount;

    void Start() {
        // Current stamina = max stamina
        //blackBoard.iCurrentStamina = blackBoard.iMaxStamina;

        // stamina bars max value = max stamina
        staminaBar.maxValue = blackBoard.fMaxStamina;

        // stamina value = max stamina
        staminaBar.value = blackBoard.fMaxStamina;

        healCount.text = blackBoard.iHealCount.ToString();
    }

    void Update() {


        healCount.text = blackBoard.iHealCount.ToString();
        // If health is more than the number of hearts then set health to number of hearts
        if (blackBoard.fPlayerHealth > blackBoard.fPlayerMaxHealth) {
            blackBoard.fPlayerHealth = blackBoard.fPlayerMaxHealth;
        }

        // If i is less than hearts in heart array
        for (int i = 0; i < hearts.Length; i++) {

            // if i is less than health, set heart to full heart sprite
            if (i < blackBoard.fPlayerHealth) {
                hearts[i].sprite = sFullHeart;
            } else { // if i is more than health, set heart to broken heart sprite
                hearts[i].sprite = sBrokenHeart;
            }

            // Check if i is smaller, if true set hearts to true
            if (i < blackBoard.fPlayerMaxHealth) {
                hearts[i].enabled = true;
            } else { // Check if i is bigger, hide hearts
                hearts[i].enabled = false;
            }
        }

        fuelText.text = blackBoard.iFuelCount.ToString();
    }

    public void UseStamina(int iAmount) {

        // If current stamina - amount is more than or = 0, we have stamina
        if (blackBoard.fCurrentStamina - iAmount >= 0) {

            // current stamina - amount
            blackBoard.fCurrentStamina -= iAmount;

            // stamina bar value = current stamina
            staminaBar.value = blackBoard.fCurrentStamina;

            // If already regenerating stamina
            if (regen != null) {

                // Stop coroutine
                StopCoroutine(regen);
            }

            // start regen stamina Coroutine
            regen = StartCoroutine(RegenStamina());

        } else {

            // Not enough stamina
            Debug.Log("Not enough stamina");
        }

    }

    // Regen stamina ienumerator
    private IEnumerator RegenStamina() {

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // While current stamina is less than max stamina
        while (blackBoard.fCurrentStamina < blackBoard.fMaxStamina) {

            // current stamina + max stamina divided by 100
            blackBoard.fCurrentStamina += blackBoard.fMaxStamina / 100;

            // stamina bar value = current stamina
            staminaBar.value = blackBoard.fCurrentStamina;

            Debug.Log(staminaBar.value);

            // regen tick (wait for 0.1 second)
            yield return regenTick;
        }
        // Regen = null
        regen = null;
    }

}
