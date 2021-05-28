using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Public audio source called snowaudiosource
    public AudioSource SnowAudioSource;

    public AudioListener audioListener;

    //
    public AudioClip[] snowSounds;

    // Int for random snow sound
    private int randomSnowSound;

    // Start is called before the first frame update
    void Start()
    {

        // Set snow audio source to get audio source component
        SnowAudioSource = GetComponent<AudioSource>();

        

        //
        //snowSounds = Resources.LoadAll<AudioClip>("SnowSounds");

    }

    

    // On collision
    private void OnTriggerStay2D(Collision2D collision) {
        
        // If the collision is = to a gameobject with "Snow" tag
        if (collision.gameObject.tag == "Player" && (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D))) {

            PlaySnowAudio();
            Debug.Log("YESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");

        } //else if (collision.gameObject.tag == "Player" && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D))) {

        //    SnowAudioSource.Stop();
        //    Debug.Log("Not Walking");

        //}

        //if (collision.gameObject.tag == "Snow" && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D))){

        //    //
        //    SnowAudioSource.Pause;

        //}

    }

    public void PlaySnowAudio() {

        //randomSnowSound = Random.Range(0, 3);

        // Play the audio source
        //SnowAudioSource.PlayOneShot(snowSounds[randomSnowSound]);
        SnowAudioSource.clip = snowSounds[Random.Range(0, snowSounds.Length)];
        Debug.Log("Walking");

    }

}
