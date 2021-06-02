////////////////////////////////////////////////////////////
// File:                 <SoundManager.cs>
// Author:               <Jack Peedle>
// Date Created:         <29/05/2021>
// Brief:                <File responsible for the sound of the snow>
// Last Edited By:       <Jack>
// Last Edited Date:     <29/05/2021>
// Last Edit Brief:      <Commenting Code>
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Public audio source called snowaudiosource
    public AudioSource SnowAudioSource;

    // Audio clip array for the snow sounds
    public AudioClip[] snowSounds;

    // Int for random snow sound
    private int randomSnowSound;

    // Start is called before the first frame update
    void Start()
    {

        // Set snow audio source to get audio source component
        SnowAudioSource = GetComponent<AudioSource>();
        
    }
    
    // play snow audio
    public void PlaySnowAudio() {

        //randomSnowSound = Random.Range(0, 3);
        if (SnowAudioSource.isPlaying) {

          // else if Snow audio is not playing
        } else if (!SnowAudioSource.isPlaying) {

            // Select a random number between 0,1
            randomSnowSound = Random.Range(0, 2);

            
            //select a random clip from both snow audio sources;
            SnowAudioSource.clip = snowSounds[Random.Range(0, snowSounds.Length)];

            // Play the audio source
            SnowAudioSource.PlayOneShot(snowSounds[randomSnowSound]);

            //Start the coroutine snow sound pause
            StartCoroutine(SnowSoundPause());

        }
        
        

    }

    // Coroutine for the snow sound pausing
    private IEnumerator SnowSoundPause() {

        // return a new wait for 5 seconds
        yield return new WaitForSeconds(5f);
    }

}
