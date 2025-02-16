using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : EquipmentItem {

    protected int armorPower;

    public Armor(string baseName, string type, string slot, Texture2D icon, int cost, Rarity rarity, int armorPower) 
    : base(baseName, type, slot, icon, cost, rarity) {
        this.slot = slot;
        this.armorPower = armorPower;
        this.tooltip = "Armor: " + armorPower;
        getPlayer();
    }

    public int getArmorPower() {
        return armorPower;
    }
}
