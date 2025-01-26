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
        
        allItems.Add(new Consumable("Health Potion", "Consumable", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 10, 50));
        allItems.Add(new Consumable("Mana Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 10, 50));
        allItems.Add(new Consumable("Rage Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/RagePotion"), 10, 50 ) );
        allItems.Add(new Consumable("Energy Potion", "Consumable", "ResourceHeal", (Texture2D)Resources.Load("Images/EnergyPotion"), 10, 50 ) );
        allItems.Add(new Armor("Iron Helm", "Armor", "Head", (Texture2D)Resources.Load("Images/IronHelm"), 10, 10 ) ) ;
        allItems.Add(new Armor("Iron Chest", "Armor", "Chest", (Texture2D)Resources.Load("Images/IronChest"), 30, 30 ) );
        allItems.Add(new Armor("Iron Legs", "Armor", "Legs", (Texture2D)Resources.Load("Images/IronLegs"), 20, 20 ) );
        allItems.Add(new Armor("Iron Boots", "Armor", "Feet", (Texture2D)Resources.Load("Images/IronBoots"), 10, 10 ) );

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

    public static List<Item> generateItems(int numItems, List<Item> itemList) {
        List<Item> itemsToReturn = new List<Item>();
        if(null == itemList) {
            itemList = allItems;
        }

        for(int i = 0; i < numItems; i++) {
            itemsToReturn.Add(itemList[Random.Range(0, itemList.Count)]);
        }

        return itemsToReturn;
    }

    public static List<Item> generateItemsOfType(int numItems, string type) {
        return generateItems(numItems, itemsMappedByType[type]);
    }
}
