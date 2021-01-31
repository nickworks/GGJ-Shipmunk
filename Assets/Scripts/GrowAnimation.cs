using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAnimation : MonoBehaviour
{
    public AnimationCurve curve;

    public float timeForEase = 0.5f;
    protected float timer = 0;
    
    bool isAnimating = false;
    float targetSize = 0;
    SpaceRigidbody body;

    public void Animate(SpaceRigidbody body, float targetSize = 0) {
        this.body = body;
        this.targetSize = targetSize;
        isAnimating = true;
    }

    // Update is called once per frame
    void Update() {
        if (!isAnimating) return;
        timer += Time.deltaTime * body.timeScale;
        float p = curve.Evaluate(timer / timeForEase);
        transform.localScale = Vector3.one * p * targetSize;
        if (timer >= timeForEase) isAnimating = false;
    }
}
