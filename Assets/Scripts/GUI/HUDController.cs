using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HUDController : MonoBehaviour {

    public AbilityPanelController prefabAbilityPanel;

    private AbilityPanelController[] panels;
    public void RebuildViews(PlayerController player) {

        if (panels != null) {
            foreach (var panel in panels) if(panel) Destroy(panel.gameObject);
        }
        panels = new AbilityPanelController[4];

        if (player == null) return;
        if (player.ship == null) return;
        if (player.ship.abilitySystems == null) return;

        Spaceship.AbilitySlots[] slots = new Spaceship.AbilitySlots[]{
            Spaceship.AbilitySlots.ActionA,
            Spaceship.AbilitySlots.ActionB,
            Spaceship.AbilitySlots.ActionC,
            Spaceship.AbilitySlots.ActionD
        };
        int i = 0;
        foreach (Spaceship.AbilitySlots slot in slots)
            if (player.ship.abilitySystems.ContainsKey(slot)) {
                AbilityPanelController panel = Instantiate(prefabAbilityPanel, transform);
                panel.UpdateView(player.ship.abilitySystems[slot]);
                RectTransform xform = panel.transform as RectTransform;
                xform.anchoredPosition = new Vector2(10, 10 + i * 47);
                panels[i++] = panel;
            }
    }
    
}
