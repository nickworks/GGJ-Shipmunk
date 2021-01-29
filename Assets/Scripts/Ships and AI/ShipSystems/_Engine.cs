using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Engine : _ShipSystem {

    public float strength = 1;
    public float maxSpeed = 10;

    public void DoTick() {

        if (!ship) return;
        if (ship.controller.wantsToMove) {
            
            ship.AddForce(transform.forward * strength * Time.deltaTime);
            Quaternion targetRot = Quaternion.LookRotation(ship.controller.dirToMove, Vector3.up);
            transform.rotation = AnimMath.Slide(transform.rotation, targetRot, 0.001f);

            ship.ClampVelocity(maxSpeed);

        } else {
            ship.DoSlowDown(0.05f);
        }
    }
}
