using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item {

    protected string slot;

    public EquipmentItem(string baseName, string type, string slot, Texture2D icon, int cost, Rarity rarity) 
    : base(baseName, type, icon, cost, rarity) {
        this.slot = slot;
        this.tooltip = "EquipmentItem: " + slot;
        getPlayer();
    }

    public string getSlot() {
        return slot;
    }

    public override bool use() {
        return getPlayer().getEquipment().equipItem(playerScript, this);
    }
}
