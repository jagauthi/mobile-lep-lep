using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    List<Item> items;
    List<Item> stashItems;
    int maxSize, inventoryPage, stashPage;
    Transform playerInventoryPanel;
    PlayerScript playerScript;

    public Inventory() {
        items = new List<Item>();
        stashItems = new List<Item>();
        maxSize = 12;
        inventoryPage = 0;
        stashPage = 0;
        
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        initPlayerInventoryPanel();
    }

    public Inventory(List<Item> items) {
        this.items = items;
        maxSize = 12;
        inventoryPage = 0;
        stashPage = 0;
    }

    private void initPlayerInventoryPanel() {
        if(null == playerInventoryPanel) {
            playerInventoryPanel = GameObject.FindGameObjectWithTag("PlayerInventoryPanel").GetComponent<Transform>();
        }

        foreach(Item item in items) {
            UiManager.Instance.CreateButton(playerInventoryPanel, UiButton.ButtonType.Item, item.getBaseName(), item.getRarity(), item.getIcon(), () => playerScript.useItem(item));
        }
    }

    public bool addItem(Item item) {
        if(items.Count < maxSize) {
            items.Add(item);
            UiManager.Instance.CreateButton(playerInventoryPanel, UiButton.ButtonType.Item, item.getBaseName(), item.getRarity(), item.getIcon(), () => playerScript.useItem(item));
            return true;
        }
        else {
            Debug.Log("Inventory at max size");
            return false;
        }
    }

    public List<Item> getItems() {
        return items;
    }

    public void loseItem(Item item) {
        items.Remove(item);
        UiManager.Instance.RemoveButton(playerInventoryPanel, item.getBaseName());
    }

    public int getSize() {
        return items.Count;
    }

    public List<Item> getStashItems() {
        return stashItems;
    }

    public int getInventoryPageNumber() {
        return inventoryPage;
    }

    public void moveInventoryPage(int direction) {
        inventoryPage += direction;
        if(inventoryPage < 0) {
            inventoryPage = 0;
        }
    }

    public int getStashPageNumber() {
        return stashPage;
    }

    public void moveStashPage(int direction) {
        stashPage += direction;
        if(stashPage < 0) {
            stashPage = 0;
        }
    }
}
