using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject go_EnemyPrefab;
    public Transform RespawnTransform;

    Respawner[] respawners;
    private PlayerController playerController;

	private void Awake() {
	}

	void Start()
    {
        respawners = (Respawner[])GameObject.FindObjectsOfType(typeof(Respawner));
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.bResetWorld == true) {
            foreach (Respawner respawner in respawners) {
                respawner.Respawn();
            }
            playerController.bResetWorld = false;
        }  else {
            playerController.bResetWorld = false;
        }
    }

    public void Respawn() {
        if(transform.childCount < 1) {
            GameObject enemy = Instantiate(go_EnemyPrefab, RespawnTransform.position, RespawnTransform.rotation);
            enemy.transform.SetParent(this.gameObject.transform);
        }

    }
}
