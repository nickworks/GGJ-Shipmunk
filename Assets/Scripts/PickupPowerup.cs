using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class PickupPowerup : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player) {
            // add ability ...

            _Ability sys = SpawnThings.PickRandom(SpawnThings.main.prefabAbilities);

            player.ship.SpawnAndInstall(sys);

            Destroy(gameObject);
        }
    }
}
