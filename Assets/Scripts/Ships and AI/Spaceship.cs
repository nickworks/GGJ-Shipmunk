using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class Spaceship : MonoBehaviour {

    public enum AbilitySlots {
        ActionA,
        ActionB,
        ActionC,
        ActionD
    }

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
                
                return null;
            }
        }
        public class Dashing : _State {
            
            float time = 0.5f;
            float distance = 0;

            Vector3 start;
            Vector3 end;

            float animTimer = 0;

            public Dashing(float distance, float time) {
                this.time = time;
                this.distance = distance;
            }
            public override _State Update() {
                ship.DoUpdateSubSystems();
                if(DoDashTick()) return new Moving();
                return null;
            }
            private bool DoDashTick() {
                animTimer += Time.deltaTime;
                ship.transform.localPosition = AnimMath.Lerp(start, end, animTimer / time, true);
                return (animTimer >= time);
            }
            public override void OnStart(Spaceship ship) {
                base.OnStart(ship);

                start = ship.transform.localPosition;
                end = start + ship.controller.dirToMove * distance;
                ship.velocity = .25f * ship.engine.transform.forward * distance / time;
            }
            public override void OnEnd() {
                if (ship.controller.wantsToMove) {
                    ship.AddForce(ship.controller.dirToMove * 10);
                }
            }
        }
    }

    public float speed = 5;
    private Vector3 velocity;

    public Controller controller { get; private set; }
    public SpaceRigidbody body { get; private set; }
    public States._State state { get; private set; }

    public _ShipSystem[] prefabsToAddAtRuntime;

    private _Engine engine;
    private List<_Passive> passiveSystems = new List<_Passive>();
    public Dictionary<AbilitySlots, _Ability> abilitySystems { get; private set; }

    private void Awake() {
        controller = GetComponent<Controller>();
        body = GetComponent<SpaceRigidbody>();
        abilitySystems = new Dictionary<AbilitySlots, _Ability>();
    }
    void Start() {
        ships.Add(this);

        // install _ShipSystems already on the prefab:
        _ShipSystem[] children = GetComponentsInChildren<_ShipSystem>();
        foreach (_ShipSystem sys in children) {
            Install(sys);
        }

        // test install from prefab definitions:
        foreach (_ShipSystem prefab in prefabsToAddAtRuntime)
            if(prefab) SpawnAndInstall(prefab);

    }
    private bool Install(_ShipSystem sys) {
        
        if (sys is _Engine) return Install((_Engine)sys);
        if (sys is _Passive) return Install((_Passive)sys);
        if (sys is _Ability) return Install((_Ability)sys);

        return false;
    }
    private bool Install(_Passive sys) {
        passiveSystems.Add(sys);
        return true;
    }
    private bool Install(_Ability sys) {
        AbilitySlots? slot = NextAbilitySlot();
        if (slot != null) {
            abilitySystems.Add((AbilitySlots)slot, sys);
            return true;
        }
        return false;
    }
    private bool Install(_Engine sys) {
        try {
            _Engine prev = engine;
            engine = (_Engine)sys;
            if (prev) Destroy(prev.gameObject);
            return true;
        } catch {
            print("ERROR");
            Destroy(sys.gameObject);
        }
        return true;
    }
 
    void OnDestroy() {
        ships.Remove(this);
    }

    void Update() {

        if (state == null) ChangeState(new States.Moving());
        ChangeState(state.Update());

    }
    public void ChangeState(States._State next) {
        if (next == null) return;
        if (state != null) state.OnEnd();
        state = next;
        state.OnStart(this);
    }

    public void AddForce(Vector3 force) {
        force.y = 0;
        velocity += force;
    }
    public void Clamp(Vector3 min, Vector3 max) {
        Vector3 pos = transform.position;

        if (pos.x < min.x) {
            pos.x = min.x;
            velocity.x = 0;
        }
        if (pos.x > max.x) {
            pos.x = max.x;
            velocity.x = 0;
        }
        if (pos.z < min.z) {
            pos.z = min.z;
            velocity.z = 0;
        }
        if (pos.z > max.z) {
            pos.z = max.z;
            velocity.z = 0;
        }
        transform.position = pos;
    }
    public bool SpawnAndInstall(_ShipSystem prefab) {
        if (prefab == null) return false;

        _ShipSystem sys = Instantiate(prefab, transform); // spawn it

        if (Install(sys)) {
            if(controller is PlayerController) {
                (controller as PlayerController).UpdateHUD();
            }
            return true; // try to install it
        }

        Destroy(sys.gameObject); // delete it if unsuccessful

        return false;
    }

    private AbilitySlots? NextAbilitySlot() {
        if (!abilitySystems.ContainsKey(AbilitySlots.ActionA)) return AbilitySlots.ActionA;
        if (!abilitySystems.ContainsKey(AbilitySlots.ActionB)) return AbilitySlots.ActionB;
        if (!abilitySystems.ContainsKey(AbilitySlots.ActionC)) return AbilitySlots.ActionC;
        if (!abilitySystems.ContainsKey(AbilitySlots.ActionD)) return AbilitySlots.ActionD;
        return null;
    }

    public void Uninstall(_ShipSystem sys) {

    }

    private void DoPhysTick() {
        transform.localPosition += velocity * Time.deltaTime;
    }
    public void ClampVelocity(float speed) {
        if(velocity.sqrMagnitude > speed * speed){
            velocity = velocity.normalized * speed;
        }
    }
    public void DoSlowDown(float amountLeftAfterSecond = .05f) {
        velocity = AnimMath.Slide(velocity, Vector3.zero, amountLeftAfterSecond, Time.deltaTime);
    }
    /*
    private void DoAbility(AbilitySlots slot) {
        if (!abilitySystems.ContainsKey(slot)) return;
        abilitySystems[slot].
    }*/
    private void DoUpdateSubSystems() {

        if (engine) engine.DoTick();

        foreach (KeyValuePair<AbilitySlots, _Ability> sys in abilitySystems)
            sys.Value.DoTick(sys.Key);
        
        foreach (_Passive sys in passiveSystems)
            sys.DoTick();
        
    }
}
