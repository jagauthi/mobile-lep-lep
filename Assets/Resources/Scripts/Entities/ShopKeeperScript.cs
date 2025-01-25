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
        getGameScript();
        // GetComponent<Renderer>().material.color = Color.magenta;
    }

    protected void initInventory(string name)
    {
        inventory = new List<Item>();
        inventory.AddRange(ItemHandler.generateItemsOfType(5, "Consumable"));
        inventory.AddRange(ItemHandler.generateItemsOfType(3, "Armor"));
    }
    
    public override void startInteraction()
    {
        showingInventory = true;

        //Closing the player inventory since shopkeeper will display it's own version of it to allow selling
        getPlayerScript().closeMenu("Inventory");
    }

    protected new void Update () {
       
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
