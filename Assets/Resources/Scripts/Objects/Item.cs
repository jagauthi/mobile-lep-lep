using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Item {

    public enum Rarity { None, Common, Uncommon, Rare, Epic, Legendary }
    
    protected int cost;
    protected string baseName, type;
	protected Texture2D icon;
    protected PlayerScript playerScript;
    protected string tooltip;
    protected Rarity rarity;

    public Item(string baseName, string type, Texture2D icon, int cost, Rarity rarity) {
        this.baseName = baseName;
        this.type = type;
        this.icon = icon;
        this.cost = cost;
        this.rarity = rarity;
        playerScript = null;
        tooltip = "Cost: " + cost;
    }

    public virtual bool use() {
        //Implemented by children classes
        return false;
    }

    protected PlayerScript getPlayer() {
        if(playerScript == null && null != GameObject.FindGameObjectWithTag("Player")) {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
        return playerScript;
    }

    public Rarity getRarity() {
        return rarity;
    }

    public void setRarity(Rarity rarity) {
        this.rarity = rarity;
        updateTooltip();
    }

    public Texture2D getIcon() {
        return icon;
    }

    public string getBaseName() {
        return this.baseName;
    }

    public string getType() {
        return type;
    }

    public string getTooltip()
    {
        return tooltip;
    }

    public virtual void updateTooltip() {
        tooltip = "Cost: " + cost;
    }

    public int getCost()
    {
        return cost;
    }
}
