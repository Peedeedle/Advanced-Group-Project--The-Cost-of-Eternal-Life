using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackedOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeTo(float aValue, float aTime) {
        float alpha = this.GetComponent<SpriteRenderer>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            this.GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
            StartCoroutine(FadeTo(0.0f, 0.65f));
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            StartCoroutine(FadeTo(0.97f, 0.65f));
        }
    }
}
