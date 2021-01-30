using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceRigidbody : MonoBehaviour {

    private Rigidbody body;

    bool first = true;

    [SerializeField]
    private float _timeScale = 1;
    public float timeScale {
        get { return _timeScale; }
        set {
            if (_timeScale == value) return;
            if (!body) return;
            if (!first) {
                body.mass *= timeScale;
                body.velocity /= timeScale;
                body.angularVelocity /= timeScale;
            }
            first = false;
            _timeScale = Mathf.Abs(value);
            body.mass /= timeScale;
            body.velocity *= timeScale;
            body.angularVelocity *= timeScale;
        }
    }

    void Awake() {
        body = GetComponent<Rigidbody>();
        timeScale = _timeScale;
    }
    public void AddForce(Vector3 v) {
        body.AddForce(v);
    }
    public void SetVelocity(Vector3 v) {
        body.velocity = v;
    }
    public void SetAngularVelocity(Vector3 v) {
        body.angularVelocity = v;
    }
}
