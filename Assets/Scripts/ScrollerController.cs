using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ScrollerController : MonoBehaviour
{
    public enum CameraMode {
        Scrolling,
        FreeRoam
    }

    public CameraMode currentMode = CameraMode.Scrolling;
    
    public Camera cam1;
    public Camera cam2;

    private PostProcessVolume postProcess;
    public Transform thingToFollow;
    public Transform thingFollowing;
    
    public Vector3 scrollVelocity = new Vector3();
    private Vector3 actualVelocity = new Vector3();

    public float transitionLength = 1;
    private float transitionTimer = 0;

    public float zoom { get; private set; }

    private void Start() {

        postProcess = cam1.GetComponent<PostProcessVolume>();
    }

    void Update() {

        if(Input.GetButtonDown("Jump")) currentMode = (currentMode == CameraMode.Scrolling) ? CameraMode.FreeRoam : CameraMode.Scrolling;

        // scroll:
        Vector3 targetVelocity = (currentMode == CameraMode.Scrolling) ? scrollVelocity : Vector3.zero;
        actualVelocity = AnimMath.Slide(actualVelocity, targetVelocity, 0.01f, Time.deltaTime);
        transform.position += actualVelocity * Time.deltaTime;

        // dolly/track camera:
        DollyTrackCamera();
    }

    private void DollyTrackCamera() {

        if (cam1 == null) return;

        if (currentMode == CameraMode.Scrolling) transitionTimer -= Time.deltaTime;
        if (currentMode == CameraMode.FreeRoam) transitionTimer += Time.deltaTime;
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
        cam2.fieldOfView = cam1.fieldOfView = degrees;

        zoom = AnimMath.Smooth(10, 20, p);

        Vector3 pos = cam1.transform.localPosition;
        pos.z = -zoom / Mathf.Tan(degrees * Mathf.Deg2Rad / 2);
        cam1.transform.localPosition = pos;

        if (postProcess) postProcess.weight = AnimMath.Lerp(1, 0.25f, 1-(1-p)*(1-p));

        if (thingToFollow && thingFollowing) { // ease towards target:
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, thingToFollow.position, p);
            thingFollowing.position = AnimMath.Slide(thingFollowing.position, lerpedPosition, 0.05f, Time.deltaTime);
        }
    }

    public Vector3 min {
        get {
            return new Vector3(transform.position.x - zoom * cam1.aspect * .9f, 0, transform.position.z - zoom);
        }
    }
    public Vector3 max {
        get {
            return new Vector3(transform.position.x + zoom * cam1.aspect * .9f, 0, transform.position.z + zoom);
        }
    }

}
