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
    public void DoTick() {
        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;
        Aim();
        
        if (!hasLetOff) { 
            if (abilityFiresOnRelease) TryToDo();
            hasLetOff = true;
            timerForChargeUp = 0;
        }
    }
    public Spaceship.States._State DoTickActive() {
        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;
        Aim();

        timerForChargeUp += Time.deltaTime;
        bool shoot = (abilityIsAutoFire || hasLetOff);
        if (abilityFiresOnRelease) shoot = false;
        if (abilityChargesUp && timerForChargeUp < timeToCharge) {
            shoot = (chargeScalesPotency && abilityIsAutoFire);
        }

        hasLetOff = false;
        
        if (shoot) TryToDo();

        return null;
    }
    private void Aim() {
        if (ship.controller.wantsToAim) transform.rotation = Quaternion.LookRotation(ship.controller.dirToAim, Vector3.up);
    }
    
    public void TryToDo() {
        if (timerCooldown > 0) return; // cancel
        timerCooldown = 1 / maxUsesPerSecond;
        float s = (chargeScalesPotency) ? chargedUpPercent : 1;
        DoAbility(s);
    }
    /// <summary>
    /// And switch ship state if necessary...
    /// </summary>
    /// <param name="mult"></param>
    virtual protected void DoAbility(float mult = 1) {

    }
}
