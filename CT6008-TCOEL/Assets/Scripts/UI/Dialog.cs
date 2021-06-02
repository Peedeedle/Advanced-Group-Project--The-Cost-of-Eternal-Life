using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI textDisplay;
    [TextArea(5, 10)]
    public string[] sentences;
    private int iIndex;
    public float fTypingSpeed;
    public GameObject DialogCanvas;
    public GameObject UpgradeCanvas;

    public GameObject go_ContinueButton;

    void Start()
    {
        
    }


    void Update() {
        if (textDisplay.text == sentences[iIndex]) {
            go_ContinueButton.SetActive(true);
		}
    }

    IEnumerator Type() {
        DialogCanvas.SetActive(true);
        textDisplay.text = "";
        foreach(char letter in sentences[iIndex].ToCharArray()) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(fTypingSpeed);
		}
	}

    public void NextSentence() {
        go_ContinueButton.SetActive(false);

        if(iIndex < sentences.Length - 1) {
            iIndex++;
            textDisplay.text = "";
            StartCoroutine(Type());
		} else {
            textDisplay.text = "";
            DialogCanvas.SetActive(false);
            if (UpgradeCanvas != null) {
                UpgradeCanvas.SetActive(true);
            }
        }
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
            StartCoroutine(Type());
        }
	}

	private void OnTriggerExit2D(Collider2D collision) {
        textDisplay.text = "";
        DialogCanvas.SetActive(false);
        iIndex = 0;
	}
}
