using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : _Ability {

    public float distance = 10;
    public float time = 0.25f;

    public override void DoAbility(float mult = 1) {
        ship.ChangeState(new Spaceship.States.Dashing(distance, time));
    }
}
