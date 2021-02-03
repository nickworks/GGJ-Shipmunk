using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveInPlace : MonoBehaviour {

    public Vector3 velocity;
    public float addedRandomVelocity = 1;
    protected Vector3 euler;

    private void Start() {
        velocity += (Random.onUnitSphere * addedRandomVelocity);
    }
    void Update() {
        euler += velocity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(euler);
    }
}
