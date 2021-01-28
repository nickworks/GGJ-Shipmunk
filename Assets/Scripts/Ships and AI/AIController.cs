using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller {

    public static class States {
        public class _State {
            protected AIController controller;
            virtual public _State Update() {
                return null;
            }
            virtual public void OnStart(AIController controller) {
                this.controller = controller;
            }
            virtual public void OnEnd() { }
        }
        public class Idle : _State {
            public override _State Update() {
                controller.ScanForAttackTarget();
                if (controller.attackTarget) return new Aggro();
                return null;
            }
        }
        public class Aggro : _State {
            public override _State Update() {

                if (!controller.attackTarget) return new Idle();

                Vector3 dir = (controller.attackTarget.position - controller.ship.transform.position);

                controller.dirToAim = dir.normalized;
                controller.wantsToAim = true;
                controller.wantsToAbilityA = true;

                return null;
            }
            public override void OnEnd() {
                controller.wantsToAim = false;
                controller.wantsToAbilityA = false;
            }
        }
        public class Cooldown : _State {

        }
    }

    Transform attackTarget;
    States._State state;

    float delayUntilNextScan = 0;

    void Update() {
        if (state == null)
            ChangeState(new States.Idle());
        else
            ChangeState(state.Update());
    }
    void ChangeState(States._State nextState) {
        if (nextState == null) return;
        if (state != null) state.OnEnd();
        state = nextState;
        state.OnStart(this);
    }
    void ScanForAttackTarget() {
        if (attackTarget) return;
        delayUntilNextScan -= Time.deltaTime;
        if (delayUntilNextScan > 0) return;
        delayUntilNextScan = Random.Range(.75f, 1.25f);

        float minDis = 0;
        foreach (Spaceship s in Spaceship.ships) {
            if (s.controller.allegiance == allegiance) continue;

            float dis = (s.transform.position - ship.transform.position).sqrMagnitude;

            if (attackTarget == null || dis < minDis) {
                attackTarget = s.transform;
                minDis = dis;
            }
        }
    }
}
