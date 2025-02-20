using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquipmentItem {

    protected GameObject weaponGameObject;

    float baseDamage;

    public Weapon(string baseName, string type, Texture2D icon, int cost, Rarity rarity, string slot, int levelReq, int strReq, int intReq, int agilReq, float baseDamage) 
    : base(baseName, type, icon, cost, rarity, slot, levelReq, strReq, intReq, agilReq) {
        this.baseDamage = baseDamage;
        getPlayer();
    }

    public float getDamage() {
        return baseDamage;
    }

    public override void updateTooltip() {
        this.tooltip = "Damnage: " + getDamage() + ", cost: " + cost;
    }
}
