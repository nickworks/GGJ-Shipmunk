using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : _Ability
{
    public Projectile projectilePrefab;

    public int splitAmount = 1;
    public float splitAngle = 0;

    public float bulletsPerSecond = 10;

    public float randomWidth = 0.5f;
    public float randomAngle = 0;

    override public void DoAbility() {
        if (delayTimer > 0) return;

        delayTimer = 1 / bulletsPerSecond;

        float yawAim = transform.eulerAngles.y;

        float offsetAngle = (splitAmount / 2) * splitAngle;

        for (int i = 0; i < splitAmount; i++) {

            float radsAngle = Mathf.Deg2Rad * (yawAim - offsetAngle + i * splitAngle);
            Vector3 dir = new Vector3(Mathf.Sin(radsAngle), 0, Mathf.Cos(radsAngle));
            Vector3 off = Vector3.Cross(dir, Vector3.up);
            off *= Random.Range(-randomWidth, randomWidth);

            Projectile p = Instantiate(projectilePrefab, transform.position + off, Quaternion.LookRotation(dir, Vector3.up));
            p.InitBullet(ship.controller.allegiance);
        }
        
    }
}
