using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityPanelController : MonoBehaviour {

    public Image img;
    public TMP_Text text;
    public Image bar;

    public void UpdateView(_Ability ability) {
        img.sprite = ability.sprite;
        text.text = ability.abilityName;
        //bar.fillAmount = ability.ammoPercent;
    }
}
