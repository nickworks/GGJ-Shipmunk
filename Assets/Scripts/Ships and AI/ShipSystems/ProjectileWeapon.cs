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
    
    public float chargeMinSize = 0;
    public float chargeMaxSize = 1;
    public float chargeMinSpeed = 0;
    public float chargeMaxSpeed = 1;
    public float chargeMinStrength = 0;
    public float chargeMaxStrength = 1;

    void Update() {
        angleOffset += angleRotate * Time.deltaTime * ship.body.timeScale;
    }

    override protected void DoAbility(float mult = 1) {

        float yawAim = transform.eulerAngles.y + angleOffset;

        // kick-back:
        ship.AddForce(yawToDir(yawAim) * -kickbackImpulse);

        // offset for spread-shot:
        yawAim -= (splitAmount / 2) * splitAngle;

        float dmg = AnimMath.Lerp(chargeMinStrength, chargeMaxStrength, mult);
        float spd = AnimMath.Lerp(chargeMinSpeed, chargeMaxSpeed, mult);
        float siz = AnimMath.Lerp(chargeMinSize, chargeMaxSize, mult);

        // spawn 1 projectile per split:
        for (int i = 0; i < splitAmount; i++) {

            float rand = Random.Range(-randomAngle, randomAngle);
            Vector3 dir = yawToDir(yawAim + i * splitAngle + rand);
            Vector3 offRight = Vector3.Cross(dir, Vector3.up) * Random.Range(-randomWidth, randomWidth);

            Projectile p = Instantiate(projectilePrefab, transform.position + offRight, Quaternion.LookRotation(dir, Vector3.up));
            p.InitBullet(ship.controller.allegiance, dmg, spd, siz);
        }
        ship.ChangeState(new Spaceship.States.Attacking());
    }
    Vector3 yawToDir(float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians));
    }
}
