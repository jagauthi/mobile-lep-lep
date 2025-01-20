using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuiUtil : MonoBehaviour {
    
    public static float regularBarLength = Screen.width / 3;
    public static float expBarLength = Screen.width / 3;

    private static Texture2D redTex, blackTex, greenTex, blueTex;
    private static GUIStyle redStyle, blackStyle, greenStyle, blueStyle;

    private static Rect enemyGroupRect, enemyBackgroundRect, npcGroupRect, npcBackgroundRect;

    private static Rect shopkeeperGroupRect, shopkeeperBackgroundRect, shopkeeperCloseButton, shopkeeperIntroRect;
    private static Rect townOptionsGroupRect, townOptionsBackgroundRect;
    
    private static Rect mainGroupRect, levelGroupRect, inventoryGroupRect;
    private static Rect pointsRect, backgroundRect, strengthRect, strengthTextRect;
    private static Rect agilityRect, agilityTextRect, intelligenceRect, intelligenceTextRect, quitRect, goldRect;

    private static Rect screenRect, startRect, quit2Rect;

    public static ShopKeeperScript shopkeeperShowingInventory;

    public static void initStyles() {

        redStyle = new GUIStyle(GUI.skin.box);
        redTex = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.75f));
        redStyle.normal.background = redTex;

        blackStyle = new GUIStyle(GUI.skin.box);
        blackTex = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
        blackStyle.normal.background = blackTex;
        
        greenStyle = new GUIStyle(GUI.skin.box);
        greenTex = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.75f));
        greenStyle.normal.background = greenTex;
        
        blueStyle = new GUIStyle(GUI.skin.box);
        blueTex = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.75f));
        blueStyle.normal.background = blueTex;
        
        enemyGroupRect = new Rect(4 * Screen.width / 8, 2* Screen.height / 8, 3* Screen.width / 8, 2 * Screen.height / 4);
        enemyBackgroundRect = new Rect(0, 0, enemyGroupRect.width, enemyGroupRect.height);
        
        npcGroupRect = new Rect(1 * Screen.width / 4, 1 * Screen.height / 4, 1 * Screen.width / 2, 1 * Screen.height / 2);
        npcBackgroundRect = new Rect(0, 0, npcGroupRect.width, npcGroupRect.height);

        townOptionsGroupRect = new Rect(10, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
        townOptionsBackgroundRect = new Rect(0, 0, townOptionsGroupRect.width, townOptionsGroupRect.height);
        
        shopkeeperGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        shopkeeperIntroRect = new Rect(shopkeeperGroupRect.width/16, shopkeeperGroupRect.height/16, 3 * shopkeeperGroupRect.width / 4, shopkeeperGroupRect.height/16);
        shopkeeperBackgroundRect = new Rect(0, 0, shopkeeperGroupRect.width, shopkeeperGroupRect.height);
        shopkeeperCloseButton = new Rect( 13 * shopkeeperGroupRect.width / 16, Screen.height / 32, shopkeeperGroupRect.width / 8, Screen.height / 16);
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        startRect = new Rect(Screen.width/8, Screen.height/8, Screen.width / 4, Screen.height / 8);
        quit2Rect = new Rect(Screen.width / 8, Screen.height / 2, Screen.width / 4, Screen.height / 8);

        mainGroupRect = new Rect(Screen.width / 8, Screen.height / 6, 3 * Screen.width / 4, 2 * Screen.height / 3);
        inventoryGroupRect = new Rect(11 * Screen.width / 16, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        backgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4);

        levelGroupRect = new Rect(Screen.width / 6, Screen.height / 4, Screen.width / 2, 5 * Screen.height / 8);  
        int levelGroupBuffer = (int)levelGroupRect.width/16;
        int buttonLength = (int)(levelGroupRect.width / 8);
        int textLength = (int)(levelGroupRect.width / 8);
        pointsRect = new Rect(levelGroupBuffer, 0, textLength, buttonLength); 
        //Str
        strengthTextRect = new Rect(levelGroupBuffer, Screen.height / 8, Screen.height / 4, textLength);
        strengthRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8, buttonLength, buttonLength);
        //Agil
        agilityTextRect = new Rect(levelGroupBuffer, Screen.height / 8 * 2, Screen.height / 4, textLength);
        agilityRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8 * 2, buttonLength, buttonLength);
        //Intel
        intelligenceTextRect = new Rect(levelGroupBuffer, Screen.height / 8 * 3, Screen.height / 4, textLength);
        intelligenceRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8 * 3, buttonLength, buttonLength);

        quitRect = new Rect(Screen.width/2-buttonLength, 0, buttonLength, buttonLength);
        goldRect = new Rect(inventoryGroupRect.width/16, inventoryGroupRect.height/16, inventoryGroupRect.width/4, inventoryGroupRect.height/16);
    }

    public static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    
    public static void drawHealthBar(int currentHealth, int maxHealth, int level)
    { 
        if(null == redStyle) {
            initStyles();
        }
        float healthBarLength = regularBarLength * (currentHealth / (float)maxHealth);

        if (healthBarLength > 0)
        {
            GUI.Box(new Rect(10, 10, healthBarLength, 20), "", redStyle);
        }
        GUI.Box(new Rect(10, 10, regularBarLength, 20), currentHealth + "/" + maxHealth);
        GUI.Box(new Rect(regularBarLength + 20, 10, 20, 20), "" + level);
    }

    public static void drawExpBar(int exp, int expToNextLevel, int skillPoints)
    {
        if(null == greenStyle) {
            initStyles();
        }
        
        expBarLength = regularBarLength * (exp / (float)expToNextLevel);
        if (expBarLength > 0)
        {
            GUI.Box(new Rect(10, 35, expBarLength, 20), "", greenStyle);
        }
        GUI.Box(new Rect(10, 35, regularBarLength, 20), exp + "/" + expToNextLevel);
        if(skillPoints > 0)
        {
            GUI.Box(new Rect(regularBarLength + 20, 35, 20, 20), "+");
        }
    }

    public static void drawResourceBar(int currentResource, int maxResource)
    {
        float manaBarLength = GuiUtil.regularBarLength * (currentResource / (float)maxResource);
        if (manaBarLength > 0)
        {
            GUI.Box(new Rect(10, 60, manaBarLength, 20), "", blueStyle);
        }
        GUI.Box(new Rect(10, 60, GuiUtil.regularBarLength, 20), currentResource + "/" + maxResource);
    }

    public static void drawEnemies(List<EnemyScript> enemies, Func<EnemyScript, bool> getAttackedFunction)
    {
        int buttonLength = (int)(enemyGroupRect.width / 4);
        int buffer = (int)enemyGroupRect.width/16;

        if(null == redStyle) {
            initStyles();
        }

        
        GUI.BeginGroup(enemyGroupRect);
        GUI.Box(enemyBackgroundRect, "");


        for(int i = 0; i < enemies.Count; i++) {
            int yValue = 10 + (i * 30);
            EnemyScript enemyScript = enemies[i];
            if (enemyScript != null)
            {
                Rect slot = new Rect(buffer*(i+1) + buttonLength*i, 
                    buffer, 
                    buttonLength, 
                    buttonLength); 

                GUI.DrawTexture( slot, enemyScript.getTexture() );
                
                //Button to select enemy
                if (GUI.Button(slot, "" + enemyScript.getName())) {
                    // Debug.Log("Clicked on enemy " + enemyScript.getName());
                    getAttackedFunction(enemyScript);
                }

                //Enemy healthbar
                float healthBarLength = (buttonLength) * (enemyScript.currentHealth / (float)enemyScript.maxHealth);
                // Debug.Log("HealthbarLength " + healthBarLength);
                // Debug.Log("redstyle " + redStyle);
                if (healthBarLength > 0)
                {
                    GUI.Box(new Rect(slot.x, slot.y, healthBarLength, 20), "", redStyle);
                }
                GUI.Box(new Rect(slot.x, slot.y, buttonLength, 20), enemyScript.currentHealth + "/" + enemyScript.maxHealth);

                //cursor tooltip
                if (null != enemyScript && slot.Contains(Event.current.mousePosition))
                {
                    String tooltipText = "Tooltip for " + enemyScript.getName();
                    Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - enemyGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - enemyGroupRect.y,
                        tooltipText.Length*8, Screen.height / 16 / 2);
                    GUI.Box(mouseTextRect, tooltipText);
                }
            }
        }

        GUI.EndGroup();
    }

    public static void drawNpcs(List<NpcScript> npcs)
    {
        int buttonLength = (int)(npcGroupRect.width / 4);
        int buffer = (int)npcGroupRect.width/16;

        if(null == redStyle) {
            initStyles();
        }

        GUI.BeginGroup(npcGroupRect);
        GUI.Box(npcBackgroundRect, "");

        for(int i = 0; i < npcs.Count; i++) {
            int yValue = 10 + (i * 30);
            NpcScript npcScript = npcs[i];
            if (npcScript != null)
            {
                Rect slot = new Rect(buffer*(i+1) + buttonLength*i, 
                    buffer, 
                    buttonLength, 
                    buttonLength); 

                GUI.DrawTexture( slot, npcScript.getTexture() );
                
                //Button to select enemy
                if (GUI.Button(slot, "" + npcScript.getName())) {
                    // Debug.Log("Clicked on NPC " + npcScript.getName());
                    npcScript.startInteraction();
                }

                //cursor tooltip
                if (null != npcScript && slot.Contains(Event.current.mousePosition))
                {
                    String tooltipText = "Tooltip for " + npcScript.getName();
                    Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - npcGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - npcGroupRect.y,
                        tooltipText.Length*8, Screen.height / 16 / 2);
                    GUI.Box(mouseTextRect, tooltipText);
                }
            }
        }

        GUI.EndGroup();
    }

    public static void shopkeeperMenu(ShopKeeperScript shopkeeper, PlayerScript playerScript)
    {
        int buttonLength = (int)(shopkeeperGroupRect.width / 4);
        int buffer = (int)shopkeeperGroupRect.width/16;
        GUI.BeginGroup(shopkeeperGroupRect);
        GUI.Box(shopkeeperBackgroundRect, "");
        GUI.Box(shopkeeperIntroRect, shopkeeper.getName() + "'s Shop");
        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        for( int col = 0; col < 3; col++ ) {
            for( int row = 0; row < 3; row++ ) {
                int slotNum =  ( col * 3 ) + row;
                if(shopkeeper.inventory.Count > slotNum) {
                    Item item = shopkeeper.inventory[slotNum];
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    GUI.DrawTexture( slot, item.getIcon() );

                    //Button to buy the item
                    if (GUI.Button(slot, ""+slotNum)) {
                        if( playerScript.buyItem(item, shopkeeper.getCost(item)) )
                        {
                            shopkeeper.inventory.Remove(item);
                        }
                    }
                    
                    //cursor tooltip
                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - shopkeeperGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - shopkeeperGroupRect.y,
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
                if (GUI.Button(shopkeeperCloseButton, "X"))
                {
                    shopkeeper.closeInventory();
                }
            }
        }
        GUI.EndGroup();
    }

    public static void drawPlayerTownOptions(PlayerScript playerScript)
    {
        int buffer = (int)townOptionsGroupRect.width/16;
        int buttonLength = (int)(townOptionsGroupRect.width / 4 - buffer);
        int buttonHeight = (int)(townOptionsGroupRect.height  - buffer);
        GUI.BeginGroup(townOptionsGroupRect);
        GUI.Box(townOptionsBackgroundRect, "");
      //   GUI.DrawTexture(backgroundRect, backgroundTexture);
        for(int i = 0; i < 4; i++) {

            //Create the rect for the slot for the ability
            Rect slot = new Rect(
               buffer/2*(i+1) + buttonLength*i, 
               buffer/2, 
               buttonLength, 
               buttonHeight);

            //For now, manually defining the buttons that player can press in town.
            //0 = Open Character menu
            //1 = Open Inventory menu
            //2 = Open Escape Menu
            if(i == 0) {
                if (GUI.Button(slot, "Character")) {
                    Debug.Log("Character menu");
                    playerScript.toggleMenu("Character");
                }
                // GUI.DrawTexture( slot, CHARACTER_MENU_ICON );
            }
            else if(i == 1) {
                if (GUI.Button(slot, "Inventory")) {
                    Debug.Log("Inventory menu");
                    playerScript.toggleMenu("Inventory");
                }
                // GUI.DrawTexture( slot, INVENTORY_MENU_ICON );
            }
            else if(i == 2) {
                if (GUI.Button(slot, "Main")) {
                    Debug.Log("Main menu");
                    playerScript.toggleMenu("Main");
                }
                // GUI.DrawTexture( slot, MAIN_MENU_ICON );
            }
            else {
                //...
            }

         }
        
        GUI.EndGroup();
    }

    public static void mainMenu()
    {
        GUI.Box(screenRect, "");
        GUI.BeginGroup(mainGroupRect);

        GUI.Box(backgroundRect, "");
        if (GUI.Button(startRect, "Quit to Central Hub"))
        {
            //saveAndQuit();
            Debug.Log("Quit to Central Hub not implemented");
        }
        if (GUI.Button(quit2Rect, "Quit the Whole Game"))
        {
            Application.Quit();
        }
        GUI.EndGroup();
    }

    public static void characterMenu(PlayerScript playerScript)
    {
        GUI.BeginGroup(levelGroupRect);
        GUI.Box(backgroundRect, "");
        //GUI.DrawTexture(backgroundRect, backgroundTexture);

        GUI.Label(pointsRect, "Points: " + playerScript.getSkillPoints());
        GUI.Label(strengthTextRect, "Strength: " + playerScript.getStrength());
        GUI.Label(agilityTextRect, "Agility: " + playerScript.getAgility());
        GUI.Label(intelligenceTextRect, "Intelligence: " + playerScript.getIntelligence());
        if (playerScript.getSkillPoints() > 0)
        {
            if (GUI.Button(strengthRect, "+"))
            {
                playerScript.setStrength(playerScript.getStrength() + 1);
                playerScript.setSkillPoints(playerScript.getSkillPoints() - 1);
            }
            if (GUI.Button(agilityRect, "+"))
            {
                playerScript.setAgility(playerScript.getAgility() + 1);
                playerScript.setSkillPoints(playerScript.getSkillPoints() - 1);
            }
            if (GUI.Button(intelligenceRect, "+"))
            {
                playerScript.setIntelligence(playerScript.getIntelligence() + 1);
                playerScript.setSkillPoints(playerScript.getSkillPoints() - 1);
            }
        }

        drawArmorSlots(playerScript);

        if (GUI.Button(quitRect, "X"))
        {
            playerScript.toggleMenu("Character");
        }
        GUI.EndGroup();
    }

    public static void drawArmorSlots(PlayerScript playerScript)
    {
        int levelGroupBuffer = (int)levelGroupRect.width/16;
        int armorButtonLength = (int)(3 * levelGroupRect.height / 16);
        float armorSlotCenterX = 5 * levelGroupRect.width / 8;
        int armorSlotBuffer = levelGroupBuffer / 2;
        float armorSlotYBuffer = levelGroupRect.height / 8;

        Hashtable playerEquipment = playerScript.getEquipment().getItemMap();

        //Head slot
        Armor playerHead = (Armor)playerEquipment["Head"];
        Texture2D helmTexture = (Texture2D)Resources.Load("Images/HeadSlot");
        Rect headSlot = new Rect(armorSlotCenterX, armorSlotYBuffer, armorButtonLength, armorButtonLength);
        drawArmorSlot(playerScript, playerHead, helmTexture, headSlot);

        //Chest slot
        Armor playerChest = (Armor)playerEquipment["Chest"];
        Texture2D chestTexture = (Texture2D)Resources.Load("Images/ChestSlot");
        Rect chestSlot = new Rect(armorSlotCenterX, armorSlotYBuffer + armorButtonLength + armorSlotBuffer, armorButtonLength, armorButtonLength);
        drawArmorSlot(playerScript, playerChest, chestTexture, chestSlot);

        //Legs slot
        Armor playerLegs = (Armor)playerEquipment["Legs"];
        Texture2D legsTexture = (Texture2D)Resources.Load("Images/LegsSlot");
        Rect legsSlot = new Rect(
            armorSlotCenterX, 
            armorSlotYBuffer + (2 * (armorButtonLength + armorSlotBuffer)), 
            armorButtonLength, armorButtonLength);
        drawArmorSlot(playerScript, playerLegs, legsTexture, legsSlot);

        //Feet slot
        Armor playerFeet = (Armor)playerEquipment["Feet"];
        Texture2D feetTexture = (Texture2D)Resources.Load("Images/BootsSlot");
        Rect feetSlot = new Rect(
            armorSlotCenterX + armorButtonLength + armorSlotBuffer, 
            armorSlotYBuffer + (2 * (armorButtonLength + armorSlotBuffer)), 
            armorButtonLength, armorButtonLength);
        drawArmorSlot(playerScript, playerFeet, feetTexture, feetSlot);

        //Armor Stats Text
        int totalArmor = 0;
        foreach(Armor armor in playerEquipment.Values)
        {
            totalArmor += (null != armor) ? armor.getArmorPower() : 0;
        }
        Rect armorTextSlot = new Rect(
            armorSlotCenterX,
            armorSlotYBuffer + (3 * (armorButtonLength + armorSlotBuffer)), 
            armorButtonLength, armorButtonLength);
        GUI.Label(armorTextSlot, "Armor: " + totalArmor);
    }

    public static void drawArmorSlot(PlayerScript playerScript, Armor armor, Texture2D helmTexture, Rect armorSlot)
    {
        int levelGroupBuffer = (int)levelGroupRect.width/16;
        //Draw armor texture
        if (null != armor)
        {
            helmTexture = armor.getIcon();
        }
        GUI.DrawTexture(armorSlot, helmTexture);

        if (GUI.Button(armorSlot, ""))
        {
            playerScript.getEquipment().unequipArmor(playerScript, armor);
        }

        //cursor tooltip
        if (null != armor && armorSlot.Contains(Event.current.mousePosition))
        {
            Rect mouseTextRect = new Rect(
                Input.mousePosition.x - levelGroupRect.x + (levelGroupBuffer / 2),
                Screen.height - Input.mousePosition.y - levelGroupRect.y,
                armor.getTooltip().Length * 8, levelGroupBuffer / 2);
            GUI.Box(mouseTextRect, armor.getTooltip());
        }
    }

    public static void inventoryMenu(PlayerScript playerScript)
    {
        int buttonLength = (int)(inventoryGroupRect.width / 4);
        int buffer = (int)inventoryGroupRect.width/16;
        GUI.BeginGroup(inventoryGroupRect);
        GUI.Box(backgroundRect, "");
        GUI.Box(goldRect, playerScript.getGold() + " gp");
        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        for( int col = 0; col < 3; col++ ) {
            for( int row = 0; row < 3; row++ ) {
                int slotNum =  ( col * 3 ) + row;
                if(playerScript.getInventory().getSize() > slotNum) {
                    Item item = playerScript.getInventory().getItems()[slotNum];
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    GUI.DrawTexture( slot, item.getIcon() );

                    if (GUI.Button(slot, ""+slotNum)) {
                        playerScript.useItem(item);
                    }

                    //cursor tooltip
                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - inventoryGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - inventoryGroupRect.y,
                            item.getTooltip().Length * 8, Screen.height / 16 / 2);
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
            }
        }
        GUI.EndGroup();
    }
}
