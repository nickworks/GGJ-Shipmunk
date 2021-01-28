using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ability : _ShipSystem {

    protected float delayTimer = 0;
    public bool isAuto = false;
    protected bool hasLetOff = false;

    public void DoTick(Spaceship.AbilitySlots currentSlot) {
        if (delayTimer > 0) delayTimer -= Time.deltaTime;

        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
        
        bool doIt =(
            (ship.controller.wantsToAbilityA && currentSlot == Spaceship.AbilitySlots.ActionA) ||
            (ship.controller.wantsToAbilityB && currentSlot == Spaceship.AbilitySlots.ActionB) ||
            (ship.controller.wantsToAbilityC && currentSlot == Spaceship.AbilitySlots.ActionC) ||
            (ship.controller.wantsToAbilityD && currentSlot == Spaceship.AbilitySlots.ActionD)
        );
        
        if (doIt) {
            if (isAuto || hasLetOff) DoAbility();
            hasLetOff = false;
        } else hasLetOff = true;
    }
    virtual public void DoAbility() {

    }
}
