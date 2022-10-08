using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityPanelController : MonoBehaviour {

    public Image img;
    public TMP_Text text;
    public Image barCooldown;
    public Image barAmmo;
    public Image barCharge;

    _Ability _ability;

    public void UpdateView(_Ability ability) {
        _ability = ability;
    }
    public void Update(){
        if(_ability){
            img.sprite = _ability.sprite;
            text.text = _ability.abilityName;
            barCooldown.fillAmount = _ability.cooldownPercent;
            barAmmo.fillAmount = _ability.ammoPercent;
            barCharge.fillAmount = _ability.chargedUpPercent;
            //_ability.
        }
    }

}
