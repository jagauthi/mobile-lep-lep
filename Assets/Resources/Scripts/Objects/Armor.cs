using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item {

    protected string armorName, slot;
    protected int armorPower;

    public Armor(int itemNumber, string type, string armorName, string slot, Texture2D icon) 
    : base(itemNumber, type, icon) {
        this.armorName = armorName;
        this.slot = slot;
        basicInits();
    }

    public void basicInits() {
        loadArmorInfo(armorName);
    }

    void loadArmorInfo(string armorName) {
        getPlayer();
        if(armorName == "Iron Chest") {
            armorPower = 20;
        }
        else {
            armorPower = 10;
        }
        this.tooltip = "Armor: " + armorPower;
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
