using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    EventSystem events;

    void Start() {
        events = GetComponentInChildren<EventSystem>();
    }

    
    void Update() {
        if(events.currentSelectedGameObject == null) {
            if(events.firstSelectedGameObject != null) {
                events.SetSelectedGameObject(events.firstSelectedGameObject);
            }
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
