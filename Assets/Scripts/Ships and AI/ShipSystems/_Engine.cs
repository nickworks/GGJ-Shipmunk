using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Engine : _ShipSystem {

    public float strength = 1;

    public void DoTick() {
        
        if (ship && ship.controller.wantsToMove) {
            ship.AddForce(transform.forward * strength * Time.deltaTime);
            Quaternion targetRot = Quaternion.LookRotation(ship.controller.dirToMove, Vector3.up);
            transform.rotation = AnimMath.Slide(transform.rotation, targetRot, 0.05f);
        }
    }
}
