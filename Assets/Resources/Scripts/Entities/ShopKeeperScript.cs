using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopKeeperScript : NpcScript {

    public bool showingInventory = false;
    public int shopkeeperPageNumber = 0;
    public List<Item> inventory;

    public ShopKeeperScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        initInventory(npcName);
        initValues();
        // GetComponent<Renderer>().material.color = Color.magenta;
    }

    protected void initInventory(string name)
    {
        inventory = new List<Item>();

        //Add 2-4 consumeables
        inventory.AddRange(ItemHandler.generateItemsOfType(2, 4, "Consumable"));

        //Add 0-2 pieces of armor
        inventory.AddRange(ItemHandler.generateItemsOfType(0, 2, "Armor"));
    }
    
    public override void startInteraction(TownScript townScript)
    {
        showingInventory = true;

        //Closing the player inventory since shopkeeper will display it's own version of it to allow selling
        getPlayerScript().closeMenu("Inventory");
    }

    
    public int getCost(Item item) {
        return item.getCost();
    }

    public void closeInventory() {
        showingInventory = false;
    }

    public void movePage(int direction) {
        shopkeeperPageNumber += direction;
        if(shopkeeperPageNumber < 0) {
            shopkeeperPageNumber = 0;
        }
    }

    public bool buyItem(Item item) {
        inventory.Add(item);
        return true;
    }
}
