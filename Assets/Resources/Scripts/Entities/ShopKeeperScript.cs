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
        inventory.Add(new Consumable(0, "Health Potion", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Rage Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/RagePotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Energy Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/EnergyPotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50 ) );
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50 ) );
        inventory.Add(new Armor(0, "Armor", "Iron Helm", "Head", (Texture2D)Resources.Load("Images/IronHelm"), 10 ) ) ;
        inventory.Add(new Armor(0, "Armor", "Iron Chest", "Chest", (Texture2D)Resources.Load("Images/IronChest"), 30 ) );
        inventory.Add(new Armor(0, "Armor", "Iron Legs", "Legs", (Texture2D)Resources.Load("Images/IronLegs"), 20 ) );
        inventory.Add(new Armor(0, "Armor", "Iron Boots", "Feet", (Texture2D)Resources.Load("Images/IronBoots"), 10 ) );
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
