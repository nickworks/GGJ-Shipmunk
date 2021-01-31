using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Ability : _ShipSystem {

    public Sprite sprite;
    public string abilityName = "NAME ME";

    [Header("Burst and Reload")]
    public int maxUsesPerReload = 3;
    public float maxUsesPerSecond = 10;
    public float timeToReload = 0.1f;
    protected float cooldownUntilNextShot = 0;
    protected float cooldownUntilReload = 0;
    protected int shotsLeftUntilReload = 0;

    [Header("Charge-Up")]
    public bool abilityFiresOnRelease = false;
    public bool abilityChargesUp = false;
    public float timeToCharge = 1;
    protected float timerForChargeUp = 0;
    public bool chargeScalesPotency = false;

    [Header("Aiming Ability")]
    public bool usesMoveDirInsteadOfAim = false;

    public float chargedUpPercent {
        get {
            return (timerForChargeUp > timeToCharge) ? 1 : timerForChargeUp / timeToCharge;
        }
    }
    public float ammoPercent {
        get {
            return shotsLeftUntilReload / (float)maxUsesPerReload;
        }
    }

    private void Start() {
        shotsLeftUntilReload = maxUsesPerReload;
    }

    public void DoTick(bool wantsToUse) {
        if (cooldownUntilNextShot > 0) cooldownUntilNextShot -= Time.deltaTime;
        if (!wantsToUse && cooldownUntilReload > 0) {
            cooldownUntilReload -= Time.deltaTime;
            if(cooldownUntilReload <= 0) {
                shotsLeftUntilReload = maxUsesPerReload;
            }
        }
        Aim();
    }
    public Spaceship.States._State DoTickActive(bool wantsToUse) {

        timerForChargeUp += Time.deltaTime;
        bool shoot = (cooldownUntilReload <= 0 || maxUsesPerReload == 0);

        if (wantsToUse) {
            if (abilityFiresOnRelease) {
                shoot = false;
            } else if (abilityChargesUp && timerForChargeUp < timeToCharge) {
                shoot = chargeScalesPotency;
            }
        } else {
            if (abilityFiresOnRelease) {
                shoot = true;
            } else {
                cooldownUntilReload = timeToReload; // start reload
                return new Spaceship.States.Moving();
            }
        }
        if (shoot) {
            if (cooldownUntilNextShot > 0) return null; // must wait...
            if(maxUsesPerReload > 0) {
                if (shotsLeftUntilReload <= 0) return null; // no ammo
                if (--shotsLeftUntilReload <= 0) {
                    cooldownUntilReload = timeToReload;
                }
            }
            cooldownUntilNextShot = 1 / maxUsesPerSecond;
            float s = (chargeScalesPotency) ? chargedUpPercent : 1;
            return DoAbility(s);
        }
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
