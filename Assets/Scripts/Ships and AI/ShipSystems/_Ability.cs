using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ability : _ShipSystem {

    public Sprite sprite;
    public string abilityName = "NAME ME";

    public bool abilityIsAutoFire = false;
    public float maxUsesPerSecond = 10;
    protected float timerCooldown = 0;
    
    public int burstSize = 0;
    protected int ammo = 0;

    public bool abilityFiresOnRelease = false;
    public bool abilityChargesUp = false;
    public float timeToCharge = 1;
    protected float timerForChargeUp = 0;
    public bool chargeScalesPotency = false;
    protected bool hasLetOff = true;

    public bool usesMoveDirInsteadOfAim = false;

    public float chargedUpPercent {
        get {
            return (timerForChargeUp > timeToCharge) ? 1 : timerForChargeUp / timeToCharge;
        }
    }
    public void DoTick() {
        if (timerCooldown > 0) timerCooldown -= Time.deltaTime;
        Aim();
    }
    public Spaceship.States._State DoTickActive(bool wantsToUse) {

        timerForChargeUp += Time.deltaTime;
        bool shoot = (abilityIsAutoFire || hasLetOff);

        if (wantsToUse) {
            if (abilityChargesUp && timerForChargeUp < timeToCharge) {
                shoot = (chargeScalesPotency && abilityIsAutoFire);
            }
            if (abilityFiresOnRelease) shoot = false;
        } else {
            if (abilityFiresOnRelease) shoot = true;
        }
        if (shoot) {
            if (timerCooldown > 0) return null; // cancel
            timerCooldown = 1 / maxUsesPerSecond;
            float s = (chargeScalesPotency) ? chargedUpPercent : 1;
            return DoAbility(s);
        }
        if (!wantsToUse) return new Spaceship.States.Moving();
        return null;
    }
    private void Aim() {

        if(GetAbilityDir(out Vector3 dir)) {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
    public bool GetAbilityDir(out Vector3 dir) {
        dir = Vector3.zero;
        if (usesMoveDirInsteadOfAim) {
            dir = ship.controller.dirToMove;
            return (ship.controller.wantsToMove);
        } else {
            dir = ship.controller.dirToAim;
            return (ship.controller.wantsToAim);
        }
    }
    /// <summary>
    /// And switch ship state if necessary...
    /// </summary>
    /// <param name="mult"></param>
    virtual protected Spaceship.States._State DoAbility(float mult = 1) {
        return null;
    }
}
