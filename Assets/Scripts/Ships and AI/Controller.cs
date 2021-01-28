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
    protected Spaceship ship;

    [HideInInspector] public bool wantsToMove;
    [HideInInspector] public Vector3 dirToMove;
    [HideInInspector] public bool wantsToAim;
    [HideInInspector] public bool wantsToAttack;
    [HideInInspector] public Vector3 dirToAim;

    void Start() {
        Init();
    }
    protected virtual void Init() {
        ship = GetComponent<Spaceship>();
    }

}
