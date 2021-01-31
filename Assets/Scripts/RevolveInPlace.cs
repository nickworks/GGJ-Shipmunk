using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveInPlace : MonoBehaviour {

    public Vector3 velocity;
    protected Vector3 euler;

    void Update() {
        euler += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(euler);
    }
}
