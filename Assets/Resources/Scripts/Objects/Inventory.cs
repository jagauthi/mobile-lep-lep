using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    bool open;
    List<Item> items;
    int maxSize;

    public Inventory() {
        items = new List<Item>();
        open = false;
        maxSize = 12;
    }

    public Inventory(List<Item> items) {
        this.items = items;
        open = false;
    }

    public bool addItem(Item item) {
        if(items.Count < maxSize) {
            items.Add(item);
            return true;
        }
        else {
            return false;
        }
    }

    public List<Item> getItems() {
        return items;
    }

    public void loseItem(Item item) {
        items.Remove(item);
    }

    public bool isOpen() {
        return open;
    }

    public void toggle() {
        open = !open;
    }

    public void close() {
        open = false;
    }

    public int getSize() {
        return items.Count;
    }
}
