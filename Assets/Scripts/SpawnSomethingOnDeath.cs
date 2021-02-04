using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSomethingOnDeath : MonoBehaviour {

    public List<SpaceRigidbody> items;

    public void OnDie() {
        if (items.Count <= 0) return;
        
        SpaceRigidbody item = items[Random.Range(0, items.Count)];
        if(item) Instantiate(item, transform.position, transform.rotation);
    }
}
