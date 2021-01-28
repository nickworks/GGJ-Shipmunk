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
    public Transform scroller;
    public Transform target;
    
    public Vector3 scrollVelocity = new Vector3();

    public float transitionLength = 1;
    private float transitionTimer = 0;


    private void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {

        // scroll:
        scroller.position += scrollVelocity * Time.deltaTime;

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

        float zoom = AnimMath.Smooth(10, 20, p);

        Vector3 pos = cam.transform.localPosition;
        pos.z = -zoom / Mathf.Tan(degrees * Mathf.Deg2Rad / 2);
        cam.transform.localPosition = pos;


        if (target) { // ease towards target
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, target.position, p);
            transform.position = AnimMath.Slide(transform.position, lerpedPosition, 0.5f, Time.deltaTime);
        }
    }
}
