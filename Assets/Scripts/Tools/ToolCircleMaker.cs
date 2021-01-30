using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ToolCircleMaker : MonoBehaviour {

    LineRenderer line;
    public int res = 16;
    [Range(0,10)] public float radius = 5;

    void OnValidate() {
        line = GetComponent<LineRenderer>();

        Vector3[] pts = new Vector3[res];
        float angleIncr = Mathf.PI * 2f / res;
        float angle = 0;
        for(int i = 0; i < res; i++) {

            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            pts[i] = radius * new Vector3(x, 0, y);
            angle += angleIncr;
        }

        line.positionCount = res;
        line.SetPositions(pts);

    }

}
