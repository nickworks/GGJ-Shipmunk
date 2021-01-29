using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ability : _ShipSystem {

    public Sprite sprite;
    public string abilityName = "NAME ME";

    public bool abilityIsAutoFire = false;
    public float autoUsesPerSecond = 10;
    protected float timerForAutofire = 0;

    public bool abilityFiresOnRelease = false;
    public bool abilityChargesUp = false;
    public float timeToCharge = 1;
    protected float timerForChargeUp = 0;
    public bool chargeScalesPotency = false;
    public float chargeMaxPotencyMultiplier = 2;
    protected bool hasLetOff = false;

    public float chargedUpPercent {
        get {
            return (timerForChargeUp > timeToCharge) ? 1 : timerForChargeUp / timeToCharge;
        }
    }

    public void DoTick(Spaceship.AbilitySlots currentSlot) {
        if (timerForAutofire > 0) timerForAutofire -= Time.deltaTime;
        
        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
        
        bool doIt =(
            (ship.controller.wantsToAbilityA && currentSlot == Spaceship.AbilitySlots.ActionA) ||
            (ship.controller.wantsToAbilityB && currentSlot == Spaceship.AbilitySlots.ActionB) ||
            (ship.controller.wantsToAbilityC && currentSlot == Spaceship.AbilitySlots.ActionC) ||
            (ship.controller.wantsToAbilityD && currentSlot == Spaceship.AbilitySlots.ActionD)
        );

        if (doIt) {
            timerForChargeUp += Time.deltaTime;
            bool shoot = hasLetOff || (abilityIsAutoFire && timerForAutofire <= 0);

            if (abilityFiresOnRelease) shoot = false;
            if (abilityChargesUp && timerForChargeUp < timeToCharge) {
                bool fireAnyway = (chargeScalesPotency && abilityIsAutoFire);
                if (!fireAnyway) { 
                    shoot = false;
                    timerForAutofire = 0;
                }
            }
            if (shoot) { 
                timerForAutofire = 1 / autoUsesPerSecond;
                Do();
            }
            
            hasLetOff = false;
        } else if (!hasLetOff) { 
            if (abilityFiresOnRelease) Do();
            hasLetOff = true;
            timerForChargeUp = 0;
            timerForAutofire = 0;
        }
    }
    private void Do() {
        if (chargeScalesPotency) {
            float s = AnimMath.Lerp(1, chargeMaxPotencyMultiplier, chargedUpPercent);
            DoAbility(s);
        } else {
            DoAbility(1);
        }
    }
    virtual public void DoAbility(float mult = 1) {

    }
}
