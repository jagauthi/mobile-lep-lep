using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeperScript : NpcScript {

    bool showingInventory = false;
    bool playerInRange = false;
    protected List<Item> inventory;

    Rect inventoryGroupRect;
    Rect backgroundRect;
    Rect closeButton;
    Rect introRect;

    protected new void Start ()
    {
        npcName = "Shopkeeper David";
        initInventory(npcName);
        initValues();
        getGameScript();
        GetComponent<Renderer>().material.color = Color.magenta;
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
        
        inventoryGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        introRect = new Rect(inventoryGroupRect.width/16, inventoryGroupRect.height/16, 3 * inventoryGroupRect.width / 4, inventoryGroupRect.height/16);
        backgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4);
        closeButton = new Rect( 13 * inventoryGroupRect.width / 16, Screen.height / 32, inventoryGroupRect.width / 8, Screen.height / 16);
    }
    
    public void showInventory()
    {
        showingInventory = true;
    }

    protected new void Update () {
        if(player == null || playerScript == null) {
            initValues();
        }
        else
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromPlayer <= 20)
            {
                playerInRange = true;
                //Look at player
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(player.transform.position - transform.position), 5f * Time.deltaTime);
            }
            else
            {
                playerInRange = false;
                showingInventory = false;
            }
        }
    }
    
    void OnGUI()
    {
        if (showingInventory && playerInRange)
        {
            gameScript.pauseGame();
            inventoryMenu();
        }
    }

    void inventoryMenu()
    {
        int buttonLength = (int)(inventoryGroupRect.width / 4);
        int buffer = (int)inventoryGroupRect.width/16;
        GUI.BeginGroup(inventoryGroupRect);
        GUI.Box(backgroundRect, "");
        GUI.Box(introRect, name + "'s Shop");
        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        for( int col = 0; col < 3; col++ ) {
            for( int row = 0; row < 3; row++ ) {
                int slotNum =  ( col * 3 ) + row;
                if(inventory.Count > slotNum) {
                    Item item = inventory[slotNum];
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    GUI.DrawTexture( slot, item.getIcon() );

                    //Button to buy the item
                    if (GUI.Button(slot, ""+slotNum)) {
                        if( playerScript.buyItem(item, getCost(item)) )
                        {
                            inventory.Remove(item);
                        }
                    }
                    
                    //cursor tooltip
                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - inventoryGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - inventoryGroupRect.y,
                            item.getTooltip().Length*8, Screen.height / 16 / 2);
                        GUI.Box(mouseTextRect, item.getTooltip());
                    }
                }
                else {
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    if (GUI.Button(slot, ""+slotNum)) {
                        
                    }
                }
                if (GUI.Button(closeButton, "X"))
                {
                    closeInventory();
                }
            }
        }
        GUI.EndGroup();
    }

    public int getCost(Item item) {
        return 10;
    }

    protected void closeInventory() {
        showingInventory = false;
        gameScript.unpauseGame();
    }
}
