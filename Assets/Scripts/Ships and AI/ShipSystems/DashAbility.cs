using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : _Ability {

    public float distance = 10;
    public float time = 0.25f;
    public float chargeMinDistanceMult = 1;
    public float chargeMaxDistanceMult = 2;

    protected override void DoAbility(float mult = 1) {
        mult = mult * AnimMath.Lerp(chargeMinDistanceMult, chargeMaxDistanceMult, mult);
        GetAbilityDir(out Vector3 dir);

        Vector3 dest = ship.transform.localPosition + dir * distance;
        ship.ChangeState(new Spaceship.States.Dashing(ship.transform.localPosition, dest, time));
    }
}
