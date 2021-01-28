using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    public static List<Spaceship> ships = new List<Spaceship>();
    public static class States {
        public class _State {
            protected Spaceship ship;
            virtual public _State Update() { return null; }
            virtual public void OnStart(Spaceship ship) { this.ship = ship; }
            virtual public void OnEnd() { }
        }
        public class Moving : _State {
            public override _State Update() {

                ship.DoUpdateSubSystems();
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

    [HideInInspector]
    public List<_ShipSystem> installedSubSystems = new List<_ShipSystem>();
    public _ShipSystem[] prefabsToTest;

    void Start() {
        ships.Add(this);

        controller = GetComponent<Controller>();

        // install _ShipSystems already on the prefab:
        installedSubSystems.AddRange(GetComponentsInChildren<_ShipSystem>());

        // test install:
        foreach(_ShipSystem prefab in prefabsToTest)
            if(prefab) Install(prefab);

    }
    void OnDestroy() {
        ships.Remove(this);
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
    public void Install(_ShipSystem prefab) {
        if (prefab == null) return;
        _ShipSystem sys = Instantiate(prefab, transform);

        installedSubSystems.Add(sys);
    }
    public void Uninstall(_ShipSystem sys) {

    }
    private void DoPhysTick() {
        transform.localPosition += velocity * Time.deltaTime;
    }
    private void DoSlowDown(float amountLeftAfterSecond = .05f) {
        velocity = AnimMath.Slide(velocity, Vector3.zero, amountLeftAfterSecond, Time.deltaTime);
    }
    private void DoUpdateSubSystems() {
        foreach (_ShipSystem sys in installedSubSystems)
            sys.DoTick();
    }

}
