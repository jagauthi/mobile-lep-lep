using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemHandler {

    GameScript gameScript;
    List<Item> allItems;

	public ItemHandler(GameScript gameScript) {
        this.gameScript = gameScript;
        allItems = new List<Item>();
    }

    public void parseItems(string itemString) {
		List<Item> itemList = JsonUtility.FromJson<List<Item>>(itemString);
        this.allItems = itemList;
    }

    public List<Item> getAllItems() {
        return allItems;
    }
}
