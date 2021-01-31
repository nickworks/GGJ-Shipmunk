using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public SpriteRenderer art;
    [Range(0,1)]public float spriteQueeze = 0.2f;

    public Sprite[] sprites;

    float lifespan = 10;
    float age = 0;
    float pitch = 60;
    SpaceRigidbody body;
    
    void Start() {
        body = GetComponent<SpaceRigidbody>();

        ApplyArt();

        // set rotational velocity:
        float quarterRange = 5;
        Vector3 vel = Vector3.zero;
        vel.y = Random.Range(-quarterRange, quarterRange) + Random.Range(-quarterRange, quarterRange);
        body.SetAngularVelocity(vel);
        pitch = Random.Range(-30, -10);

        // set random scale:
        transform.localScale = Vector3.one * Random.Range(.8f, 2f);
    }

    private void OnValidate() {
        UpdateColliderSize();
    }
    void ApplyArt() {
        if (art && sprites.Length > 0) {
            art.sprite = sprites[Random.Range(0,sprites.Length)];
            UpdateColliderSize();
        }
    }
    void UpdateColliderSize() {
        if (!art) return;
        Vector3 size = new Vector3(art.bounds.size.x - spriteQueeze, 1, art.bounds.size.z - spriteQueeze);
        GetComponent<BoxCollider>().size = size;
    }

    void Update() {
        age += Time.deltaTime * body.timeScale;
        if (age > lifespan) Destroy(gameObject);
        art.transform.rotation = Quaternion.Euler(pitch,0,0) * transform.rotation * Quaternion.Euler(90, 0, 0);
    }
}
