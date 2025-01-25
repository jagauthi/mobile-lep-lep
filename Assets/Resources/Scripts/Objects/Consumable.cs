using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item {

    protected string consumableType;
    protected int power;

    public Consumable(string baseName, string type, string consumableType, Texture2D icon, int cost, int power)
         : base(baseName, type, icon, cost) {
        this.consumableType = consumableType;
        this.power = power;
        this.tooltip = consumableType + ": " + power;
    }

    public override bool use() {
        getPlayer();
        switch(consumableType) {
            case "Heal": {
                if(playerScript.gainHealth(power)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            case "ResourceHeal": {
                if(playerScript.gainResource(power)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            default: {
                //lol
                Debug.Log("rip not implemented");
                return false;
            }
        }
    }
}
