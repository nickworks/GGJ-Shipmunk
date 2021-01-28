using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public Transform art;


    float lifespan = 10;
    float age = 0;
    float pitch = 60;
    Rigidbody body;
    
    void Start() {
        body = GetComponent<Rigidbody>();

        float quarterRange = 15;
        
        Vector3 vel = Vector3.zero;
        vel.y = Random.Range(-quarterRange, quarterRange) + Random.Range(-quarterRange, quarterRange);
        body.angularVelocity = vel;
        pitch = Random.Range(-30, -10);

        transform.localScale = Vector3.one * Random.Range(1.5f, 5f);

    }

    
    void Update() {
        age += Time.deltaTime;
        if (age > lifespan) Destroy(gameObject);
        art.rotation = Quaternion.Euler(pitch,0,0) * transform.rotation * Quaternion.Euler(90, 0, 0);
    }
}
