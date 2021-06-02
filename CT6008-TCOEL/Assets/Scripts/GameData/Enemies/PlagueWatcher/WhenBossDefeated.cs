////////////////////////////////////////////////////////////
// File:                 <WhenBossDefeated.cs>
// Author:               <Jack Peedle>
// Date Created:         <29/05/2021>
// Brief:                <File responsible for stopping the timer and showing the player what time they got when the game is over>
// Last Edited By:       <Jack Peedle>
// Last Edited Date:     <29/05/2021>
// Last Edit Brief:      <Commented on code>
////////////////////////////////////////////////////////////

using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class WhenBossDefeated : MonoBehaviour {

    // Text for the timer on the end screen
    public Text EndTimerText;

    //public float for the start time
    public float FlstartTime;

    // gameobject for the canvas which the text is displayed
    public GameObject BossDefeatedCanvas;

    // Bool for when the game is over
    private bool BgameIsOver = false;
    
    // Update is called once per frame
    public void Update() {

        // If game is over, return
        if (BgameIsOver) {
            return;
        }

        // Float t = time - the start time
        float t = Time.time - FlstartTime;

        // String for minutes (int t divided by 60 to string)
        string minutes = ((int)t / 60).ToString();

        // String for seconds (int t module of 60 to string)
        string seconds = ((int)t % 60).ToString();

        // Set the end timer text to say "" and have the minutes and seconds
        EndTimerText.text = minutes + " Minutes " + "and " + seconds + " Seconds";
        
        // If boss defeated canvas is true
        if (BossDefeatedCanvas) {

            // time scale set to 0.000000001
            Time.timeScale = 0.000000001f;

            // game over is true
            BgameIsOver = true;

            // end timer = yellow colour
            EndTimerText.color = Color.yellow;


        }

        // if the boss defeated canvas is not true
        if (!BossDefeatedCanvas) {

            // Do nothing

        }



    }
    
}
