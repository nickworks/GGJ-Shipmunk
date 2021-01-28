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

    public float angleRotate = 0;
    private float angleOffset = 0;

    override public void DoAbility() {
        angleOffset += angleRotate * Time.deltaTime;

        if (delayTimer > 0) return;

        delayTimer = 1 / bulletsPerSecond;


        float yawAim = transform.eulerAngles.y;
        yawAim -= (splitAmount / 2) * splitAngle;
        yawAim += angleOffset;

        for (int i = 0; i < splitAmount; i++) {

            float radsAngle = Mathf.Deg2Rad * (yawAim + i * splitAngle);
            Vector3 dir = new Vector3(Mathf.Sin(radsAngle), 0, Mathf.Cos(radsAngle));
            Vector3 off = Vector3.Cross(dir, Vector3.up);
            off *= Random.Range(-randomWidth, randomWidth);

            Projectile p = Instantiate(projectilePrefab, transform.position + off, Quaternion.LookRotation(dir, Vector3.up));
            p.InitBullet(ship.controller.allegiance);
        }
        
    }
}
