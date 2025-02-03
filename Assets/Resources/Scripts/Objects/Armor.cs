using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item {

    protected string slot;
    protected int armorPower;

    public Armor(string baseName, string type, string slot, Texture2D icon, int cost, Rarity rarity, int armorPower) 
    : base(baseName, type, icon, cost, rarity) {
        this.slot = slot;
        this.armorPower = armorPower;
        this.tooltip = "Armor: " + armorPower;
        getPlayer();
    }

    public int getArmorPower() {
        return armorPower;
    }

    public string getSlot() {
        return slot;
    }

    public override bool use() {
        return getPlayer().getEquipment().equipArmor(playerScript, this);
    }
}
