using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    void Start() {
        Time.timeScale = 0;
    }
    public void Update() {
        
    }
    public void BttnReplay() {
        Time.timeScale = 1;
        SceneManager.LoadScene("BulletHell");
    }
    public void BttnQuit() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
