using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
[RequireComponent(typeof(HealthAndEffects))]
public class Projectile : MonoBehaviour {

    public enum Affect {
        DamageOnHit,
        SlowCondition,
        PoisonCondition,
        Gravity,
    }
    public Affect affect = Affect.DamageOnHit;
    public int affectAmount = 10;
    public bool shouldAffectOtherProjectiles = false;

    public float baseSpeed = 1;
    public float baseLifeSpan = 4;
    float age = 0;
    
    public bool destroyOnDoDamage = true;
    [Tooltip("When using destroy-on-hit, use large numbers (1+). Otherwise, use small numbers (.1 - .5).")]
    public float affectDuration = 1;
    protected float cooldownToNextHit = 0;

    private float damageMult = 1;
    private float speedMult = 1;

    public SpaceRigidbody body { get; private set; }
    public HealthAndEffects health { get; private set; }
    private List<HealthAndEffects> overlappedObjects = new List<HealthAndEffects>();

    public void InitBullet(Controller.Allegiance allegiance, float damageMult = 1, float speedMult = 1, float sizeMult = 1) {

        body = GetComponent<SpaceRigidbody>();
        health = GetComponent<HealthAndEffects>();

        body.SetVelocity(transform.forward * baseSpeed * speedMult);

        health.allegiance = allegiance;
        this.damageMult = damageMult;
        this.speedMult = speedMult;
        transform.localScale *= sizeMult;
    }
    void Update() {
        age += Time.deltaTime * Time.timeScale;
        if (age > baseLifeSpan) {
            Destroy(gameObject);
        }
        if (!destroyOnDoDamage) {
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
        print("hit " + overlappedObjects.Count);
        for (int i = overlappedObjects.Count - 1; i >= 0; i--) {
            HealthAndEffects body = overlappedObjects[i];
            if (body) HitSingle(body);
            else overlappedObjects.RemoveAt(i);
        }
    }
    private void HitSingle(HealthAndEffects injurableBody) {

        int dmg = (int)(affectAmount * damageMult);

        switch (affect) {
            case Affect.DamageOnHit:
                injurableBody.TakeDamage(dmg);
                break;
            case Affect.SlowCondition:
                print($"adding SLOW affect -- dmg: {dmg} duration: {affectDuration}");
                injurableBody.AddCondition(new HealthAndEffects.Condition.Slow(dmg, affectDuration));
                break;
            case Affect.PoisonCondition:
                injurableBody.AddCondition(new HealthAndEffects.Condition.Poison(dmg, affectDuration));
                break;
            case Affect.Gravity:
                if (!injurableBody.body) return;
                Vector3 dir = (transform.position - injurableBody.transform.position).normalized;
                injurableBody.body.AddForce(affectAmount * dir);
                break;
        }
    }
    private void OnTriggerEnter(Collider other) {

        if (ShouldIgnoreCollider(other, out HealthAndEffects injurableBody)) return;

        if (destroyOnDoDamage) {
            HitSingle(injurableBody);
            Destroy(gameObject);
        } else {
            overlappedObjects.Add(injurableBody);
        }
    }
    private void OnTriggerExit(Collider other) {

        if (ShouldIgnoreCollider(other, out HealthAndEffects injurableBody)) return;
        overlappedObjects.Remove(injurableBody);
    }
    bool ShouldIgnoreCollider(Collider c, out HealthAndEffects health) {
        health = null;
        if (!shouldAffectOtherProjectiles && c.tag == tag) return true;
        health = c.GetComponent<HealthAndEffects>();
        return (!health || health.IsFriendly(this.health.allegiance));
    }
    void OnTriggerStay(Collider other) {
        /*
        switch (affect) {
            case Affect.DamageOnHit:
            case Affect.SlowCondition:
            case Affect.PoisonCondition:
                break;
            case Affect.DamagePerSecond:
                HealthAndEffects injurableBody = other.GetComponent<HealthAndEffects>();
                if (!injurableBody || injurableBody.IsFriendly(health.allegiance)) return;
                injurableBody.TakeDamage(affectAmount * damageMult * Time.fixedDeltaTime);
                break;
            case Affect.Gravity:
                SpaceRigidbody body = other.GetComponent<SpaceRigidbody>();
                if (!body) return;
                Vector3 dir = (transform.position - other.transform.position).normalized;
                body.AddForce(affectAmount * dir);
                break;
        }
        */
    }
}
