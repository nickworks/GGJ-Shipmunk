using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : _Weapon
{
    public Projectile projectilePrefab;
    
    override public void DoAttack() {
        Projectile p = Instantiate(projectilePrefab, transform.position, transform.rotation);
        p.InitBullet(ship.controller.allegiance, 30);
    }
}
