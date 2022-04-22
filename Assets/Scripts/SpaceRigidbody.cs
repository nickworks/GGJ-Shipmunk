using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceRigidbody : MonoBehaviour {

    #region TimeScale
    bool _timeScaleInit = false;
    [SerializeField]
    private float _timeScale = 1;
    public float timeScale {
        get { return _timeScale; }
        set {
            if (_timeScale == value) return;
            if (!body) return;
            if (_timeScaleInit) {
                body.mass *= timeScale;
                body.velocity /= timeScale;
                body.angularVelocity /= timeScale;
            }
            _timeScale = Mathf.Abs(value);
            body.mass /= timeScale;
            body.velocity *= timeScale;
            body.angularVelocity *= timeScale;
            _timeScaleInit = true;
        }
    }
    #endregion

    #region Health and Conditions
    public float health = 100;
    public float maxHealth = 100;
    public bool completelyIgnoreProjectiles = false;

    public float age { get; private set; }
    public float lifespan = 8;
    private List<Condition._Condition> activeConditions = new List<Condition._Condition>();
    static public class Condition {
        public class _Condition {
            public int strength { get; private set; }
            public float timeLeft { get; private set; }
            internal _Condition(int s = 1, float t = 0.5f) {
                strength = s;
                timeLeft = t;
            }
            virtual public void Update(SpaceRigidbody dnc) {
                timeLeft -= Time.unscaledDeltaTime;
            }
        }
        public class Slow : _Condition {
            public Slow(int s = 1, float t = 0.5f) : base(s, t) {}
            public override void Update(SpaceRigidbody dnc) {
                base.Update(dnc);
                dnc.valueTimeScale -= .01f * strength;
            }
        }
        public class Poison : _Condition {
            public Poison(int s = 1, float t = 0.5f) : base(s, t) { }
            public override void Update(SpaceRigidbody hp) {
                base.Update(hp);
                hp.valuePoisonDPS += strength * 10 ;
            }
        }
    }
    #endregion

    #region Access to Rigidbody, Controller, Allegiance
    private Rigidbody body;
    private Controller controller;
    private Controller.Allegiance _allegianceCache = Controller.Allegiance.Neutral;
    public Controller.Allegiance allegiance {
        get {
            return (controller) ? controller.allegiance : _allegianceCache;
        }
        set {
            _allegianceCache = (!controller) ? value : controller.allegiance;
        }
    }
    #endregion

    #region BB for Condition Calculations
    private float valueTimeScale = 1;
    private float valuePoisonDPS = 0;
    #endregion

    void Awake() {
        body = GetComponent<Rigidbody>();
        controller = GetComponent<Controller>();
        timeScale = _timeScale;
    }
    void Update() {
        CalculateAllConditions();

        age += timeScale * Time.deltaTime;
        if (lifespan > 0 && age > lifespan) Destroy(gameObject);

    }

    private void CalculateAllConditions() {
        // set blackboard variables:
        valueTimeScale = 1;
        valuePoisonDPS = 0;

        // allow conditions to write into bb:
        for (int i = activeConditions.Count - 1; i >= 0; i--) {
            activeConditions[i].Update(this);
            if (activeConditions[i].timeLeft < 0) activeConditions.RemoveAt(i);
        }

        // use bb values:
        if (valuePoisonDPS > 0) TakeDamage(valuePoisonDPS * Time.deltaTime);
        timeScale = Mathf.Clamp(valueTimeScale, .01f, 1);
    }

    public bool IsFriendly(Controller.Allegiance other) {
        Controller.Allegiance myAllegiance = (controller ? controller.allegiance : allegiance);
        return (other == myAllegiance);
    }
    public void Heal(float amt) {
        if (amt < 0) amt = 0;
        health += amt;
        if (health > maxHealth) health = maxHealth;
    }
    public void TakeDamage(float amt) {
        if (amt <= 0) return;
        health -= amt;
        
        if (health <= 0) {
            Die();
        }
    }
    public void Die() {
        SendMessage("OnDie");
        Destroy(gameObject);
    }
    public void OnDie() {
        // keep this 
    }
    public void AddCondition(Condition._Condition c) {
        activeConditions.Add(c);
    }
    #region Physics Rigidbody API
    public void AddForce(Vector3 v, ForceMode mode = ForceMode.Force) {
        v.y = 0;
        body.AddForce(v,mode);
    }
    public void ClampVelocity(float speed) {
        if (body.velocity.sqrMagnitude > speed * speed) {
            body.velocity = body.velocity.normalized * speed;
        }
    }
    public void DoSlowDown(float amountLeftAfterSecond = .05f) {
        body.velocity = AnimMath.Slide(body.velocity, Vector3.zero, amountLeftAfterSecond, Time.deltaTime);
    }
    public void SetVelocity(Vector3 v) {
        body.velocity = v;
    }
    public Vector3 GetVelocity() {
        return body.velocity;
    }
    public void SetAngularVelocity(Vector3 v) {
        body.angularVelocity = v;
    }
    #endregion
}
