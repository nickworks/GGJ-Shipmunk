using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public Transform art;

    public Texture texture;

    float lifespan = 10;
    float age = 0;
    float pitch = 60;
    Rigidbody body;
    
    void Start() {
        body = GetComponent<Rigidbody>();

        ApplyArt();

        // set rotational velocity:
        float quarterRange = 5;
        Vector3 vel = Vector3.zero;
        vel.y = Random.Range(-quarterRange, quarterRange) + Random.Range(-quarterRange, quarterRange);
        body.angularVelocity = vel;
        pitch = Random.Range(-30, -10);

        // set random scale:
        transform.localScale = Vector3.one * Random.Range(.8f, 2f);
    }

    private void OnValidate() {
        //ApplyArt();
    }
    void ApplyArt() {
        if (art && texture) {
            MeshRenderer mesh = art.GetComponent<MeshRenderer>();
            if (mesh) {
                List<Material> mats = new List<Material>();
                mesh.GetMaterials(mats);
                foreach (Material mat in mats) mat.SetTexture("_MainTex", texture);
            }
        }
    }

    void Update() {
        age += Time.deltaTime;
        if (age > lifespan) Destroy(gameObject);
        art.rotation = Quaternion.Euler(pitch,0,0) * transform.rotation * Quaternion.Euler(90, 0, 0);
    }
}
