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
    public static float Map(float v, float mina, float maxa, float minb, float maxb) {
        float p = (v - mina) / (maxa - mina);
        return Lerp(minb, maxb, p);
    }
    public static float Lerp(float min, float max, float p, bool shouldClamp = false) {
        if (shouldClamp) p = Clamp(p);
        return (max - min) * p + min;
    }
    public static Vector3 Lerp(Vector3 min, Vector3 max, float p, bool shouldClamp = false) {
        if (shouldClamp) p = Clamp(p);
        return (max - min) * p + min;
    }
    public static Quaternion Lerp(Quaternion a, Quaternion b, float p, bool allowExtrapolation = false) {

        b = WrapQuaternion(a, b);
        Quaternion rot = Quaternion.identity;

        rot.x = Lerp(a.x, b.x, p, allowExtrapolation);
        rot.y = Lerp(a.y, b.y, p, allowExtrapolation);
        rot.z = Lerp(a.z, b.z, p, allowExtrapolation);
        rot.w = Lerp(a.w, b.w, p, allowExtrapolation);

        return rot;
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
    public static Quaternion Slide(Quaternion val, Quaternion target, float percentLeftAfter1Second, float dt = -1) {
        if (dt < 0) dt = Time.deltaTime;
        float p = (1 - Mathf.Pow(percentLeftAfter1Second, dt));
        return Quaternion.Lerp(val, target, p);
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
    /// <summary>
    /// Trying to ease between angles > 180 degrees? You need to wrap your angles!
    /// </summary>
    /// <param name="baseAngle">This angle won't change.</param>
    /// <param name="angleToBeWrapped">This angle will change so that it is relative to baseAngle.</param>
    /// <returns>The wrapped angle.</returns>
    public static float AngleWrapDegrees(float baseAngle, float angleToBeWrapped) {

        while (baseAngle > angleToBeWrapped + 180) angleToBeWrapped += 360;
        while (baseAngle < angleToBeWrapped - 180) angleToBeWrapped -= 360;

        return angleToBeWrapped;
    }
    public static float AngleWrapRadians(float baseAngle, float angleToBeWrapped) {

        while (baseAngle > angleToBeWrapped + Mathf.PI) angleToBeWrapped += Mathf.PI * 2;
        while (baseAngle < angleToBeWrapped - Mathf.PI) angleToBeWrapped -= Mathf.PI * 2;

        return angleToBeWrapped;
    }
    public static Quaternion WrapQuaternion(Quaternion baseAngle, Quaternion angleToBeWrapped) {

        float alignment = Quaternion.Dot(baseAngle, angleToBeWrapped);

        if(alignment < 0) {

            angleToBeWrapped.x *= -1;
            angleToBeWrapped.y *= -1;
            angleToBeWrapped.z *= -1;
            angleToBeWrapped.w *= -1;

        }

        return angleToBeWrapped;
    }
}
