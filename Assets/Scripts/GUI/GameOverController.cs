using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    EventSystem eventSystem;
    public Button defaultSelectedButton;

    void Start() {
        Time.timeScale = 0;
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }
    public void Update() {
        if(eventSystem){
            if(eventSystem.currentSelectedGameObject == null){
                if(defaultSelectedButton) defaultSelectedButton.Select();
            }
        }
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
