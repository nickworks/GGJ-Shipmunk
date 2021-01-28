using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    public static class States {
        public class _State {
            protected Spaceship ship;
            virtual public _State Update() { return null; }
            virtual public void OnStart(Spaceship ship) { this.ship = ship; }
            virtual public void OnEnd() { }
        }
        public class Moving : _State {
            public override _State Update() {

                ship.engine.DoTick();
                ship.weapon.DoTick();
                ship.DoPhysTick();
                ship.DoSlowDown();

                return null;
            }
        }
        public class Dashing : _State {

        }
    }

    public float speed = 5;
    private Vector3 velocity;

    public Controller controller { get; private set; }
    public States._State state { get; private set; }
    public _Engine engine { get; private set; }
    public _Weapon weapon { get; private set; }
    public Transform weaponArt;


    void Start() {
        controller = GetComponent<Controller>();
        engine = GetComponentInChildren<_Engine>();
        weapon = GetComponentInChildren<_Weapon>();
    }
    
    void Update() {

        if (state == null) ChangeState(new States.Moving());
        ChangeState(state.Update());

    }
    private void ChangeState(States._State next) {
        if (next == null) return;
        if (state != null) state.OnEnd();
        state = next;
        state.OnStart(this);
    }

    public void AddForce(Vector3 force) {
        force.y = 0;
        velocity += force;
    }
    void DoPhysTick() {
        transform.position += velocity * Time.deltaTime;
    }
    void DoSlowDown(float amountLeftAfterSecond = .05f) {
        velocity = AnimMath.Slide(velocity, Vector3.zero, amountLeftAfterSecond, Time.deltaTime);
    }

}
