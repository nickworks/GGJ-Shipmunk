using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Weapon : _ShipSystem {

    protected float delayTimer = 0;
    public bool isAuto = false;
    protected bool hasLetOff = false;

    override public void DoTick() {
        if (delayTimer > 0) delayTimer -= Time.deltaTime;

        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
        if (ship.controller.wantsToAction1) {
            if (isAuto || hasLetOff) DoAttack();
            hasLetOff = false;
        } else hasLetOff = true;
    }
    virtual public void DoAttack() {

    }
}
