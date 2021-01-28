using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Engine : _ShipSystem {

    public float strength = 1;

    override public void DoTick() {
        if (ship.controller.wantsToMove)
            ship.AddForce(ship.controller.dirToMove * strength * Time.deltaTime);
    }
}
