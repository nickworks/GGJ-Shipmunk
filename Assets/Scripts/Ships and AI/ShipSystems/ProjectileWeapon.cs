using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : _Ability {

    public float kickbackImpulse = .1f;
    public Projectile projectilePrefab;

    public int splitAmount = 1;
    public float splitAngle = 0;

    public float randomWidth = 0.25f;
    public float randomAngle = 0;

    public float angleRotate = 0;
    private float angleOffset = 0;

    public float potencyScaleDamage = 1;
    public float potencyScaleSpeed = 1;
    public float potencyScaleSize = 1;

    void Update() {
        angleOffset += angleRotate * Time.deltaTime;
    }

    override public void DoAbility(float mult = 1) {

        float yawAim = transform.eulerAngles.y + angleOffset;

        // kick-back:
        ship.AddForce(yawToDir(yawAim) * -kickbackImpulse);

        // offset for spread-shot:
        yawAim -= (splitAmount / 2) * splitAngle;

        // spawn 1 projectile per split:
        for (int i = 0; i < splitAmount; i++) {

            Vector3 dir = yawToDir(yawAim + i * splitAngle);
            Vector3 offRight = Vector3.Cross(dir, Vector3.up) * Random.Range(-randomWidth, randomWidth);

            Projectile p = Instantiate(projectilePrefab, transform.position + offRight, Quaternion.LookRotation(dir, Vector3.up));
            p.InitBullet(ship.controller.allegiance, mult * potencyScaleDamage, mult * potencyScaleSpeed, mult * potencyScaleSize);
        }
    }
    Vector3 yawToDir(float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
    }
}
