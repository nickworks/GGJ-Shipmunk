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

    public bool wantsToMove;
    public Vector3 dirToMove;

    public bool wantsToAim;
    public bool wantsToAttack;
    public Vector3 dirToAttack;

    void Start() {
        Init();
    }
    protected virtual void Init() {
        ship = GetComponent<Spaceship>();
    }

}
