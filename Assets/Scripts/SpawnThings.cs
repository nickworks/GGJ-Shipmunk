using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour {

    public Asteroid[] prefabAsteroids;

    public ScrollerController scroller;
    float spawnTimer = 1;

    void Update() {

        if (spawnTimer > 0) spawnTimer -= Time.deltaTime;
        else {
            spawnTimer = Random.Range(0.1f, 3f);
            SpawnAThing();
        }
    }

    private void SpawnAThing() {
        Vector3 pos = Vector3.zero;

        int n = Random.Range(1, 100);
        if (n < 66) pos = SpawnFromFront();
        else pos = SpawnFromRandom();

        Asteroid prefab = prefabAsteroids[Random.Range(0, prefabAsteroids.Length)];
        Asteroid asteroid = Instantiate(prefab, pos, Quaternion.identity, scroller.transform);

        Vector3 dir = -(pos - scroller.transform.position).normalized;
        float impulse = Random.Range(5, 10);
        asteroid.GetComponent<Rigidbody>().AddForce(dir * impulse, ForceMode.Impulse);
    }

    /// <summary>
    /// Returns a spawn point in world coordinates
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

        dir = dir.normalized;

        dir.x *= (max.x - min.x);
        dir.z *= (max.z - min.z);

        return dir + scroller.transform.position;
    }
    Vector3 SpawnFromRandom() {

        Vector3 min = scroller.min;
        Vector3 max = scroller.max;

        Vector2 dir2d = Random.insideUnitCircle.normalized;

        Vector3 dir = Vector3.zero;

        dir.x = dir2d.x * (max.x - min.x);
        dir.z = dir2d.y * (max.z - min.z);

        return dir + scroller.transform.position;
    }
}
