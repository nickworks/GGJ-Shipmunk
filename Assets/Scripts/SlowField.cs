using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        SpaceRigidbody hp = other.GetComponent<SpaceRigidbody>();
        if(hp) hp.AddCondition(new SpaceRigidbody.Condition.Slow());
    }
    
}
