using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {


    private float prevTimeScale = 1;

    void Start() {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void Update() {
        if (Input.GetButtonDown("Pause") && Time.timeScale == 0) {
            Unpause();
        }
    }
    public void BttnResume() {
        Unpause();
    }
    public void BttnQuit() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Unpause() {
        Time.timeScale = prevTimeScale;
        Destroy(gameObject);
    }
}
