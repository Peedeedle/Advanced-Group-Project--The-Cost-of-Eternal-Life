using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueWatcher : MonoBehaviour
{
    public PlayerController player;
    /// <summary>
    /// Laser Variables
    /// </summary>
    private float fDistanceRay = 25f;
    public Transform laserFirePoint;
    public LineRenderer LaserLineRenderer;
    public Transform endLaserTransform;
    public Vector3 v3_StartPosition;
    public LayerMask playerLayerMask;
    private bool bLaserReady;
    private bool bPlayLaserAttack;
    public BlackBoard blackboard;
    private float fTime;
    private float fAccelerationTime;
    private float fMinSpeed;
    private float fMaxSpeed;
    private float fCurrentSpeed;
    private int i = 0;


    /// <summary>
    /// Ground attack variables
    /// </summary> 
    public GameObject[] go_aGroundAttackColliders;
    private bool bGroundAttack;

    private bool bChooseAnAttack;
    public bool bAttacked;
    private int iChooseAttack;

    public GameObject CloseDoor;

    public float fEnemyHealth;
    public float fEnemyMaxHealth;

    public GameObject[] go_aHealthBars;

    public GameObject go_FuelDrop;

    public ParticleSystem DeathXplosionEffect;

    public AudioSource PlagueWatcherSoundTrack;

    public AudioSource LaserSound;

    public AudioSource GroundSlam;

    public AudioSource backGroundMusic;

    public Animator plagueWatcherAnimator;


    /// <summary>
    /// UI Elements
    /// </summary> 
    public GameObject go_GameOverScreen;


    void Start()
    {
        bLaserReady = false;
        v3_StartPosition = endLaserTransform.position;
        LaserLineRenderer.enabled = false;
        fMinSpeed = fCurrentSpeed;

        fEnemyHealth = 4000;
        fEnemyMaxHealth = fEnemyHealth;

        bAttacked = false;
        bGroundAttack = false;
        bChooseAnAttack = true;
        fCurrentSpeed = 0.05f;
        fMinSpeed = fCurrentSpeed;
        fMaxSpeed = 2f;
        fAccelerationTime = 60f;
        fTime = 0;

        // Set the game over screen to false
        go_GameOverScreen.SetActive(false);
    }

    void Update()
    {
        if(fEnemyHealth < fEnemyMaxHealth) {
            bAttacked = true;
            if(bAttacked == true) {
                player.bBossFightCameraActive = true;
            }
		}

        if (bChooseAnAttack == true) {
            iChooseAttack = Random.Range(0, 2);
            Debug.Log(iChooseAttack);
            for (int i = 0; i < go_aGroundAttackColliders.Length; i++) {
                BoxCollider2D boxCollider2D = go_aGroundAttackColliders[i].GetComponent<BoxCollider2D>();
                boxCollider2D.enabled = false;
            }
        }

        if(bGroundAttack == false) {

		}

        if (player.bBossFightCameraActive == true) {
            backGroundMusic.Pause();
            if(PlagueWatcherSoundTrack.isPlaying == false) {
                PlagueWatcherSoundTrack.Play();
            }

            //PlagueWatcherSoundTrack.volume = Mathf.Clamp(0.05f, 0.01f, 0.065f);
            if (fEnemyHealth > 0) {
                for (int i = 0; i < go_aHealthBars.Length; i++) {
                    go_aHealthBars[i].SetActive(true);
                }
                CloseDoor.SetActive(true);
                if (iChooseAttack == 0 && bChooseAnAttack == true) {
                    bGroundAttack = true;
                    bLaserReady = false;
                    bChooseAnAttack = false;
                } else if (iChooseAttack == 1 && bChooseAnAttack == true) {
                    bGroundAttack = false;
                    bLaserReady = true;
                    bChooseAnAttack = false;
                }
                if (bGroundAttack == true) {
                    GroundAttack();
                    bGroundAttack = false;
                }
                if (bLaserReady == true) {
                    if (endLaserTransform.position != new Vector3(233.0f, -0.75f, 0) && bLaserReady == true) {
                        ShootLaser();
                    } else {
                        endLaserTransform.position = v3_StartPosition;
                        i++;
                    }
                }
            } else {
                DropFuel(transform.position);
                Instantiate(DeathXplosionEffect, transform.position, transform.rotation);
                CloseDoor.SetActive(false);
                backGroundMusic.Play();
                Destroy(gameObject);

                // Make Game over screen visible
                go_GameOverScreen.SetActive(true);

            }
        }
    }

    private void ShootLaser() {
        LaserLineRenderer.enabled = true;
        if(LaserSound.isPlaying == false) {
            LaserSound.Play();
        }

        if (i < 3) {

            if (Physics2D.Raycast(endLaserTransform.position, Vector2.left, 0.1f, playerLayerMask) || Physics2D.Raycast(endLaserTransform.position, (Vector2.up + Vector2.right + Vector2.right + Vector2.right), 10f, playerLayerMask)) {
                StartCoroutine("WaitForLaserRecharge");
            }
            fCurrentSpeed = Mathf.SmoothStep(fMinSpeed, fMaxSpeed, fTime / fAccelerationTime);
            endLaserTransform.position = Vector3.MoveTowards(endLaserTransform.position, new Vector3(233.0f, -0.75f, 0), fCurrentSpeed);
            fTime += Time.deltaTime;

            //endLaserTransform.position = Vector3.Lerp(v3_StartPosition, new Vector3(183, -0.75f, -107), 1.25f * Time.deltaTime);
            Draw2DRay(laserFirePoint.position, endLaserTransform.position);
		} else {
            StartCoroutine("WaitForLaserRecharge2");
        }

    }

    IEnumerator WaitForLaserRecharge2() {
        fCurrentSpeed = 0.05f;
        fMinSpeed = fCurrentSpeed;
        fMaxSpeed = 2f;
        fAccelerationTime = 10f;
        fTime = 0;
        bLaserReady = false;
        LaserLineRenderer.enabled = false;
        endLaserTransform.position = v3_StartPosition;
        yield return new WaitForSeconds(7f);
        i = 0;
        bChooseAnAttack = true;

    }

    IEnumerator WaitForLaserRecharge() {
        fCurrentSpeed = 0.05f;
        fMinSpeed = fCurrentSpeed;
        fMaxSpeed = 2f;
        fAccelerationTime = 60f;
        fTime = 0;
        bLaserReady = false;
        LaserLineRenderer.enabled = false;
        blackboard.fPlayerHealth -= 1;
        endLaserTransform.position = v3_StartPosition;
        yield return new WaitForSeconds(9f);
        i = 0;
        bChooseAnAttack = true;
    }

    private void Draw2DRay(Vector2 startPos, Vector2 endPos) {
        LaserLineRenderer.SetPosition(0, startPos);
        LaserLineRenderer.SetPosition(1, endPos);
	}

    private void GroundAttack() {
        for (int i = 0; i < go_aGroundAttackColliders.Length; i++) {
            BoxCollider2D boxCollider2D = go_aGroundAttackColliders[i].GetComponent<BoxCollider2D>();
            boxCollider2D.enabled = false;
        }
        plagueWatcherAnimator.SetBool("bSlamAttack", true);
        GroundSlam.Play();
        for (int i = 0; i < 7; i++) {

            int index = Random.Range(0, go_aGroundAttackColliders.Length);
            ParticleSystem warningSystem = go_aGroundAttackColliders[index].transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            warningSystem.Play();

            //go_aGroundAttackColliders[index].GetComponent<BoxCollider2D>().enabled = true;
            ParticleSystem attackSystem = go_aGroundAttackColliders[index].transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

            BoxCollider2D boxCollider2D = go_aGroundAttackColliders[index].GetComponent<BoxCollider2D>();
            boxCollider2D.enabled = true;

            attackSystem.Play();
        }

        StartCoroutine("GroundAttackRecharge");

    }

    IEnumerator GroundAttackRecharge() {
        yield return new WaitForSeconds(6f);
        bGroundAttack = false;
        bChooseAnAttack = true;
        plagueWatcherAnimator.SetBool("bSlamAttack", false);
    }

    public void DropFuel(Vector3 v3FuelDropPosition) {
        for (int i = 0; i < 7; i++) {
            go_FuelDrop.GetComponent<Fuel>().iFuelDropAmmount = 500;
            Instantiate(go_FuelDrop, v3FuelDropPosition, Quaternion.identity);
        }
    }

}
