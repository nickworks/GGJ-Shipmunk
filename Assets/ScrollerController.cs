using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerController : MonoBehaviour
{

    public Vector3 velocity = new Vector3();

    void Start()
    {
        
    }

    
    void Update() {
        transform.position += velocity * Time.deltaTime;
    }
}
