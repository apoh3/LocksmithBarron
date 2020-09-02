using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            StopAllCoroutines();
            SceneManager.LoadScene("Puzzle");
        }
    }
}
