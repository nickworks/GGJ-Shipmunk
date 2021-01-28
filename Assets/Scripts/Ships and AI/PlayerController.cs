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
        InputAimMouse();
        // InputAimController();
    }

    private void InputMove() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        wantsToMove = (h * h + v * v > .2f);
        if (wantsToMove) dirToMove = dir.normalized;
    }
    private void InputAimMouse() {
        float h = Input.GetAxisRaw("AimX");
        float v = Input.GetAxisRaw("AimY");

        Vector3 dir = new Vector3(h, 0, v);
        wantsToAim = (h * h + v * v > .2f);
        if (wantsToAim) dirToAttack = dir.normalized;
        
    }
    private void InputAimController() {
        
    }
}
