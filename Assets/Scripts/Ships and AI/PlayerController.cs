using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    void Start() {
        allegiance = Allegiance.Player;
    }
    
    void Update() {
        InputMove();
        InputAimMouse();
        // InputAimController();
    }

    private void InputMove() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 axisMove = new Vector3(h, 0, v);
        // TODO: tell ship
    }
    private void InputAimMouse() {
        
    }
    private void InputAimController() {
        
    }
}
