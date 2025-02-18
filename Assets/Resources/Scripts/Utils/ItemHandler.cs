
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static Item;

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
        
        /********** Consumables **********/
        allItems.Add(new Consumable("Health Potion", "Consumable", (Texture2D)Resources.Load("Images/HealthPotion"), 10, Item.Rarity.Common, "Heal", 50 ) );
        allItems.Add(new Consumable("Mana Potion", "Consumable", (Texture2D)Resources.Load("Images/ManaPotion"), 10, Item.Rarity.Common, "ResourceHeal", 50 ) );
        // allItems.Add(new Consumable("Rage Potion", "Consumable", (Texture2D)Resources.Load("Images/RagePotion"), 10, Item.Rarity.Common, "ResourceHeal", 50 ) );
        // allItems.Add(new Consumable("Energy Potion", "Consumable", (Texture2D)Resources.Load("Images/EnergyPotion"), 10, Item.Rarity.Common, "ResourceHeal", 50 ) );



        /********** Armor **********/
        allItems.Add(new Armor("Copper Helm", "Armor", (Texture2D)Resources.Load("Images/CopperHelm"), 10, Item.Rarity.Common, "Head", 1, 1, 1, 1, 5f ) ) ;
        allItems.Add(new Armor("Copper Chest", "Armor", (Texture2D)Resources.Load("Images/CopperChest"), 30, Item.Rarity.Common, "Chest", 1, 1, 1, 1, 15f ) );
        allItems.Add(new Armor("Copper Legs", "Armor", (Texture2D)Resources.Load("Images/CopperLegs"), 20, Item.Rarity.Common, "Legs", 1, 1, 1, 1, 10f ) );
        allItems.Add(new Armor("Copper Boots", "Armor", (Texture2D)Resources.Load("Images/CopperBoots"), 10, Item.Rarity.Common, "Feet", 1, 1, 1, 1, 5f ) );

        allItems.Add(new Armor("Iron Helm", "Armor", (Texture2D)Resources.Load("Images/IronHelm"), 30, Item.Rarity.Common, "Head", 5, 10, 1, 1, 8f ) ) ;
        allItems.Add(new Armor("Iron Chest", "Armor", (Texture2D)Resources.Load("Images/IronChest"), 60, Item.Rarity.Common, "Chest", 5, 10, 1, 1, 20f ) );
        allItems.Add(new Armor("Iron Legs", "Armor", (Texture2D)Resources.Load("Images/IronLegs"), 40, Item.Rarity.Common, "Legs", 5, 10, 1, 1, 15f ) );
        allItems.Add(new Armor("Iron Boots", "Armor", (Texture2D)Resources.Load("Images/IronBoots"), 20, Item.Rarity.Common, "Feet", 5, 10, 1, 1, 8f ) );



        /********** Crafting materials **********/
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

    public static List<Item> generateItems(int minNumItems, int maxNumItems, List<Item> itemList, int level) {
        List<Item> itemsToReturn = new List<Item>();
        if(null == itemList) {
            itemList = allItems;
        }

        for(int i = 0; i < maxNumItems; i++) {
            //50% chance to add an item
            if(Random.Range(0, 2) == 1) {
                Item randomItem = itemList[Random.Range(0, itemList.Count)];
                if(randomItem is Armor) {
                    //Get a random armor if it was armor
                    itemsToReturn.Add(getRandomArmor(level));
                }
                //Otherwise just add this
                else {
                    itemsToReturn.Add(randomItem);
                }
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

    public static List<Item> generateItemsOfType(int minNumItems, int maxNumItems, string type, int level) {
        return generateItems(minNumItems, maxNumItems, itemsMappedByType[type], level);
    }

    public static Armor getRandomArmor(int level) {
        int randomNumber = Random.Range(0,4);
        string slotToLookFor = "";
        if(randomNumber == 0) {
            slotToLookFor = "Head";
        }
        else if(randomNumber == 1) {
            slotToLookFor = "Chest";
        }
        else if(randomNumber == 2) {
            slotToLookFor = "Legs";
        }
        else {
            slotToLookFor = "Feet";
        }
        
        List<Armor> armors = new List<Armor>();
        foreach(Item item in itemsMappedByType["Armor"]) {
            armors.Add((Armor)item);
        }

        //Sort by descending level req
        armors.Sort((x,y) => y.getLevelReq().CompareTo(x.getLevelReq()));

        //Get the first armor that meets the req
        //TODO: Eventually when there's more types, need to get the list of ones that meet req and then randomly pick one of those
        Armor pickedArmor = null;
        foreach(Armor armor in armors) {
            if(armor.getSlot() == slotToLookFor && level >= armor.getLevelReq() ){
                pickedArmor = armor;
                break;
            }
        }

        if(null == pickedArmor) {
            Debug.Log("Whoops: " + slotToLookFor + ", " + level);
            return null;
        }

        pickedArmor.setRarity(getRandomRarity());

        return pickedArmor;
    }

    public static Rarity getRandomRarity() {
        int randomNumber = Random.Range(1,101);
        Debug.Log(randomNumber);
        if(randomNumber < 70) {
            return Rarity.Common;
        }
        else if(randomNumber < 90) {
            return Rarity.Uncommon;
        }
        else if(randomNumber < 97) {
            return Rarity.Rare;
        }
        else if(randomNumber < 99) {
            return Rarity.Epic;
        }
        else {
            return Rarity.Legendary;
        }
    }

    public static float rarityModifier(Rarity rarity) {
        if(rarity == Rarity.None ) {
            return 1.0f;
        }
        else if(rarity == Rarity.Common) {
            return 1.0f;
        }
        else if(rarity == Rarity.Uncommon) {
            return 1.5f;
        }
        else if(rarity == Rarity.Rare) {
            return 2.0f;
        }
        else if(rarity == Rarity.Epic) {
            return 2.5f;
        }
        else if(rarity == Rarity.Legendary) {
            return 3.0f;
        }
        else {
            return 1.0f;
        }
    }
}
