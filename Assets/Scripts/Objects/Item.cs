using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item {

    protected int itemNumber;
    protected string type;
	protected Texture2D icon;
    protected PlayerScript playerScript;
    protected string tooltip;

    public Item(int itemNumber, string type, Texture2D icon) {
        this.itemNumber = itemNumber;
        this.type = type;
        this.icon = icon;
        playerScript = null;
        tooltip = "testTooltip";
    }

    public virtual bool use() {
        //Implemented by children classes
        return false;
    }

    protected void getPlayer() {
        if(playerScript == null) {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
    }

    public Texture2D getIcon() {
        return icon;
    }

    public string getType() {
        return type;
    }

    public string getTooltip()
    {
        return tooltip;
    }
}
