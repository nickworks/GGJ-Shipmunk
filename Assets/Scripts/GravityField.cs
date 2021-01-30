using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour {

    void OnTriggerStay(Collider other) {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body) {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            if (body.isKinematic) {
                float dot = Vector3.Dot(dir, other.transform.right);
                float angVel = (dot > 0) ? 1 : -1;
                body.transform.Rotate(0, angVel, 0);
            } else {
                body.AddForce(20 * dir);
            }
            return;
        }
    }
}
