using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour {

    void OnTriggerStay(Collider other) {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body) {
            Vector3 d = (transform.position - other.transform.position).normalized;
            body.AddForce(d * 20);
            return;
        }
    }
}
