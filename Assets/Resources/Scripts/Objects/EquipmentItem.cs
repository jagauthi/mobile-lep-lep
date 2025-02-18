using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EquipmentItem : Item {

    protected string slot;
    protected int levelReq, strReq, intReq, agilReq;
    protected Dictionary<string, int> statBuffs;

    public EquipmentItem(string baseName, string type, Texture2D icon, int cost, Rarity rarity, string slot, int levelReq, int strReq, int intReq, int agilReq) 
    : base(baseName, type, icon, cost, rarity) {
        this.slot = slot;
        this.tooltip = "EquipmentItem: " + slot;
        this.levelReq = levelReq;
        this.strReq = strReq;
        this.intReq = intReq;
        this.agilReq = agilReq;
        getPlayer();
    }

    public string getSlot() {
        return slot;
    }

    public override bool use() {
        return getPlayer().getEquipment().equipItem(playerScript, this);
    }

    public void setStatBuffs(Dictionary<string, int> statBuffs) {
        this.statBuffs = statBuffs;
    }

    public Dictionary<string, int> getStatBuffs() {
        return this.statBuffs;
    }

    public int getLevelReq() {
        return levelReq;
    }
}
