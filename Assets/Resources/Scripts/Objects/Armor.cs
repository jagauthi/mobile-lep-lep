using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : EquipmentItem {

    protected float armorPower;

    public Armor(string baseName, string type, Texture2D icon, int cost, Rarity rarity, string slot, int levelReq, int strReq, int intReq, int agilReq, float armorPower) 
    : base(baseName, type, icon, cost, rarity, slot, levelReq, strReq, intReq, agilReq) {
        this.slot = slot;
        this.armorPower = armorPower;
        this.tooltip = "Armor: " + getArmorPower() + ", cost: " + cost;
        getPlayer();
    }

    public float getArmorPower() {
        return armorPower * ItemHandler.rarityModifier(rarity);
    }

    public override void updateTooltip() {
        this.tooltip = "Armor: " + getArmorPower() + ", cost: " + cost;
    }

    public Armor copyArmor() {
        return new Armor(baseName, type, icon, cost, rarity, slot, levelReq, strReq, intReq, agilReq, armorPower);
    }
    
}
