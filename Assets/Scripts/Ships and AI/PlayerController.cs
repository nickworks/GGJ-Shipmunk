using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {
    
    Camera cam;
    ScrollerController scroller;

    public bool isUsingGamepad = false;
    public HUDController hudPrefab;
    public PauseMenuController pausePrefab;
    public GameOverController gameOverPrefab;
    protected HUDController hud;

    SpaceRigidbody rigidbody;

    void OnDestroy() {
        if(hud) Destroy(hud.gameObject);
    }
    void Start() {
        rigidbody = GetComponent<SpaceRigidbody>();
        scroller = FindObjectOfType<ScrollerController>();
        cam = Camera.main;
        allegiance = Allegiance.Player;
        hud = Instantiate(hudPrefab);
        RebuildHUD();
    }
    public float GetHealthPercent(){
        if(rigidbody == null) return 0;
        return rigidbody.health / rigidbody.maxHealth;
    }
    void Update() {
        InputMove();
        float h = Input.GetAxisRaw("AimX");
        float v = Input.GetAxisRaw("AimY");
        float dMouseX = Input.GetAxisRaw("Mouse X");
        float dMouseY = Input.GetAxisRaw("Mouse Y");

        bool switchToController = (h * h + v * v > 0.2f);
        bool switchToMouse = (dMouseX != 0 || dMouseY != 0);

        if (!isUsingGamepad && switchToController) isUsingGamepad = true;
        else if ( isUsingGamepad && switchToMouse) isUsingGamepad = false;

        if (!isUsingGamepad) GetAimAxisFromMouse(ref h, ref v);
        InputAim(h, v);
        
        wantsToAbilityA = Input.GetButton("Fire1");
        wantsToAbilityB = Input.GetButton("Fire2");

        float axis3 = Input.GetAxisRaw("Fire3");

        wantsToAbilityC = axis3 > .2f;
        wantsToAbilityD = axis3 < -.2f || Input.GetButton("Fire4");

        if (Input.GetButtonDown("Pause") && Time.timeScale > 0) {
            Instantiate(pausePrefab);
        }
        UpdateHUD();
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
    private void GetAimAxisFromMouse(ref float axisH, ref float axisV) {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float dis)) {
            Vector3 mouseWorldPos = ray.GetPoint(dis);
            Vector3 dir = mouseWorldPos - transform.position;
            dir = new Vector3(dir.x, 0, dir.z).normalized;
            axisH = dir.x;
            axisV = dir.z;
            wantsToAim = true;
        } else {
            wantsToAim = false;
        }
    }
    private void InputAim(float axisH, float axisV) {
        wantsToAim = (axisH * axisH + axisV * axisV > .2f);
        if (wantsToAim) dirToAim = new Vector3(axisH, 0, axisV).normalized;
    }
    public void RebuildHUD() {
        if(hud) hud.RebuildViews(this);
    }
    public void UpdateHUD(){
        if(hud) hud.UpdateHealth(this);
    }
    /// <summary>
    /// Like OnDestroy(), but OnDie() is NOT called when unloading scenes.
    /// </summary>
    public void OnDie() {
        Instantiate(gameOverPrefab);
    }
}
