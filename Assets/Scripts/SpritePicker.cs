using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceRigidbody))]
public class SpritePicker : MonoBehaviour {

    public SpriteRenderer art;
    public Sprite[] randomSprites;
    SpaceRigidbody body;

    [Header("Collider Fit")]
    public bool shouldFitToCollider = false;
    [Tooltip("How many meters to squeeze in the sides of the BoxCollider.")]
    [Range(0,1)]public float spriteSqueeze = 0.2f;
    
    [Header("Dynamic Tilting")]
    public bool shouldTiltTowardsCam = true;
    public float pitch = 60;
    
    void Start() {
        body = GetComponent<SpaceRigidbody>();

        ApplyArt();

        // ASTEROID STUFF:
        
        // // set rotational velocity:
        // // float quarterRange = 5;
        // // Vector3 vel = Vector3.zero;
        // // vel.y = Random.Range(-quarterRange, quarterRange) + Random.Range(-quarterRange, quarterRange);
        // // body.SetAngularVelocity(vel);
        // // pitch = Random.Range(-30, -10);
        // // set random scale:
        // // transform.localScale = Vector3.one * Random.Range(.8f, 2f);
    }

    private void OnValidate() {
        ApplyArt();
        UpdateColliderSize();
    }
    void ApplyArt() {
        if (art && randomSprites.Length > 0) {
            art.sprite = randomSprites[Random.Range(0,randomSprites.Length)];
            UpdateColliderSize();
        }
    }
    void UpdateColliderSize() {
        if (!art) return;
        if (!shouldFitToCollider) return;
        var b = GetComponent<BoxCollider>();
        var s = GetComponent<SphereCollider>();
        if (b) {
            Vector3 size = new Vector3(art.bounds.size.x - spriteSqueeze, 1, art.bounds.size.z - spriteSqueeze);
            b.size = size;
        }
        else if (s) {
            s.radius = art.bounds.size.x / 2 - spriteSqueeze;
        }
    }

    void Update() {
        if (!shouldTiltTowardsCam) return;
        art.transform.rotation = Quaternion.Euler(pitch,0,0) * transform.rotation * Quaternion.Euler(90, 0, 0);
    }
}
