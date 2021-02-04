using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public enum Allegiance {
        Neutral,
        Player,
        Friendly,
        Enemy
    }

    public Allegiance allegiance { get; protected set; }
    public Spaceship ship { get; protected set; }

    [HideInInspector] public bool wantsToMove;
    [HideInInspector] public Vector3 dirToMove;
    [HideInInspector] public bool wantsToAim;
    [HideInInspector] public Vector3 dirToAim;
    [HideInInspector] public bool wantsToAbilityA;
    [HideInInspector] public bool wantsToAbilityB;
    [HideInInspector] public bool wantsToAbilityC;
    [HideInInspector] public bool wantsToAbilityD;

    public bool wantsToUseAbility {
        get {
            return wantsToAbilityA || wantsToAbilityB || wantsToAbilityC || wantsToAbilityD;
        }
    }

    void Awake() {
        ship = GetComponent<Spaceship>();
    }
}
