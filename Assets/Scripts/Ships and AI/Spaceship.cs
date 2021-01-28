using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour {

    public float speed = 5;
    private Vector3 velocity;

    public Controller controller { get; private set; }

    void Start() {
        controller = GetComponent<Controller>();
    }

    
    void Update() {

        if (controller.wantsToMove) AddForce(controller.dirToMove * speed * Time.deltaTime);

        EulerPhysTick();
    }

    public void AddForce(Vector3 force) {
        force.y = 0;
        velocity += force;
    }
    public void EulerPhysTick() {
        transform.position += velocity * Time.deltaTime;
    }

}
