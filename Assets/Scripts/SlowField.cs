using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Spaceship ship = other.GetComponent<Spaceship>();
        if (ship) {
            
        }
    }
    
}
