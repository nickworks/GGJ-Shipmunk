using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : _Ability {
    public float radius = 10;
    public float impulse = 400;
    public override void DoAbility(float mult = 1) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders) {
            SpaceRigidbody body = c.GetComponent<SpaceRigidbody>();
            if (!body) continue;
            if (body.allegiance == ship.controller.allegiance) continue;
            Vector3 d = (body.transform.position - transform.position);
            float mag = d.magnitude;
            float p = 1 - mag / radius;
            body.AddForce(d * impulse * p / mag);
        }
    }
}
