using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour {


    private float prevTimeScale = 1;
    EventSystem eventSystem;
    public Button defaultSelectedButton;

    void Start() {
        prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
    }
    public void BttnResume() {
        Unpause();
    }
    public void BttnQuit() {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Update(){
        if(eventSystem){
            if(eventSystem.currentSelectedGameObject == null){
                if(defaultSelectedButton) defaultSelectedButton.Select();
            }
        }
    }
    public void Unpause() {
        Time.timeScale = prevTimeScale;
        //print($"setting timescale to {Time.timeScale}");
        Destroy(gameObject);
    }
    public void OnUnpause(InputAction.CallbackContext cxt){
        Unpause();
    }
}
