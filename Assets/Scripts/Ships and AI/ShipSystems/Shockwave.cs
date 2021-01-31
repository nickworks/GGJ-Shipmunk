using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : _Ability {
    public float radius = 10;
    public float impulse = 400;
    public LineRenderer artRing;

    public float animLength = 0.5f;
    protected float animTimer = 0;
    protected bool animating = false;

    public void Awake() {
        StopAnim();
    }
    public void Update() {

        if (animating && artRing) {
            animTimer += Time.deltaTime;
            float p = animTimer / animLength;

            float pFinal = 1 - (1 - p) * (1 - p);

            artRing.transform.localScale = Vector3.one * AnimMath.Lerp(0, radius, pFinal);
            artRing.widthMultiplier = (1 - p) * (1 - p);
            if (p > 1) {
                p = 1;
                StopAnim();
            }
        }
    }
    void StopAnim() {
        artRing.gameObject.SetActive(false);
        animating = false;
        artRing.transform.localScale = Vector3.zero;
    }
    protected override Spaceship.States._State DoAbility(float mult = 1) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in colliders) {
            SpaceRigidbody body = c.GetComponent<SpaceRigidbody>();
            if (!body) continue;
            if (body.allegiance == ship.controller.allegiance) continue;
            Vector3 d = (body.transform.position - transform.position);
            float mag = d.magnitude;
            float p = 1 - mag / radius;
            body.AddForce(d * impulse * p / mag, ForceMode.VelocityChange);
        }
        artRing.gameObject.SetActive(true);
        animating = true;
        animTimer = 0;
        return null;
    }
}
