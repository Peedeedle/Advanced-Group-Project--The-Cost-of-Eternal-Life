////////////////////////////////////////////////////////////
// File:                 <PlayerController.cs>
// Author:               <Morgan Ellis>
// Date Created:         <30/01/2021>
// Brief:                <File responsible for the movements of the player such as jumping>
// Last Edited By:       <Morgan Ellis
// Last Edited Date:     <06/04/2021>
// Last Edit Brief:      <The character is now able to run and heal>
////////////////////////////////////////////////////////////

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask platformLayerMask; // layermask so the player can detect the ground

    [SerializeField] private LayerMask blockLayerMask; // layermask so the player can detect the ground

    private float fMoveSpeed = 6f; // defines how fast the players moves

    private Rigidbody2D playerRigidbody2D; // rigidbody of our player

    private float fDistToGround; // used to track how far off the ground the player is and determine if they are grounded

    CapsuleCollider2D capsuleCollider2D; // capsule collider for the player

    public BlackBoard blackboard; // reference to our blackboard which stores world data such as the players health so it can be accessed by other scripts

    public Camera mainCamera; // reference to the camera which will follow the player

    private float fCameraMaxLookHeight; // float used to control the max look hight of the players camera
    private float fCameraMinLookHeight; // float used to control the min look hight of the players camera

    private bool bDashIsReady; // bool that is used to allow the player to dash

    public bool bFacingRight; // bool that is used to know which way the player is facing

    private bool bCanPlayerMove; // allows the player to move, is false just after the player dashes and other instances

    public Animator playerAnimator; // reference to the players animator so we can control the animations

    public PlayerUI playerUI; // reference to the players UI script 

    public Canvas playerSaveCanvas; // reference to the players test save canvas

    public int iFuelAmmount; // a variable used to store the ammount of fuel the player has

    public float fPlayerMaxHealth; // a variable used to store the maximum health of the player

    public int iHealAmmount; // a varaible used to store the ammount of times the player can heal

    public float fMaxStamina; // a varaible used to store the highest ammount of stamina the player can have

    public GameObject go_WeaponManger;

    public WeaponManager weaponManager;

    public GameObject go_CurrentWeapon;

    public Inventory inventory;

    [SerializeField] private UI_Inventory uiInventory;

    [SerializeField] private GameObject go_UI_Inventory;

    public List<Items> itemList;

    public GameObject go_SMGPrefab;
    public GameObject go_ShotgunPrefab;

    public bool bResetWorld;

    public GameObject go_FuelPrefab;

    public GameObject FadeToBlackImage;

    public GameObject[] go_aOverHeatBars;

    public GameObject go_PauseCanvas;

    public GameObject go_GameOverCanvas;

    public AudioSource InjuredGaspSound;

    public AudioSource SnowCrunching;

    [SerializeField] public bool bBossFightCameraActive;

    private bool bIsCrouching;

    public SoundManager soundManager;

    private void Awake() {
        playerSaveCanvas.gameObject.SetActive(false); // disables the players save canvas


        if (File.Exists(Application.persistentDataPath + "/player.sdat")) {
            LoadPlayer();
        } else {
            LoadNewPlayer();
        }

        //if (File.Exists(Application.persistentDataPath + "/player.sdat")) {

        //    File.Delete(Application.persistentDataPath + "/player");
        //}
    }

	void Start() {
        bResetWorld = false;
        fDistToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y; // gets the distance between the box collider and the ground
        capsuleCollider2D = GetComponent<CapsuleCollider2D>(); // get the capsule collider componenet on our player
        playerRigidbody2D = GetComponent<Rigidbody2D>(); // get the rigidbody componenet on our player
        
        iHealAmmount = blackboard.iHealCount; // set the ammount of times the player can heal
        fMaxStamina = blackboard.fMaxStamina; // set the maximum ammount of stamina of the player
        blackboard.fCurrentStamina = fMaxStamina; // set the current stamina of the player to the highest ammount of stamina

        bDashIsReady = true; // allow the player to dash
        bCanPlayerMove = true; // allow the player to move

        inventory = new Inventory(UseItem);

        foreach (Items item in itemList) {
            Debug.Log(item);
            inventory.AddItem(item);
        }
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);

        go_UI_Inventory.SetActive(true);
    }

    void Update() {

        itemList = inventory.GetItemList();
        if(blackboard.fPlayerHealth <= 0) { // if the players health is 0 then the game is over
            go_GameOverCanvas.SetActive(true);
            Time.timeScale = 0.01f;
        } else {
            Movement(); // call the movement function
            Jumping(); // call the jumping function
            MoveCamera(); // call the movecamera function
            PlayerDash(); // call the playerdash function
            Attack(); // call the attack function
            Crouch();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            go_PauseCanvas.SetActive(true);
        }

        fCameraMaxLookHeight = transform.position.x + 5f; // set how high the camera can look
        fCameraMinLookHeight = transform.position.x - 3.5f; // set how low the camera can look

        iFuelAmmount = blackboard.iFuelCount; // constantly update the ammount of fuel the player has
        fPlayerMaxHealth = blackboard.fPlayerMaxHealth;
    }

    private void UseItem(Items item) {
        switch (item.itemType) {
            case Items.ItemType.SMG:
                weaponManager.PickUpWeapon(go_SMGPrefab);
                weaponManager.EquipWeapon(go_SMGPrefab);
                go_CurrentWeapon = go_SMGPrefab;
                foreach (AnimatorControllerParameter parameter in playerAnimator.parameters) {
                    playerAnimator.SetBool(parameter.name, false);
                }
                playerAnimator.SetBool("SMG", true);
                playerAnimator.SetBool("Crouching", bIsCrouching);

                for (int i = 0; i < go_aOverHeatBars.Length; i++) {
                    go_aOverHeatBars[i].SetActive(true);
                }
                //GameObject currentEquippedWeapon = go_SMGPrefab;
                //go_WeaponManger.GetComponent<WeaponManager>().go_CurrentWeapon = currentEquippedWeapon;     
                break;
            case Items.ItemType.Shotgun:
                weaponManager.PickUpWeapon(go_ShotgunPrefab);
                weaponManager.EquipWeapon(go_ShotgunPrefab);
                go_CurrentWeapon = go_ShotgunPrefab;

                foreach (AnimatorControllerParameter parameter in playerAnimator.parameters) {
                    playerAnimator.SetBool(parameter.name, false);
                }

                playerAnimator.SetBool("Shotgun", true);
                playerAnimator.SetBool("Crouching", bIsCrouching);

                for (int i = 0; i < go_aOverHeatBars.Length; i++) {
                    go_aOverHeatBars[i].SetActive(true);
                }
                //GameObject currentEquippedWeapon = go_SMGPrefab;
                //go_WeaponManger.GetComponent<WeaponManager>().go_CurrentWeapon = currentEquippedWeapon;     
                break;
            case Items.ItemType.Health:
                PlayerHealing();
                inventory.RemoveItem(new Items { itemType = Items.ItemType.Health, iItemAmmount = 1 });
                break;
        }
    }

    public void DropWeapon() {
        foreach (AnimatorControllerParameter parameter in playerAnimator.parameters) {
            playerAnimator.SetBool(parameter.name, false);
        }
        weaponManager.DropWeapon(go_CurrentWeapon);

        for (int i = 0; i < go_aOverHeatBars.Length; i++) {
            go_aOverHeatBars[i].SetActive(false);
        }

    }

    /// <summary>
    /// our ground check that sees if the player is more than 0.1f off the floor
    /// </summary>
    /// <returns> returns false if the player is more the 0.1f off the floor </returns>
    private bool IsGrounded() {
        float fExtraHeightTest = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2D.bounds.center, Vector2.down, fDistToGround + fExtraHeightTest, platformLayerMask);
        Color rayColor;
        if(raycastHit.collider != null) {
            rayColor = Color.green;
		} else {
            rayColor = Color.red;
		}

        Debug.DrawRay(capsuleCollider2D.bounds.center, Vector2.down * (fDistToGround + fExtraHeightTest), rayColor);

        return raycastHit.collider != null;
    }

    /// <summary>
    /// Allows the player to move left and right, also allows for in air control by using the above ground check function
    /// </summary>
	private void Movement() {
        float fMidAirControl = 8f;
        if (Input.GetKey(KeyCode.A) && bCanPlayerMove == true) {
            bFacingRight = false;
            if (bFacingRight == false) {
                transform.eulerAngles = new Vector3(0, -180, 0);
            }
            if (IsGrounded()) {
                playerAnimator.SetBool("Running", true);
                playerRigidbody2D.velocity = new Vector2(-fMoveSpeed, playerRigidbody2D.velocity.y);

                //

            } else { 
                playerRigidbody2D.velocity += new Vector2(-fMoveSpeed * fMidAirControl * Time.deltaTime, 0);
                playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -fMoveSpeed, +fMoveSpeed), playerRigidbody2D.velocity.y);
			}           
        } else if (Input.GetKey(KeyCode.D) && bCanPlayerMove == true) {
            bFacingRight = true;
            if (bFacingRight == true) {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (IsGrounded()) {
                playerAnimator.SetBool("Running", true);
                playerRigidbody2D.velocity = new Vector2(+fMoveSpeed, playerRigidbody2D.velocity.y);

                //

            } else {
                playerAnimator.SetBool("Running", false);
                playerRigidbody2D.velocity += new Vector2(+fMoveSpeed * fMidAirControl * Time.deltaTime, 0);
                playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -fMoveSpeed, +fMoveSpeed), playerRigidbody2D.velocity.y);
            }
        } else {
            playerAnimator.SetBool("Running", false);
            if (IsGrounded()) {
                playerRigidbody2D.velocity = new Vector2(0, playerRigidbody2D.velocity.y);
			}
		}
    }
    

    /// <summary>
    /// Function that lets the player climb a ladder
    /// </summary>
    private void ClimbLadder() {
        float fClimbSpeed = 3f;
		if (Input.GetKey(KeyCode.W)) {
            playerRigidbody2D.velocity = new Vector2(playerRigidbody2D.velocity.x, +fClimbSpeed);
        }
    }

    private void Crouch() {
		if (Input.GetKeyDown(KeyCode.C)) {
            playerAnimator.SetBool("Crouching", true);
            bCanPlayerMove = false;
            bIsCrouching = true;
		} else if (Input.GetKeyUp(KeyCode.C)) {
            playerAnimator.SetBool("Crouching", false);
            bIsCrouching = false;
            bCanPlayerMove = true;
        }
	}
    /// <summary>
    /// Allows the player to jump by adding and up vector to the velocity of the rigidbody
    /// </summary>
	private void Jumping() {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && blackboard.fCurrentStamina >= 10) {
            playerAnimator.SetBool("bJumping", true);
            playerUI.UseStamina(10);
            float fJumpVelocity = 13f;
            playerRigidbody2D.velocity = Vector2.up * fJumpVelocity;
        } else {
            playerAnimator.SetBool("bJumping", false);
        }
    }

    /// <summary>
    /// By using the up and down arrows the player can move the camera to look at their surroundings
    /// </summary>
    private void MoveCamera() {
        if (Input.GetKeyDown(KeyCode.RightArrow) && mainCamera.transform.position.x < fCameraMaxLookHeight) {
            bCanPlayerMove = false;
            Vector3 v3TargetPos = new Vector3(mainCamera.transform.position.x + 5, mainCamera.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, v3TargetPos, transform.position.x + 5);
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            bCanPlayerMove = true;
		}
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && mainCamera.transform.position.x > fCameraMinLookHeight) {
            bCanPlayerMove = false;
            Vector3 v3TargetPos = new Vector3(mainCamera.transform.position.x - 5, mainCamera.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, v3TargetPos, transform.position.x + 5);
		} else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            bCanPlayerMove = true;
        }
    }

    /// <summary>
    /// Uses a bool to know which direction the player is facing and than adds a 1.5f to their position so they can dash
    /// </summary>
    private void PlayerDash() {
        RaycastHit2D blockedInfoRight = Physics2D.Raycast(transform.position, Vector2.right, 7.1f, blockLayerMask); 
        RaycastHit2D blockedInfoLeft = Physics2D.Raycast(transform.position, Vector2.left, 7.1f, blockLayerMask); 


        if (bDashIsReady) {
            if (Input.GetKeyDown(KeyCode.V) && bFacingRight == true && blackboard.fCurrentStamina >= 15 && blockedInfoRight.collider == null) {
                playerUI.UseStamina(15);
                playerRigidbody2D.position = new Vector2(playerRigidbody2D.position.x + 7.0f, playerRigidbody2D.position.y);
                bDashIsReady = false;
                bCanPlayerMove = false;
                StartCoroutine("WaitForDash");
                StartCoroutine("WaitForPlayerMoveAfterDash");
            } else if(Input.GetKeyDown(KeyCode.V) && bFacingRight == false && blackboard.fCurrentStamina >= 15 && blockedInfoLeft.collider == null) {
                playerUI.UseStamina(15);
                playerRigidbody2D.position = new Vector2(playerRigidbody2D.position.x - 7.0f, playerRigidbody2D.position.y);
                bDashIsReady = false;
                bCanPlayerMove = false;
                StartCoroutine("WaitForDash");
                StartCoroutine("WaitForPlayerMoveAfterDash");
            }

        }

    }

    /// <summary>
    /// sets the bool on the animator which plays the attacking sprites
    /// </summary>
    private void Attack() {
		if (Input.GetKey(KeyCode.Mouse1)) {
            playerAnimator.SetBool("bFacingRight", bFacingRight);
            playerAnimator.SetBool("bAttacking", true);
            StartCoroutine("WaitForAttack");
        }
	}

    /// <summary>
    /// When the player pressing the healing key they get two health back
    /// </summary>
    private void PlayerHealing() {
        blackboard.fPlayerHealth = blackboard.fPlayerHealth + 2;
        blackboard.iHealCount -= 1;
	}

    /// <summary>
    /// This function is used to reset the world when players save such as giving back the players heals or respawing enemies
    /// </summary>
    public void ResetWorld() {
        blackboard.fPlayerHealth = blackboard.fPlayerMaxHealth;
        blackboard.iHealCount = iHealAmmount;
	}

    /// <summary>
    /// stops the player from moving temporaily after dashing so they cannot do it in quick succession
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForPlayerMoveAfterDash() {
        yield return new WaitForSeconds(0.4f);
        bCanPlayerMove = true;
    }

    /// <summary>
    /// Sets a recharge value on the dash
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForDash() {
        yield return new WaitForSeconds(1.5f);
        bDashIsReady = true;
    }

    /// <summary>
    /// sets a cool down on the attack so the player cannot spam attacks
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForAttack() {
        yield return new WaitForSeconds(0.4f);
        playerAnimator.SetBool("bAttacking", false);
    }

    /// <summary>
    /// Saves the players data such as ther max health, position, stamina etc.
    /// </summary>
    public void SavePlayer() {
        SaveSystem.SavePlayer(this); // saves the current settings and position of the player
        blackboard.fPlayerHealth = blackboard.fPlayerMaxHealth;
        bResetWorld = true;
        GameObject[] fuel = GameObject.FindGameObjectsWithTag("Fuel");
        for(int i = 0; i < fuel.Length; i++) {
            Destroy(fuel[i]);
		}

        StartCoroutine("WaitForSave");
    }

    public IEnumerator WaitForSave() {
        playerSaveCanvas.transform.GetChild(1).gameObject.SetActive(false);
        playerSaveCanvas.transform.GetChild(0).gameObject.SetActive(true);
        bCanPlayerMove = false;
        FadeToBlack();
        yield return new WaitForSeconds(4.0f);
        FadeFromBlack();
        playerSaveCanvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    void FadeToBlack() {
        FadeToBlackImage.GetComponent<Image>().color = Color.black;
        FadeToBlackImage.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        FadeToBlackImage.GetComponent<Image>().CrossFadeAlpha(1.0f, 2f, false);
    }

    void FadeFromBlack() {
        FadeToBlackImage.GetComponent<Image>().color = Color.black;
        FadeToBlackImage.GetComponent<Image>().canvasRenderer.SetAlpha(1.0f);
        FadeToBlackImage.GetComponent<Image>().CrossFadeAlpha(0.0f, 2f, false);
        bCanPlayerMove = true;
    }

    /// <summary>
    /// loads in the players previous save
    /// </summary>
    public void LoadPlayer() {
        go_GameOverCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        FadeFromBlack();
        PlayerData data = SaveSystem.LoadPlayer(); // loads the data of the last performed save 

        Vector2 v2PlayerPosition; // create a vector to set the players position
        v2PlayerPosition.x = data.afPlayerPositions[0]; 
        v2PlayerPosition.y = data.afPlayerPositions[1];
        transform.position = v2PlayerPosition; // set the x and y of our player to that of the x and y in the save file
        blackboard.iFuelCount = data.iPlayerFuelAmmount;
        blackboard.fPlayerMaxHealth = data.fPlayerMaxHealthAmmount;
        blackboard.iHealCount = data.iHealAmmount;
        blackboard.fMaxStamina = data.fMaxStamina;
        blackboard.fPlayerHealth = blackboard.fPlayerMaxHealth; // set the players health to the maximum health value
        itemList = data.itemList;

        mainCamera.transform.position = new Vector3(data.afCameraPositions[0], data.afCameraPositions[1], data.afCameraPositions[2]);
    }

    public void Retry() {
        if (File.Exists(Application.persistentDataPath + "/player.sdat")) {
            LoadPlayer();
        } else {
            LoadNewPlayer();
        }
    }
    
    public void LoadNewPlayer() {
        go_GameOverCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        FadeFromBlack();

        Vector2 v2PlayerPosition; // create a vector to set the players position
        v2PlayerPosition.x = -22.0f;
        v2PlayerPosition.y = -0.0f;
        transform.position = v2PlayerPosition; // set the x and y of our player to that of the x and y in the save file
        blackboard.iFuelCount = 0;
        blackboard.fPlayerMaxHealth = 3;
        blackboard.iHealCount = 3;
        blackboard.fMaxStamina = 100;
        blackboard.fPlayerHealth = 1;

        mainCamera.transform.position = new Vector3(-23.0f, 0.5f, -10.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Fuel") {
            blackboard.iFuelCount = blackboard.iFuelCount + go_FuelPrefab.GetComponent<Fuel>().iFuelDropAmmount;
            Destroy(collision.gameObject);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Save") {
            playerSaveCanvas.gameObject.SetActive(true);
            playerSaveCanvas.transform.GetChild(0).gameObject.SetActive(false);
            playerSaveCanvas.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (collision.gameObject.tag == "Fuel") {
            blackboard.iFuelCount = blackboard.iFuelCount + go_FuelPrefab.GetComponent<Fuel>().iFuelDropAmmount;
            Destroy(collision.gameObject);
        }

        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if(itemWorld != null) {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf(); 
		}

        if (collision.gameObject.tag == "Ladder") {
            ClimbLadder();
        }

        

    }

	private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Weapon" && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("Weapon");
            go_WeaponManger.GetComponent<WeaponManager>().PickUpWeapon(collision.gameObject);
        }

        if (collision.gameObject.tag == "Save") {
            if (Input.GetKey(KeyCode.X)) {
                SavePlayer();
            }
        }

        if(collision.gameObject.tag == "Ladder") {
            ClimbLadder();
		}

        //
        if (collision.GetComponent<SoundManager>() && (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.A)))) {

            //
            soundManager.PlaySnowAudio();

        }

        //      if(collision.gameObject.tag == "Snow") {
        //	if (SnowCrunching.isPlaying == false) {
        //              SnowCrunching.Play();
        //	}
        //}

    }

	private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Save") {
            playerSaveCanvas.gameObject.SetActive(false);
        }
    }
}
