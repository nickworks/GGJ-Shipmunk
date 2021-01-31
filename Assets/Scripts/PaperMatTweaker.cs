using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaperMatTweaker : MonoBehaviour {
    void Awake() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material.SetFloat("seed", Random.Range(0, 1f));
    }
}
