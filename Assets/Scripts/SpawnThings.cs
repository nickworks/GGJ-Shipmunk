using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour {

    public Asteroid[] prefabAsteroids;
    public PickupPowerup[] prefabPowerups;
    public AIController[] prefabEnemies;

    public _Ability[] prefabAbilities;

    public ScrollerController scroller;
    float spawnTimer = 1;

    [Header("Background Things")]
    public Transform[] prefabBigThings;


    public static SpawnThings main { get; private set; }

    void Awake() {
        if (main && main.gameObject) Destroy(main.gameObject);
        main = this;
        SpawnBackgroundItems();
    }

    public static T PickRandom<T>(T[] prefabs) {
        if (prefabs.Length == 0) return default(T);
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    void Update() {

        if (spawnTimer > 0) spawnTimer -= Time.deltaTime;
        else {
            spawnTimer = Random.Range(0.1f, 3f);
            SpawnRandom();
        }
    }
    public void SpawnRandom() {
        MonoBehaviour prefab = null;

        int n = Random.Range(1, 100);

        if (n < 60) prefab = PickRandom(prefabAsteroids);
        else if (n < 90) prefab = PickRandom(prefabEnemies);
        else prefab = PickRandom(prefabPowerups);

        if(prefab) SpawnAThing(prefab);
    }
    private void SpawnAThing(MonoBehaviour prefab) {

        //print("spawning!");

        Vector3 pos = Vector3.zero;

        int n = Random.Range(1, 100);
        if (n < 66) pos = GetPositionInFront();
        else pos = GetPositionRandom();

        MonoBehaviour body = Instantiate(prefab, pos, Quaternion.identity, scroller.transform);

        Vector3 dir = -(pos - scroller.transform.position).normalized;
        float impulse = Random.Range(5, 10);

        body.GetComponent<Rigidbody>().AddForce(dir * impulse, ForceMode.Impulse);
    }

    /// <summary>
    /// Returns a spawn point in world coordinates
    /// </summary>
    /// <param name="randomDegrees"></param>
    /// <returns></returns>
    Vector3 GetPositionInFront(float randomDegrees = 30) {

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
    Vector3 GetPositionRandom() {

        Vector3 min = scroller.min;
        Vector3 max = scroller.max;

        Vector2 dir2d = Random.insideUnitCircle.normalized;

        Vector3 dir = Vector3.zero;

        dir.x = dir2d.x * (max.x - min.x);
        dir.z = dir2d.y * (max.z - min.z);

        return dir + scroller.transform.position;
    }
    public void SpawnBackgroundItems() {

        float ratio = Mathf.Tan(Mathf.PI / 3);

        for (int i = 0; i < 6; i++) {

            float y = Random.Range(-400, -1500);
            float z = -y / ratio;

            Vector3 pos = transform.position + new Vector3(i * 500, y, z);
            Instantiate(PickRandom(prefabBigThings), pos, Quaternion.LookRotation(Random.onUnitSphere, Vector3.up));
        }
    }
}
