using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float baseDamage = 10;
    public float baseSpeed = 1;
    public float baseLifeSpan = 4;
    float age = 0;
    public Controller.Allegiance allegiance;
    public bool destroyOnDoDamage = true;
    private float damageMult = 1;
    public void InitBullet(Controller.Allegiance allegiance, float damageMult = 1) {
        this.allegiance = allegiance;
        this.damageMult = damageMult;
    }
    
    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * baseSpeed * Time.deltaTime;
        age += Time.deltaTime;
        if (age > baseLifeSpan) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        CanBeDamaged injurableBody = other.GetComponent<CanBeDamaged>();
        if (injurableBody && injurableBody.ShouldTakeDamage(allegiance)) {
            injurableBody.TakeDamage(baseDamage * damageMult);
            if(destroyOnDoDamage) Destroy(gameObject);
        }
    }
}
