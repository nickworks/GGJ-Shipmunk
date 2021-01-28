using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    Camera cam;
    protected override void Init() {
        base.Init();
        cam = Camera.main;
        allegiance = Allegiance.Player;
    }
    void Update() {
        InputMove();
        InputAimMouse();
        //InputAimController();
        wantsToAttack = Input.GetButton("Fire1");
    }

    private void InputMove() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        wantsToMove = (h * h + v * v > .2f);
        if (wantsToMove) dirToMove = new Vector3(h, 0, v).normalized;
    }
    private void InputAimMouse() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float dis)) {
            Vector3 mouseWorldPos = ray.GetPoint(dis);
            Vector3 dir = mouseWorldPos - transform.position;
            dirToAim = new Vector3(dir.x, 0, dir.z).normalized;

            wantsToAim = true;
        }
    }
    private void InputAimController() {
        float h = Input.GetAxisRaw("AimX");
        float v = Input.GetAxisRaw("AimY");

        wantsToAim = (h * h + v * v > .2f);
        if (wantsToAim) dirToAim = new Vector3(h, 0, v).normalized;
    }
}
