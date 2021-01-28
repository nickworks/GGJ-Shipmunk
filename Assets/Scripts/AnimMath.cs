using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimMath
{
    public static float Clamp(float val, float min = 0, float max = 1) {
        if (val < min) return min;
        if (val > max) return max;
        return val;
    }
    public static float Lerp(float min, float max, float p, bool shouldClamp = false) {
        if (shouldClamp) p = Clamp(p);
        return (max - min) * p + min;
    }
    public static Vector3 Lerp(Vector3 min, Vector3 max, float p, bool shouldClamp = false) {
        if (shouldClamp) p = Clamp(p);
        return (max - min) * p + min;
    }

    public static float Smooth(float min, float max, float p) {
        p = p * p * (3 - 2 * p);
        return Lerp(min, max, p);
    }
    public static Vector3 Smooth(Vector3 min, Vector3 max, float p) {
        p = p * p * (3 - 2 * p);
        return Lerp(min, max, p);
    }
    public static float Slide(float val, float target, float percentLeftAfter1Second, float dt = -1) {
        if (dt < 0) dt = Time.deltaTime;
        float p = (1 - Mathf.Pow(percentLeftAfter1Second, dt));
        return Lerp(val, target, p);
    }
    public static Vector3 Slide(Vector3 val, Vector3 target, float percentLeftAfter1Second, float dt = -1) {
        if (dt < 0) dt = Time.deltaTime;
        float p = (1 - Mathf.Pow(percentLeftAfter1Second, dt));
        return Lerp(val, target, p);
    }
    public static void Spring(
        ref float val,
        ref float vel,
        float target,
        float damp,
        float freq,
        float dt = -1
        ) {

        if (dt < 0) dt = Time.deltaTime;

        float k = 1 + 2 * dt * damp * freq;
        float tff = dt * freq * freq;
        float ttff = dt * tff;

        val = (k * val + dt * vel + ttff * target) / (k + ttff);
        vel = (vel + tff * (target - val)) / (k + ttff);

    }
    public static void Spring(
        ref Vector3 val,
        ref Vector3 vel,
        Vector3 target,
        float damp,
        float freq,
        float dt = -1
        ) {

        if (dt < 0) dt = Time.deltaTime;

        float k = 1 + 2 * dt * damp * freq;
        float tff = dt * freq * freq;
        float ttff = dt * tff;

        val = (k * val + dt * vel + ttff * target) / (k + ttff);
        vel = (vel + tff * (target - val)) / (k + ttff);

    }
}
