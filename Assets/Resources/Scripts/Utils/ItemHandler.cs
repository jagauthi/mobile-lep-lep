using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemHandler {

    GameScript gameScript;
    protected static List<Item> allItems;
    protected static  Dictionary<string, Item> itemsMappedByName;
    protected static  Dictionary<string, List<Item>> itemsMappedByType;

    public static void parseItems(string itemString) {
		List<Item> itemList = JsonUtility.FromJson<List<Item>>(itemString);
        allItems = itemList;
    }

    public static void loadItemsManually() {
        allItems = new List<Item>();
        
        //Consumables
        allItems.Add(new Consumable("Health Potion", "Consumable", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 10, 50, Item.Rarity.Common ) );
        allItems.Add(new Consumable("Mana Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50, Item.Rarity.Common ) );
        allItems.Add(new Consumable("Rage Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/RagePotion"), 10, 50, Item.Rarity.Common ) );
        allItems.Add(new Consumable("Energy Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/EnergyPotion"), 10, 50, Item.Rarity.Common ) );

        //Armor
        allItems.Add(new Armor("Iron Helm", "Armor", "Head", (Texture2D)Resources.Load("Images/IronHelm"), 10, Item.Rarity.Uncommon, 10 ) ) ;
        allItems.Add(new Armor("Iron Chest", "Armor", "Chest", (Texture2D)Resources.Load("Images/IronChest"), 30, Item.Rarity.Uncommon, 30 ) );
        allItems.Add(new Armor("Iron Legs", "Armor", "Legs", (Texture2D)Resources.Load("Images/IronLegs"), 20, Item.Rarity.Uncommon, 20 ) );
        allItems.Add(new Armor("Iron Boots", "Armor", "Feet", (Texture2D)Resources.Load("Images/IronBoots"), 10, Item.Rarity.Uncommon, 10 ) );

        //Crafting materials
        allItems.Add(new Item("Copper Ore", "Crafting Material", (Texture2D)Resources.Load("Images/CopperOre"), 2, Item.Rarity.Common ) ) ;
        allItems.Add(new Item("Iron Ore", "Crafting Material", (Texture2D)Resources.Load("Images/IronOre"), 3, Item.Rarity.Common ) ) ;
        allItems.Add(new Item("Copper Bar", "Crafting Material", (Texture2D)Resources.Load("Images/CopperBar"), 4, Item.Rarity.Common ) ) ;
        allItems.Add(new Item("Iron Bar", "Crafting Material", (Texture2D)Resources.Load("Images/IronBar"), 6, Item.Rarity.Common ) ) ;

        loadItemsIntoItemMaps(allItems);
    }
    
    private static void loadItemsIntoItemMaps(List<Item> itemList) {
        itemsMappedByName = new Dictionary<string, Item>();
        itemsMappedByType = new Dictionary<string, List<Item>>();
        foreach(Item item in itemList) {
            //Populate the itemByName map
            itemsMappedByName.Add(item.getBaseName(), item);

            //Add the item to the itemsByType map
            if(!itemsMappedByType.ContainsKey(item.getType())) {
                itemsMappedByType.Add(item.getType(), new List<Item>());
            }
            itemsMappedByType[item.getType()].Add(item);
        }
    }

    public static Dictionary<string, Item> getItemMap() {
        return itemsMappedByName;
    }

    public static List<Item> getItemList() {
        return allItems;
    }

    public static List<Item> generateItems(int minNumItems, int maxNumItems, List<Item> itemList) {
        List<Item> itemsToReturn = new List<Item>();
        if(null == itemList) {
            itemList = allItems;
        }

        for(int i = 0; i < maxNumItems; i++) {
            //50% chance to add an item
            if(Random.Range(0, 2) == 1) {
                itemsToReturn.Add(itemList[Random.Range(0, itemList.Count)]);
            }
        }

        //If we didn't get atleast min number of items, add the rest
        if(itemsToReturn.Count < minNumItems) {
            for(int i = 0; i < minNumItems - itemsToReturn.Count; i++) {
                itemsToReturn.Add(itemList[Random.Range(0, itemList.Count)]);
            }
        }

        return itemsToReturn;
    }

    public static List<Item> generateItemsOfType(int minNumItems, int maxNumItems, string type) {
        return generateItems(minNumItems, maxNumItems, itemsMappedByType[type]);
    }
}
