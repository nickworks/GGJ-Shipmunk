using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ability : _ShipSystem {

    public Sprite sprite;
    public string abilityName = "NAME ME";

    public bool abilityIsAutoFire = false;
    public float maxUsesPerSecond = 10;
    protected float timerCooldown = 0;

    public bool abilityFiresOnRelease = false;
    public bool abilityChargesUp = false;
    public float timeToCharge = 1;
    protected float timerForChargeUp = 0;
    public bool chargeScalesPotency = false;
    protected bool hasLetOff = true;

    public float chargedUpPercent {
        get {
            return (timerForChargeUp > timeToCharge) ? 1 : timerForChargeUp / timeToCharge;
        }
    }

    public void DoTick(Spaceship.AbilitySlots currentSlot) {
        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;
        
        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
        
        bool doIt =(
            (ship.controller.wantsToAbilityA && currentSlot == Spaceship.AbilitySlots.ActionA) ||
            (ship.controller.wantsToAbilityB && currentSlot == Spaceship.AbilitySlots.ActionB) ||
            (ship.controller.wantsToAbilityC && currentSlot == Spaceship.AbilitySlots.ActionC) ||
            (ship.controller.wantsToAbilityD && currentSlot == Spaceship.AbilitySlots.ActionD)
        );

        if (doIt) {
            timerForChargeUp += Time.deltaTime;
            bool shoot = (abilityIsAutoFire || hasLetOff);
            if (abilityFiresOnRelease) shoot = false;
            if (abilityChargesUp && timerForChargeUp < timeToCharge) {
                shoot = (chargeScalesPotency && abilityIsAutoFire);
            }
            if (shoot) Do();
            
            hasLetOff = false;
        } else if (!hasLetOff) { 
            if (abilityFiresOnRelease) Do();
            hasLetOff = true;
            timerForChargeUp = 0;
        }
    }
    private void Do() {

        if (timerCooldown > 0) return;
        timerCooldown = 1 / maxUsesPerSecond;

        float s = (chargeScalesPotency) ? chargedUpPercent : 1;
        DoAbility(s);
    }
    virtual public void DoAbility(float mult = 1) {

    }
}
