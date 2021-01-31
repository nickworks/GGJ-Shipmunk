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

    public SpriteRenderer spriteRenderer;
    public SpriteSet normal;
    public SpriteSet hit;
    public SpriteSet attack;

    private Spaceship ship;
    
    void Start() {
        ship = GetComponent<Spaceship>();
        if(!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update() {
        if (!ship || !ship.controller) return;

        Vector3 dir = ship.controller.wantsToAim ? ship.controller.dirToAim : ship.controller.dirToMove;
        float a = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        SpriteSet set = normal;
        if (ship.state != null) {
            if (ship.state is Spaceship.States.Attacking) set = attack;
            if (ship.state is Spaceship.States.Dashing) set = hit;
        }
        
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

        spriteRenderer.sprite = s;
        spriteRenderer.flipX = flip;
    }
}
