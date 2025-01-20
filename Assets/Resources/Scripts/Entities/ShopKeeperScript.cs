using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperScript : NpcScript {

    public bool showingInventory = false;
    public List<Item> inventory;

    public ShopKeeperScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        npcName = "Shopkeeper David";
        initInventory(npcName);
        initValues();
        getGameScript();
        // GetComponent<Renderer>().material.color = Color.magenta;
    }

    protected void initInventory(string name)
    {
        inventory = new List<Item>();
        inventory.Add(new Consumable(0, "Health Potion", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 50));
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
        inventory.Add(new Consumable(0, "Rage Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/RagePotion"), 50));
        inventory.Add(new Consumable(0, "Energy Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/EnergyPotion"), 50));
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
        inventory.Add(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
        inventory.Add(new Armor(0, "Armor", "Iron Helm", "Head", (Texture2D)Resources.Load("Images/IronHelm")));
        inventory.Add(new Armor(0, "Armor", "Iron Chest", "Chest", (Texture2D)Resources.Load("Images/IronChest")));
        inventory.Add(new Armor(0, "Armor", "Iron Legs", "Legs", (Texture2D)Resources.Load("Images/IronLegs")));
        inventory.Add(new Armor(0, "Armor", "Iron Boots", "Feet", (Texture2D)Resources.Load("Images/IronBoots")));
    }
    
    public override void startInteraction()
    {
        showingInventory = true;
    }

    protected new void Update () {
        if(player == null || playerScript == null) {
            initValues();
        }
    }

    
    public int getCost(Item item) {
        return 10;
    }

    public void closeInventory() {
        showingInventory = false;
    }
}
