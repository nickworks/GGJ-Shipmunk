using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller {
    
    Camera cam;
    ScrollerController scroller;

    public HUDController hudPrefab;
    public PauseMenuController pausePrefab;
    public GameOverController gameOverPrefab;
    protected HUDController hud;

    new SpaceRigidbody rigidbody;

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
        InputAim();

        UpdateHUD();
    }
    private void LateUpdate() {
        if(scroller.currentMode == ScrollerController.CameraMode.Scrolling) ship.Clamp(scroller.min, scroller.max);
    }

    private void InputMove() {        
        wantsToMove = (moveAxis.sqrMagnitude > .2f);
        if (wantsToMove) dirToMove = new Vector3(moveAxis.x, 0, moveAxis.y).normalized;
    }
    private void InputAim() {
        wantsToAim = (lookAxis.sqrMagnitude > .2f);
        if (wantsToAim) dirToAim = new Vector3(lookAxis.x, 0, lookAxis.y).normalized;
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
    #region Input UnityEvent Callbacks

    /// <summary>
    /// This stores the direction that the player is trying to move.
    /// We store it so that we can then pass it to the pawn every Update().
    /// </summary>
    Vector2 moveAxis = Vector2.zero;
    Vector2 lookAxis = Vector2.zero;

    bool IsPaused {
        get {
            return Time.timeScale == 0;
        }
    }

    /// <summary>
    /// This is called when the Move axis is updated.
    /// Store the input and pass to pawn each Update().
    /// </summary>
    /// <param name="ctxt"></param>
    public void OnMove(InputAction.CallbackContext ctxt) {
        if (IsPaused) return;
        moveAxis = ctxt.ReadValue<Vector2>();
    }
    /// <summary>
    /// When aiming with a controller stick,
    /// calculate the angle and tell the pawn where to aim.
    /// </summary>
    /// <param name="ctxt"></param>
    public void OnLookStick(InputAction.CallbackContext ctxt) {
        if (IsPaused) return;
        lookAxis = ctxt.ReadValue<Vector2>();
    }
    /// <summary>
    /// When the mouse is moved, raycast from the camera,
    /// and tell pawn to aim where the ray hits.
    /// </summary>
    /// <param name="ctxt"></param>
    public void OnLookMouse(InputAction.CallbackContext ctxt) {
        if (IsPaused) return;
        if (cam == null) return;

        // mouse delta (not used)
        Vector3 mouseDelta = ctxt.ReadValue<Vector2>();

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float dis)) {
            Vector3 dir = ray.GetPoint(dis) - transform.position;
            lookAxis = new Vector2(dir.x, dir.z).normalized;
        } else {

        }
    }
    public void OnPause(InputAction.CallbackContext ctxt) {
        if (IsPaused) return;
        if (ctxt.phase != InputActionPhase.Started) return;

        Instantiate(pausePrefab);
    }
    void UpdateHUD(int shift = 0) {
        //gui.SwitchTomes(pawn.tomes, pawn.CurrentTome(), shift);
    }
    public void OnAttack1(InputAction.CallbackContext ctxt){
        if (ctxt.phase == InputActionPhase.Started) wantsToAbilityA = true;
        if (ctxt.phase == InputActionPhase.Canceled) wantsToAbilityA = false;
    }
    public void OnAttack2(InputAction.CallbackContext ctxt){
        if (ctxt.phase == InputActionPhase.Started) wantsToAbilityB = true;
        if (ctxt.phase == InputActionPhase.Canceled) wantsToAbilityB = false;
    }
    public void OnAttack3(InputAction.CallbackContext ctxt){
        if (ctxt.phase == InputActionPhase.Started) wantsToAbilityC = true;
        if (ctxt.phase == InputActionPhase.Canceled) wantsToAbilityC = false;
    }
    public void OnAttack4(InputAction.CallbackContext ctxt){
        if (ctxt.phase == InputActionPhase.Started) wantsToAbilityD = true;
        if (ctxt.phase == InputActionPhase.Canceled) wantsToAbilityD = false;
    }
    private ScrollerController scrollRef;
    public void OnToggleCam(InputAction.CallbackContext ctxt){
        if (ctxt.phase == InputActionPhase.Started) {
            if(scrollRef == null) scrollRef = GameObject.FindObjectOfType<ScrollerController>();
            if(scrollRef != null) scrollRef.ToggleCameraMode();
        }
    }
    #endregion
}
