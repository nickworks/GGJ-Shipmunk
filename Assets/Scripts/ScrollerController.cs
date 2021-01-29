using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerController : MonoBehaviour
{
    public enum CameraMode {
        Ortho,
        Persp
    }

    public CameraMode currentMode = CameraMode.Ortho;
    
    private Camera cam;
    public Transform thingToFollow;
    public Transform thingFollowing;
    
    public Vector3 scrollVelocity = new Vector3();
    private Vector3 actualVelocity = new Vector3();

    public float transitionLength = 1;
    private float transitionTimer = 0;

    public float zoom { get; private set; }

    private void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {

        if(Input.GetButtonDown("Jump")) currentMode = (currentMode == CameraMode.Ortho) ? CameraMode.Persp : CameraMode.Ortho;

        // scroll:
        Vector3 targetVelocity = (currentMode == CameraMode.Ortho) ? scrollVelocity : Vector3.zero;
        actualVelocity = AnimMath.Slide(actualVelocity, targetVelocity, 0.05f, Time.deltaTime);
        transform.position += actualVelocity * Time.deltaTime;

        // dolly/track camera:
        DollyTrackCamera();
    }

    private void DollyTrackCamera() {

        if (cam == null) return;

        if (currentMode == CameraMode.Ortho) transitionTimer -= Time.deltaTime;
        if (currentMode == CameraMode.Persp) transitionTimer += Time.deltaTime;
        float p = transitionTimer / transitionLength;
        if (p > 1) {
            p = 1;
            transitionTimer = transitionLength;
        }
        if (p < 0) {
            p = 0;
            transitionTimer = 0;
        }
        float degrees = AnimMath.Smooth(10, 60, p);
        cam.fieldOfView = degrees;

        zoom = AnimMath.Smooth(10, 20, p);

        Vector3 pos = cam.transform.localPosition;
        pos.z = -zoom / Mathf.Tan(degrees * Mathf.Deg2Rad / 2);
        cam.transform.localPosition = pos;

        if (thingToFollow && thingFollowing) { // ease towards target:
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, thingToFollow.position, p);
            thingFollowing.position = AnimMath.Slide(thingFollowing.position, lerpedPosition, 0.05f, Time.deltaTime);
        }
    }

    public Vector3 min {
        get {
            return new Vector3(transform.position.x - zoom * cam.aspect * .9f, 0, transform.position.z - zoom);
        }
    }
    public Vector3 max {
        get {
            return new Vector3(transform.position.x + zoom * cam.aspect * .9f, 0, transform.position.z + zoom);
        }
    }

}
