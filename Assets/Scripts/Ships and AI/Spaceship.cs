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

                ship.DoDrive();
                ship.DoAim();
                ship.DoPhysTick();

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
    public Transform weaponArt;


    void Start() {
        controller = GetComponent<Controller>();
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
    void DoDrive() {
        if (controller.wantsToMove) AddForce(controller.dirToMove * speed * Time.deltaTime);
    }
    void DoAim() {
        if (controller.wantsToAim) weaponArt.rotation = Quaternion.LookRotation(controller.dirToAim, Vector3.up);
    }
    void DoPhysTick() {
        transform.position += velocity * Time.deltaTime;
    }

}
