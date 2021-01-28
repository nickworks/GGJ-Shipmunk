using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeDamaged : MonoBehaviour {
    public float hp = 100;
    public Controller.Allegiance allegiance = Controller.Allegiance.Neutral;
    private Controller controller;

    private void Start() {
        controller = GetComponent<Controller>();
    }

    public void TakeDamage(float amt) {
        if (amt <= 0) return;
        hp -= amt;

        if (hp <= 0) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        Projectile p = other.GetComponent<Projectile>();
        if (p) {
            Controller.Allegiance myAllegiance = (controller ? controller.allegiance : allegiance);
            if (p.allegiance == myAllegiance) return;
            TakeDamage(p.baseDamage);
            Destroy(p.gameObject);
        }
    }
}