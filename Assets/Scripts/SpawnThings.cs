using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour {

    public Transform prefabAsteroid;

    ScrollerController scroller;
    float spawnTimer = 1;

    void Start() {
        scroller = GetComponent<ScrollerController>();
    }

    void Update() {

        if (spawnTimer > 0) spawnTimer -= Time.deltaTime;
        else {
            spawnTimer = Random.Range(0.1f, 3f);

            Vector3 pos = Vector3.zero;

            int n = Random.Range(1, 100);
            if (n < 66) pos = SpawnFromFront();
            else pos = SpawnFromRandom();
            
            Transform asteroid = Instantiate(prefabAsteroid, pos, Quaternion.identity, transform);
            float impulse = Random.Range(5, 10);
            asteroid.GetComponent<Rigidbody>().AddForce(-pos.normalized * impulse, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// Returns a spawn point in local coordinates
    /// </summary>
    /// <param name="randomDegrees"></param>
    /// <returns></returns>
    Vector3 SpawnFromFront(float randomDegrees = 30) {

        Vector3 min = scroller.min;
        Vector3 max = scroller.max;

        Vector3 vel = scroller.scrollVelocity;

        float angle = Mathf.Atan2(vel.z, vel.x);
        angle += Random.Range(-randomDegrees, randomDegrees) * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        dir = -dir.normalized;

        dir.x *= (max.x - min.x);
        dir.z *= (max.z - min.z);

        return dir;
    }
    Vector3 SpawnFromRandom() {

        Vector3 min = scroller.min;
        Vector3 max = scroller.max;

        Vector2 dir2d = Random.insideUnitCircle.normalized;

        Vector3 dir = Vector3.zero;

        dir.x = dir2d.x * (max.x - min.x);
        dir.z = dir2d.y * (max.z - min.z);

        return dir;
    }
}
