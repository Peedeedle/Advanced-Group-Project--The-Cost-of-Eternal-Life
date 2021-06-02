using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlagueWatcherDialog : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    [TextArea(5, 10)]
    public string[] sentences;
    private int iIndex;
    public float fTypingSpeed;
    public GameObject DialogCanvas;

    public GameObject go_ContinueButton;

    public PlayerController playerController;

    public PlagueWatcher plagueWatcher;

    void Start() {

    }


    void Update() {
        if (textDisplay.text == sentences[iIndex]) {
            go_ContinueButton.SetActive(true);
        }
    }

    IEnumerator Type() {
        DialogCanvas.SetActive(true);
        textDisplay.text = "";
        foreach (char letter in sentences[iIndex].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(fTypingSpeed);
        }
    }

    public void NextSentence() {
        go_ContinueButton.SetActive(false);

        if (iIndex < sentences.Length - 1) {
            iIndex++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
                textDisplay.text = "";
                DialogCanvas.SetActive(false);
                playerController.bBossFightCameraActive = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player" && plagueWatcher.bAttacked == false) {
            StartCoroutine(Type());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        textDisplay.text = "";
        DialogCanvas.SetActive(false);
        iIndex = 0;
    }
}
