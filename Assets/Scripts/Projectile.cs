using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class Projectile : MonoBehaviour {

    public enum Affect {
        DamageOnHit,
        SlowCondition,
        PoisonCondition,
        Gravity,
        Knockback
    }
    public Affect affect = Affect.DamageOnHit;
    public int affectAmount = 10;
    public bool shouldAffectOtherProjectiles = false;

    public float baseSpeed = 1;
    public float baseLifeSpan = 4;
    float age = 0;
    
    public bool destroyOnHit = true;
    [Tooltip("When using destroy-on-hit, use large numbers (1+). Otherwise, use small numbers (.1 - .5).")]
    public float affectDuration = 1;
    protected float cooldownToNextHit = 0;

    private float damageMult = 1;
    private float speedMult = 1;

    public SpaceRigidbody body { get; private set; }
    private List<SpaceRigidbody> overlappedObjects = new List<SpaceRigidbody>();

    public void InitBullet(Vector3 vel, Controller.Allegiance allegiance, float damageMult = 1, float speedMult = 1, float sizeMult = 1) {

        body = GetComponent<SpaceRigidbody>();
        body.SetVelocity(transform.forward * baseSpeed * speedMult + vel);

        body.allegiance = allegiance;
        this.damageMult = damageMult;
        this.speedMult = speedMult;
        transform.localScale *= sizeMult;
    }
    void Update() {
        age += Time.deltaTime * Time.timeScale;
        if (age > baseLifeSpan) {
            Destroy(gameObject);
        }
        if (!destroyOnHit) {
            cooldownToNextHit -= Time.deltaTime;
            if(cooldownToNextHit <= 0) {
                cooldownToNextHit = affectDuration;
                HitAll();
            }
        }
    }
    void OnDestroy() {
        overlappedObjects = null;
    }
    private void HitAll() {
        for (int i = overlappedObjects.Count - 1; i >= 0; i--) {
            SpaceRigidbody body = overlappedObjects[i];
            if (body) HitSingle(body);
            else overlappedObjects.RemoveAt(i);
        }
    }
    private void HitSingle(SpaceRigidbody injurableBody) {

        int dmg = (int)(affectAmount * damageMult);

        switch (affect) {
            case Affect.DamageOnHit:
                injurableBody.TakeDamage(dmg);
                break;
            case Affect.SlowCondition:
                injurableBody.AddCondition(new SpaceRigidbody.Condition.Slow(dmg, affectDuration));
                break;
            case Affect.PoisonCondition:
                injurableBody.AddCondition(new SpaceRigidbody.Condition.Poison(dmg, affectDuration));
                break;
            case Affect.Gravity:
                Vector3 dir = (transform.position - injurableBody.transform.position).normalized;
                injurableBody.AddForce(affectAmount * dir);
                break;
        }
    }
    private void OnTriggerEnter(Collider other) {

        if (ShouldIgnoreCollider(other, out SpaceRigidbody injurableBody)) return;

        if (destroyOnHit) {
            HitSingle(injurableBody);
            Destroy(gameObject);
        } else {
            overlappedObjects.Add(injurableBody);
        }
    }
    private void OnTriggerExit(Collider other) {

        if (ShouldIgnoreCollider(other, out SpaceRigidbody injurableBody)) return;
        overlappedObjects.Remove(injurableBody);
    }
    bool ShouldIgnoreCollider(Collider c, out SpaceRigidbody otherBody) {
        otherBody = null;
        if (!shouldAffectOtherProjectiles && c.tag == tag) return true;
        otherBody = c.GetComponent<SpaceRigidbody>();

        // if this is gravity, affect the other particle
        // even if the other particle is friendly?

        if (affect == Affect.Gravity) return false;
        if (!otherBody) return true;
        return otherBody.IsFriendly(this.body.allegiance);
    }
}
