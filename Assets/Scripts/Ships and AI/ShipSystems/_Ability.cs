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
    private Quaternion targetRot;

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
    public float cooldownPercent {
        get {
            return cooldownUntilReload / timeToReload;
        }
    }
    virtual protected void Start() {
        shotsLeftUntilReload = maxUsesPerReload;
    }
    /*
    class States {
        class _State {
            public _State(){}
            public virtual void DoTick(bool wantsToUse){

            }
            public virtual Spaceship.States._State DoTickActive(bool wantsToUse){
                return null;
            }
            public virtual void Aim(){

            }
        }
        class Reloading : _State {

        }
        class 
    }
    /**/
    // run any cool-downs, and aim the weapon
    public void DoTick(bool wantsToUse) {
        if (cooldownUntilNextShot > 0) cooldownUntilNextShot -= Time.deltaTime;
        if (!wantsToUse && cooldownUntilReload > 0) {
            cooldownUntilReload -= Time.deltaTime;
            if(cooldownUntilReload <= 0) {
                shotsLeftUntilReload = maxUsesPerReload;
            }
        }
        // if the player has released the trigger for this weapon
        // and the weapon does not fire on release, reset weapon charge
        if(!wantsToUse && timerForChargeUp > 0 && !abilityFiresOnRelease) timerForChargeUp = 0;
        Aim();
    }
    // run the ability
    public Spaceship.States._State DoTickActive(bool wantsToUse) {

        // the current system accounts for many different kinds of abilities
        // but it does not create a very reliable state machine

        if(abilityChargesUp && timerForChargeUp < timeToCharge) timerForChargeUp += Time.deltaTime;

        // the player wants to use ability, so the ability should fire if:
        // - if there is ammo in the weapon
        // - OR if the weapon has finished reloading
        // - OR if there is no need to reload (infinite ammo)
        bool shoot = (shotsLeftUntilReload > 0 || cooldownUntilReload <= 0 || maxUsesPerReload == 0);

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
            if (maxUsesPerReload > 0) {
                if (shotsLeftUntilReload <= 0) return null; // no ammo
                if (--shotsLeftUntilReload <= 0) {
                    cooldownUntilReload = timeToReload;
                }
            }
            cooldownUntilNextShot = 1 / maxUsesPerSecond;
            float s = (chargeScalesPotency) ? chargedUpPercent : 1;
            // reset charge amount when firing ability:
            timerForChargeUp = 0;
            return DoAbility(s);
        }
        return null;
    }
    // rotates the ability to align with the aim direction
    private void Aim() {
        if(GetAbilityDir(out Vector3 dir)) {
            targetRot = Quaternion.LookRotation(dir, Vector3.up);
        }
        transform.rotation = AnimMath.Slide(transform.rotation, targetRot, .001f);
    }
    // if the aim direction can be found, 
    // output the direction otherwise, returns false
    public bool GetAbilityDir(out Vector3 dir) {
        dir = Vector3.zero;
        if (!ship || !ship.controller) return false;
        if (usesMoveDirInsteadOfAim) {
            dir = ship.controller.dirToMove;
            return (ship.controller.wantsToMove);
        } else {
            dir = ship.controller.dirToAim;
            return (ship.controller.wantsToAim);
        }
    }
    // when the ability is activated, this method is called.
    // override in child classes.
    virtual protected Spaceship.States._State DoAbility(float mult = 1) {
        return null;
    }
}
