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

                return ship.DoUpdateSubSystems();
            }
        }
        public class Attacking : _State {
            _Ability activeAbility;
            public Attacking(_Ability ability) {
                activeAbility = ability;
            }
            public override _State Update() {

                if (!activeAbility || !ship) return new Moving();
                return ship.DoUpdateSubSystems(activeAbility);
            }
            public override void OnEnd() {
                activeAbility = null;
            }
        }
        public class Dashing : _State {

            float time = 0.5f;

            Vector3 start;
            Vector3 end;

            float animTimer = 0;

            public Dashing(Vector3 start, Vector3 end, float time) {
                this.time = time;
                this.end = end;
                this.start = start;
            }
            public override _State Update() {
                ship.DoUpdateSubSystems(null);

                animTimer += Time.deltaTime;
                ship.transform.localPosition = AnimMath.Lerp(start, end, animTimer / time, true);

                if (animTimer >= time) return new Moving();
                return null;
            }
            public override void OnEnd() {
                if (ship.controller.wantsToMove) {
                    ship.body.AddForce(ship.controller.dirToMove * 10);
                }
            }
        }
    }

    private Vector3 velocity;

    public Controller controller { get; private set; }
    public SpaceRigidbody body { get; private set; }
    public States._State state { get; private set; }

    private _Engine engine;
    private List<_Passive> passiveSystems = new List<_Passive>();
    public Dictionary<AbilitySlots, _Ability> abilitySystems { get; private set; }

    [Range(0,4)]public int startingAbilityNum = 1;

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
        //foreach (_ShipSystem prefab in prefabsToAddAtRuntime) if(prefab) SpawnAndInstall(prefab);
        GiveRandomLoadout(startingAbilityNum);
        UpdateHUD();
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
            UpdateHUD();
            return true; // try to install it
        }

        Destroy(sys.gameObject); // delete it if unsuccessful

        return false;
    }
    public void UpdateHUD() {
        if (controller is PlayerController) {
            (controller as PlayerController).RebuildHUD();
        }
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
    
    /*
    private void DoAbility(AbilitySlots slot) {
        if (!abilitySystems.ContainsKey(slot)) return;
        abilitySystems[slot].
    }*/
    private States._State DoUpdateSubSystems(_Ability activeAbility = null) {

        if (engine) engine.DoTick();

        foreach (_Passive sys in passiveSystems) sys.DoTick();
        
        bool ableToActivate = (activeAbility == null);
        States._State nextState = null;
        foreach (KeyValuePair<AbilitySlots, _Ability> sys in abilitySystems) {

            bool want = DoesControllerWantToUseMe(sys.Key);

            sys.Value.DoTick(want);

            if(activeAbility == null) {
                if (want) nextState = new States.Attacking(sys.Value);
            } else if (activeAbility == sys.Value) nextState = sys.Value.DoTickActive(want);
        }

        return nextState;        
    }
    public bool DoesControllerWantToUseMe(Spaceship.AbilitySlots currentSlot) {
        return (
            (controller.wantsToAbilityA && currentSlot == Spaceship.AbilitySlots.ActionA) ||
            (controller.wantsToAbilityB && currentSlot == Spaceship.AbilitySlots.ActionB) ||
            (controller.wantsToAbilityC && currentSlot == Spaceship.AbilitySlots.ActionC) ||
            (controller.wantsToAbilityD && currentSlot == Spaceship.AbilitySlots.ActionD)
        );
    }
    public void GiveRandomLoadout(int num) {
        if (num > 4) num = 4;
        for (int i = 0; i < num; i++) {
            SpawnAndInstall(SpawnThings.PickRandom(SpawnThings.main.prefabAbilities));
        }
    }
}
