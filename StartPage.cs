using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPage : MonoBehaviour {
    public Image image;

    void Start() {
        image = GetComponent<Image>();
        StartFlashing();
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            StopAllCoroutines();
            SceneManager.LoadScene("Settings");
        }
    }

    IEnumerator Flash() {
        while(true) {
            string opac = image.color.a.ToString();

            if(opac == "0") {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                yield return new WaitForSeconds(0.5f);
            } else if(opac == "1") {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    void StartFlashing() {
        StopAllCoroutines();
        StartCoroutine("Flash");
    }

    void StopFlashing() {
        StopAllCoroutines();
    }
}
