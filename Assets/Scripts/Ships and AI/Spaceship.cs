using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    [HideInInspector]
    public _ShipSystem[] prefabsToTest;

    private _Engine engine;
    private List<_Passive> passiveSystems = new List<_Passive>();
    private Dictionary<AbilitySlots, _Ability> abilitySystems = new Dictionary<AbilitySlots, _Ability>();


    void Start() {
        ships.Add(this);

        controller = GetComponent<Controller>();

        // install _ShipSystems already on the prefab:
        _ShipSystem[] children = GetComponentsInChildren<_ShipSystem>();
        foreach (_ShipSystem sys in children) {
            Install(sys);
        }

        // test install from prefab definitions:
        foreach (_ShipSystem prefab in prefabsToTest)
            if(prefab) SpawnAndInstall(prefab);

    }
    public bool Install(_ShipSystem sys) {
        
        if (sys is _Engine) return Install((_Engine)sys);
        if (sys is _Passive) return Install((_Passive)sys);
        if (sys is _Ability) return Install((_Ability)sys);

        return false;
    }
    public bool Install(_Passive sys) {
        passiveSystems.Add(sys);
        return true;
    }
    public bool Install(_Ability sys) {
        AbilitySlots? slot = NextAbilitySlot();
        if (slot != null) {
            abilitySystems.Add((AbilitySlots)slot, sys);
            return true;
        }
        return false;
    }
    public bool Install(_Engine sys) {
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
    public bool SpawnAndInstall(_ShipSystem prefab) {
        if (prefab == null) return false;

        _ShipSystem sys = Instantiate(prefab, transform); // spawn it

        if (Install(sys)) return true; // try to install it

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
    private void DoSlowDown(float amountLeftAfterSecond = .05f) {
        velocity = AnimMath.Slide(velocity, Vector3.zero, amountLeftAfterSecond, Time.deltaTime);
    }
    private void DoUpdateSubSystems() {

        if (engine) engine.DoTick();

        foreach (KeyValuePair<AbilitySlots, _Ability> sys in abilitySystems)
            sys.Value.DoTick(sys.Key);
        
        foreach (_Passive sys in passiveSystems)
            sys.DoTick();
        
    }

}
