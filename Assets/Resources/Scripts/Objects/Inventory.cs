using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    List<Item> items;
    int maxSize;

    public Inventory() {
        items = new List<Item>();
        maxSize = 12;
    }

    public Inventory(List<Item> items) {
        this.items = items;
    }

    public bool addItem(Item item) {
        if(items.Count < maxSize) {
            items.Add(item);
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
    }

    public int getSize() {
        return items.Count;
    }
}
