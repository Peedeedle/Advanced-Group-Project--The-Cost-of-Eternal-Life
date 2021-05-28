using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueWatcherLights : MonoBehaviour
{
    public Animator[] LightsAnimator;

    public Camera mainCamera;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            mainCamera.GetComponent<CameraFollow>().bBossFightCamera = true;
            for(int i = 0; i < LightsAnimator.Length; i++) {
                LightsAnimator[i].SetTrigger("PlayerEnter");
            }
        }
    }

	private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            
        }
    }

	private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().bBossFightCameraActive = false;
            mainCamera.GetComponent<CameraFollow>().bBossFightCamera = false;
        }
    }
}
