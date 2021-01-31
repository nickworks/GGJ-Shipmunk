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

            float aggro = 0;
            bool wantToA, wantToB, wantToC, wantToD;
            public Aggro(float time = 0) {

                aggro = (time > 0) ? time : Random.Range(0.2f, 1);

                int n = Random.Range(0, 4);

                wantToA = (n == 0);
                wantToB = (n == 1);
                wantToC = (n == 2);
                wantToD = (n == 3);

            }

            public override _State Update() {

                if (!controller.attackTarget) return new Idle();

                Vector3 dif = (controller.attackTarget.position - controller.ship.transform.position);
                float dis = dif.magnitude;
                Vector3 dir = dif / dis;

                controller.dirToAim = dir;
                controller.wantsToAim = true;
                controller.wantsToAbilityA = wantToA;
                controller.wantsToAbilityB = wantToA;
                controller.wantsToAbilityC = wantToA;
                controller.wantsToAbilityD = wantToA;

                controller.dirToMove = dir;
                controller.wantsToMove = true;

                aggro -= Time.deltaTime * controller.ship.body.timeScale;
                if (aggro <= 0) return new Idle();

                return null;
            }
            public override void OnEnd() {
                controller.wantsToAim = false;
                controller.wantsToAbilityA = false;
                controller.wantsToAbilityB = false;
                controller.wantsToAbilityC = false;
                controller.wantsToAbilityD = false;
            }
        }
        public class Cooldown : _State {

        }
    }

    Transform attackTarget;
    States._State state;

    float delayUntilNextScan = 0;

    private void Start() {
        ship.GiveRandomLoadout();
    }
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
