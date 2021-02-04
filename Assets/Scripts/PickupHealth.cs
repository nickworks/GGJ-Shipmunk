using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class PickupHealth : MonoBehaviour {

    public float amount = 100;
    SpriteRenderer sprite;

    void OnTriggerEnter(Collider other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player) {
            // add ability ...
            player.GetComponent<SpaceRigidbody>().Heal(amount);

            Destroy(gameObject);
        }
    }
}
