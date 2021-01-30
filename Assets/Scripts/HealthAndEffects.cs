using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class HealthAndEffects : MonoBehaviour {
    
    static public class Condition {
        public class _Condition {
            public int strength { get; private set; }
            public float timeLeft { get; private set; }
            internal _Condition(int s = 1, float t = 0.5f) {
                strength = s;
                timeLeft = t;
            }
            virtual public void Update(HealthAndEffects dnc) {
                timeLeft -= Time.unscaledDeltaTime;
            }
        }
        public class Slow : _Condition {
            public Slow(int s = 1, float t = 0.5f) : base(s, t) {}
            public override void Update(HealthAndEffects dnc) {
                base.Update(dnc);
                dnc.valueTimeScale -= .01f * strength;
            }
        }
        public class Poison : _Condition {
            public Poison(int s = 1, float t = 0.5f) : base(s, t) { }
            public override void Update(HealthAndEffects hp) {
                base.Update(hp);
                hp.valuePoisonDPS += strength * 10 ;
            }
        }
    }

    public float health = 100;

    private Controller.Allegiance _allegianceCache = Controller.Allegiance.Neutral;
    public Controller.Allegiance allegiance {
        get {
            return (controller) ? controller.allegiance : _allegianceCache;
        }
        set {
            _allegianceCache = (!controller) ? value : controller.allegiance;
        }
    }
    private Controller controller;
    private List<Condition._Condition> activeConditions = new List<Condition._Condition>();

    public float valueTimeScale { get; private set; }
    public float valuePoisonDPS { get; private set; }

    public SpaceRigidbody body { get; private set; }


    private void Start() {
        body = GetComponent<SpaceRigidbody>();
        controller = GetComponent<Controller>();
    }
    void Update() {

        // set blackboard variables:
        valueTimeScale = 1;
        valuePoisonDPS = 0;

        // allow conditions to write into bb:
        for (int i = activeConditions.Count - 1; i >= 0; i--) {
            activeConditions[i].Update(this);
            if (activeConditions[i].timeLeft < 0) activeConditions.RemoveAt(i);
        }

        // do stuff:
        if (valuePoisonDPS > 0) TakeDamage(valuePoisonDPS * Time.deltaTime);
        body.timeScale = Mathf.Clamp(valueTimeScale, .01f, 1);

    }
    public bool IsFriendly(Controller.Allegiance other) {
        Controller.Allegiance myAllegiance = (controller ? controller.allegiance : allegiance);
        return (other == myAllegiance);
    }

    public void TakeDamage(float amt) {
        if (amt <= 0) return;
        health -= amt;

        if (health <= 0) Destroy(gameObject);
    }
    public void AddCondition(Condition._Condition c) {
        activeConditions.Add(c);
    }
    
}
