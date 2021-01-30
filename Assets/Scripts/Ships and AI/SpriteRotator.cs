using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spaceship))]
public class SpriteRotator : MonoBehaviour {

    [System.Serializable]
    public class SpriteSet {
        public Sprite north;
        public Sprite northEast;
        public Sprite east;
        public Sprite southEast;
        public Sprite south;
    }

    public SpriteSet normal;

    private Spaceship ship;
    public SpriteRenderer sprite;
    
    void Start() {
        ship = GetComponent<Spaceship>();
    }
    
    void Update() {
        if (!ship || !ship.controller) return;

        Vector3 dir = ship.controller.wantsToAim ? ship.controller.dirToAim : ship.controller.dirToMove;
        float a = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        SpriteSet set = normal;
        Sprite s = set.southEast;
        bool flip = false;

        if(a > 0) {
            if (a < 22) s = set.east;
            else if (a < 67) s = set.northEast;
            else if (a < 112) s = set.north;
            else if (a < 157) {
                s = set.northEast;
                flip = true;
            } else {
                s = set.east;
                flip = true;
            }
        } else {
            if (a > -22) s = set.east;
            else if (a > -67) s = set.southEast;
            else if (a > -112) s = set.south;
            else if (a > -157) {
                s = set.southEast;
                flip = true;
            } else {
                s = set.east;
                flip = true;
            }
        }

        sprite.sprite = s;
        sprite.flipX = flip;
    }
}
