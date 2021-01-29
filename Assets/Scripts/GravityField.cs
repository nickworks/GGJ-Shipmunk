using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{

    void OnTriggerStay(Collider other) {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body) {
            Vector3 d = (transform.position - body.transform.position).normalized;
            body.AddForce(d * 20);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
