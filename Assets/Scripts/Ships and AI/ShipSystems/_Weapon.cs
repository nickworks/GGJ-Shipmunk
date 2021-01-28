using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Weapon : _ShipSystem {

    override public void DoTick() {
        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
        if (ship.controller.wantsToAction1) DoAttack();
    }
    virtual public void DoAttack() {

    }
}
