////////////////////////////////////////////////////////////
// File:                 <CameraFollow.cs>
// Author:               <Morgan Ellis>
// Date Created:         <09/02/2021>
// Brief:                <File responsible for the movements of the player such as jumping>
// Last Edited By:       <Morgan Ellis>
// Last Edited Date:     <13/03/2021>
// Last Edit Brief:      <The camera now is smoother when following enemies including dashes>
////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public Camera playerCamera;
    public bool bBossFightCamera;
    private float fTargetOrtho = 6.8f;
    public float fSmoothSpeed = 2.0f;

    private void Start() {
        bBossFightCamera = false;
    }

	private void FixedUpdate() {
        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && bBossFightCamera == false) {
            if (target) {
                Vector3 point = playerCamera.WorldToViewportPoint(target.position);
                Vector3 delta = target.position - playerCamera.ViewportToWorldPoint(new Vector3(0.35f, 0.45f, point.z)); //(new Vector3(0.5, 0.5, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                playerCamera.orthographicSize = Mathf.MoveTowards(playerCamera.orthographicSize, 4.85f, fSmoothSpeed * Time.deltaTime);
            }
        }

        if (bBossFightCamera == true) {
            Vector3 point = playerCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - playerCamera.ViewportToWorldPoint(new Vector3(0.35f, 0.31f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            playerCamera.orthographicSize = Mathf.MoveTowards(playerCamera.orthographicSize, fTargetOrtho, fSmoothSpeed * Time.deltaTime);

        }
    }

	// Update is called once per frame
	void Update() {

    }
}

