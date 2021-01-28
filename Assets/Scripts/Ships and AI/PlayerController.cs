using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    protected override void Init() {
        base.Init();
        allegiance = Allegiance.Player;
    }
    void Update() {
        InputMove();
        //InputAimMouse();
        InputAimController();
    }

    private void InputMove() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        wantsToMove = (h * h + v * v > .2f);
        if (wantsToMove) dirToMove = new Vector3(h, 0, v).normalized;
    }
    private void InputAimMouse() {
        
        
    }
    private void InputAimController() {
        float h = Input.GetAxisRaw("AimX");
        float v = Input.GetAxisRaw("AimY");

        wantsToAim = (h * h + v * v > .2f);
        if (wantsToAim) dirToAim = new Vector3(h, 0, v).normalized;
    }
}
