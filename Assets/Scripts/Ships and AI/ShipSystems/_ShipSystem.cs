using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ShipSystem : MonoBehaviour {

    public Spaceship ship { get; private set; }

    void Start() {
        ship = GetComponentInParent<Spaceship>();
    }
    virtual public void DoTick() {}
}