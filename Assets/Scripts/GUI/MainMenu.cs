using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    EventSystem events;
    public Button defaultSelectedButton;
    void Start() {
        events = GameObject.FindObjectOfType<EventSystem>();
    }
    void Update() {
        if(events.currentSelectedGameObject == null) {
            if(defaultSelectedButton) defaultSelectedButton.Select();
        }
    }
    public void BttnPlay() {
        SceneManager.LoadScene("BulletHell", LoadSceneMode.Single);
    }
    public void BttnAbout() {

    }
    public void BttnExit() {
        Application.Quit();
    }
}
