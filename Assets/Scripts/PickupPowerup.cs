using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class PickupPowerup : MonoBehaviour {

    _Ability sys;
    SpriteRenderer sprite;

    private void Start() {
        sys = SpawnThings.PickRandom(SpawnThings.main.prefabAbilities);
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.sprite = sys.sprite;
    }

    void OnTriggerEnter(Collider other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player) {
            // add ability ...
            if(sys) player.ship.SpawnAndInstall(sys);

            Destroy(gameObject);
        }
    }
}
