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

    public Allegiance allegiance = Allegiance.Neutral;
    

    void Start() {
        
    }

}
