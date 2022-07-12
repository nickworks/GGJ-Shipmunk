using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaperMatTweaker : MonoBehaviour {

        SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(spriteRenderer.sharedMaterial);
        spriteRenderer.material.SetFloat("seed", Random.Range(0, 1f));
    }
    public void SetTint(Color color){
        if(spriteRenderer) spriteRenderer.material.SetColor("_tint", color);
    }
}
