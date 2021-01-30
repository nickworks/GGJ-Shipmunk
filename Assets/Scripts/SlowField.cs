using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        HealthAndEffects hp = other.GetComponent<HealthAndEffects>();
        if(hp) hp.AddCondition(new HealthAndEffects.Condition.Slow());
    }
    
}
