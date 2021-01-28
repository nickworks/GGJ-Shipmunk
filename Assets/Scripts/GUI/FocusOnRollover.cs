using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FocusOnRollover : MonoBehaviour, IPointerEnterHandler {
    Selectable bttn;

    void Start() {
        bttn = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        bttn.Select();
    }
}
