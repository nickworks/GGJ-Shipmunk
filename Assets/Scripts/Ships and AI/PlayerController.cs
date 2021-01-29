using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    Camera cam;
    ScrollerController scroller;

    public HUDController hudPrefab;
    protected HUDController hud;

    void OnDestroy() {
        if(hud) Destroy(hud.gameObject);
    }
    protected override void Init() {
        base.Init();
        scroller = FindObjectOfType<ScrollerController>();
        cam = Camera.main;
        allegiance = Allegiance.Player;
        hud = Instantiate(hudPrefab);
        hud.RebuildViews(this);
    }
    void Update() {
        InputMove();
        InputAimMouse();
        //InputAimController();

        wantsToAbilityA = Input.GetButton("Fire1");
        wantsToAbilityB = Input.GetButton("Fire2");

        float axis3 = Input.GetAxisRaw("Fire3");

        wantsToAbilityC = axis3 > .2f;
        wantsToAbilityD = axis3 < -.2f || Input.GetButton("Fire4");
    }
    private void LateUpdate() {
        if(scroller.currentMode == ScrollerController.CameraMode.Scrolling) ship.Clamp(scroller.min, scroller.max);
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
