using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuiUtil : MonoBehaviour {
    
    public static float regularBarLength = Screen.width / 3;
    public static float expBarLength = Screen.width / 3;

    private static GUIStyle redStyle, blackStyle, greenStyle, blueStyle, wordWrapLabelStyle, disabledButtonStyle;

    private static Texture2D redTex, blackTex, greenTex, blueTex;
    private static Texture2D selectedTexture;
   
    static int dungeonFloorOffset = 0;
    

    //Shopkeeper rects
    private static Rect shopkeeperGroupRect, shopkeeperBackgroundRect, shopkeeperCloseButton, shopkeeperIntroRect, shopkeeperLeftButtonRect, shopkeeperRightButtonRect;

    //Profession dialog rects
    private static Rect professionDialogGroupRect, professionDialogBackgroundRect, professionDialogCloseButton, professionDialogIntroRect, professionDialogImageRect;
    private static Rect professionDialogTextRect, professionDialogStartDungeonButton, professionDialogStartCraftingButton;
    private static Rect professionDialogDungeonFloorButton, professionDialogDungeonUpButton, professionDialogDungeonDownButton;

    //Town script rects
    private static Rect townOptionsGroupRect, townOptionsBackgroundRect, npcGroupRect, npcBackgroundRect;

    //Dungeon script rects
    private static Rect enemyGroupRect, enemyBackgroundRect;
    private static Rect abilitiesGroupRect, abilitiesBackgroundRect, itemsGroupRect, itemsBackgroundRect;

    private static Rect dungeonRewardsGroupRect, dungeonRewardsBackgroundRect, dungeonRewardsIntroRect, dungeonRewardsTextRect, dungeonRewardsFirstButton;
    private static Rect dungeonRewardsSendToStashButton, dungeonRewardsContinueButton, dungeonRewardsGoToTownButton;
    
    //Player Menu rects
    private static Rect playerMenuOptionsGroupRect, playerMenuOptionsBackgroundRect;
    private static Rect mainMenuGroupRect, levelGroupRect;
    private static Rect inventoryGroupRect, inventoryIntroRect, inventoryLeftButton, inventoryRightButton;
    private static Rect stashGroupRect, stashIntroRect, stashLeftButton, stashRightButton;
    private static Rect pointsRect, backgroundRect, strengthRect, strengthTextRect;
    private static Rect agilityRect, agilityTextRect, intelligenceRect, intelligenceTextRect, quitRect, goldRect;
    private static Rect screenRect, startRect, quit2Rect;

    
    //Crafting screen rects
    private static Rect craftingDialogGroupRect, craftingDialogBackgroundRect, craftingDialogIntroRect, craftingDialogImageRect;
    private static Rect craftingDialogTextRect, craftingDialogOptionButton, craftingProgressBarRect;



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

        wordWrapLabelStyle = new GUIStyle(GUI.skin.label);
        wordWrapLabelStyle.wordWrap = true;

        disabledButtonStyle = new GUIStyle(GUI.skin.button);
        disabledButtonStyle.normal.textColor = Color.gray; // Gray out the text
        disabledButtonStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f, 0.5f)); // Semi-transparent background

        

        //Town script rects
        townOptionsGroupRect = new Rect(10, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
        townOptionsBackgroundRect = new Rect(0, 0, townOptionsGroupRect.width, townOptionsGroupRect.height);
        npcGroupRect = new Rect(1 * Screen.width / 4, 1 * Screen.height / 4, 1 * Screen.width / 2, 1 * Screen.height / 2);
        npcBackgroundRect = new Rect(0, 0, npcGroupRect.width, npcGroupRect.height);
        
        //Shopkeeper rects
        shopkeeperGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        shopkeeperIntroRect = new Rect(shopkeeperGroupRect.width/16, shopkeeperGroupRect.height/16, 3 * shopkeeperGroupRect.width / 4, shopkeeperGroupRect.height/16);
        shopkeeperBackgroundRect = new Rect(0, 0, shopkeeperGroupRect.width, shopkeeperGroupRect.height);
        shopkeeperCloseButton = new Rect( 13 * shopkeeperGroupRect.width / 16, Screen.height / 32, shopkeeperGroupRect.width / 8, Screen.height / 16);
        shopkeeperLeftButtonRect = new Rect( 1 * shopkeeperGroupRect.width / 8, 3 * shopkeeperGroupRect.height / 4, shopkeeperGroupRect.width / 4, shopkeeperGroupRect.height / 8);
        shopkeeperRightButtonRect = new Rect( 5 * shopkeeperGroupRect.width / 8, 3 * shopkeeperGroupRect.height / 4, shopkeeperGroupRect.width / 4, shopkeeperGroupRect.height / 8);
        

        //Profession dialog rects
        professionDialogGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 2, 3 * Screen.height / 4);
        professionDialogIntroRect = new Rect(professionDialogGroupRect.width/4, professionDialogGroupRect.height/16, 1 * professionDialogGroupRect.width / 2, professionDialogGroupRect.height/16);
        professionDialogBackgroundRect = new Rect(0, 0, professionDialogGroupRect.width, professionDialogGroupRect.height);
        professionDialogCloseButton = new Rect( 13 * professionDialogGroupRect.width / 16, professionDialogGroupRect.height / 16, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);
        professionDialogImageRect = new Rect( 0, 0, professionDialogGroupRect.width / 4, professionDialogGroupRect.height / 4);
        professionDialogTextRect = new Rect( 1 * professionDialogGroupRect.width / 16, professionDialogGroupRect.height / 4, professionDialogGroupRect.width / 2, professionDialogGroupRect.height / 4);
        professionDialogStartDungeonButton = new Rect( 1 * professionDialogGroupRect.width / 16, professionDialogGroupRect.height / 2, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);
        professionDialogStartCraftingButton = new Rect( 5 * professionDialogGroupRect.width / 16, professionDialogGroupRect.height / 2, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);
        professionDialogDungeonFloorButton = new Rect( 3 * professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 4, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);
        professionDialogDungeonUpButton = new Rect( 6 * professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 2, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);
        professionDialogDungeonDownButton = new Rect( 6 * professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 2 + professionDialogDungeonUpButton.height, professionDialogGroupRect.width / 8, professionDialogGroupRect.height / 8);


        //Dungeon Script rects 
        enemyGroupRect = new Rect(4 * Screen.width / 8, 2* Screen.height / 8, 3* Screen.width / 8, 2 * Screen.height / 4);
        enemyBackgroundRect = new Rect(0, 0, enemyGroupRect.width, enemyGroupRect.height);

        abilitiesGroupRect = new Rect(10, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
        abilitiesBackgroundRect = new Rect(0, 0, abilitiesGroupRect.width, abilitiesGroupRect.height);
        
        itemsGroupRect = new Rect(abilitiesGroupRect.width + 20, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
        itemsBackgroundRect = new Rect(0, 0, itemsGroupRect.width, itemsGroupRect.height);

        dungeonRewardsGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 2, 3 * Screen.height / 4);
        dungeonRewardsIntroRect = new Rect(dungeonRewardsGroupRect.width/4, dungeonRewardsGroupRect.height/16, 1 * dungeonRewardsGroupRect.width / 2, dungeonRewardsGroupRect.height/16);
        dungeonRewardsBackgroundRect = new Rect(0, 0, dungeonRewardsGroupRect.width, dungeonRewardsGroupRect.height);
        dungeonRewardsTextRect = new Rect( 1 * dungeonRewardsGroupRect.width / 16, dungeonRewardsGroupRect.height / 4, dungeonRewardsGroupRect.width / 2, dungeonRewardsGroupRect.height / 4);
        dungeonRewardsFirstButton = new Rect( 1 * dungeonRewardsGroupRect.width / 16, dungeonRewardsGroupRect.height / 2, dungeonRewardsGroupRect.width / 8, dungeonRewardsGroupRect.height / 8);
        dungeonRewardsSendToStashButton = new Rect( 1 * dungeonRewardsGroupRect.width / 16, 3 * dungeonRewardsGroupRect.height / 4, dungeonRewardsGroupRect.width / 4, dungeonRewardsGroupRect.height / 8);
        dungeonRewardsContinueButton = new Rect( 6 * dungeonRewardsGroupRect.width / 16, 3 * dungeonRewardsGroupRect.height / 4, dungeonRewardsGroupRect.width / 4, dungeonRewardsGroupRect.height / 8);
        dungeonRewardsGoToTownButton = new Rect( 11 * dungeonRewardsGroupRect.width / 16, 3 * dungeonRewardsGroupRect.height / 4, dungeonRewardsGroupRect.width / 4, dungeonRewardsGroupRect.height / 8);
        

        //Player Menu rects
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        startRect = new Rect(Screen.width/8, Screen.height/8, Screen.width / 4, Screen.height / 8);
        quit2Rect = new Rect(Screen.width / 8, Screen.height / 2, Screen.width / 4, Screen.height / 8);

        playerMenuOptionsGroupRect = new Rect(3 * Screen.width / 4, (7 * Screen.height / 8) - 10, Screen.width / 4, Screen.height / 8);
        playerMenuOptionsBackgroundRect = new Rect(0, 0, playerMenuOptionsGroupRect.width, playerMenuOptionsGroupRect.height);

        mainMenuGroupRect = new Rect(Screen.width / 8, Screen.height / 6, 3 * Screen.width / 4, 2 * Screen.height / 3);
        backgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4);

        inventoryGroupRect = new Rect(11 * Screen.width / 16, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        inventoryIntroRect = new Rect(inventoryGroupRect.width/4, inventoryGroupRect.height/16, 1 * inventoryGroupRect.width / 2, inventoryGroupRect.height/16);
        inventoryLeftButton = new Rect( 1 * inventoryGroupRect.width / 8, 3 * inventoryGroupRect.height / 4, inventoryGroupRect.width / 4, inventoryGroupRect.height / 8);
        inventoryRightButton = new Rect( 5 * inventoryGroupRect.width / 8, 3 * inventoryGroupRect.height / 4, inventoryGroupRect.width / 4, inventoryGroupRect.height / 8);
        
        stashGroupRect = new Rect(3 * Screen.width / 16, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        stashIntroRect = new Rect(stashGroupRect.width/4, stashGroupRect.height/16, 1 * stashGroupRect.width / 2, stashGroupRect.height/16);
        stashLeftButton = new Rect( 1 * stashGroupRect.width / 8, 3 * stashGroupRect.height / 4, stashGroupRect.width / 4, stashGroupRect.height / 8);
        stashRightButton = new Rect( 5 * stashGroupRect.width / 8, 3 * stashGroupRect.height / 4, stashGroupRect.width / 4, stashGroupRect.height / 8);

        levelGroupRect = new Rect(Screen.width / 6, Screen.height / 4, Screen.width / 2, 5 * Screen.height / 8);  
        int levelGroupBuffer = (int)levelGroupRect.width/16;
        int buttonLength = (int)(levelGroupRect.width / 8);
        int textLength = (int)(levelGroupRect.width / 8);
        pointsRect = new Rect(levelGroupBuffer, 0, textLength, buttonLength); 
        
        strengthTextRect = new Rect(levelGroupBuffer, Screen.height / 8, Screen.height / 4, textLength);
        strengthRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8, buttonLength, buttonLength);
        
        agilityTextRect = new Rect(levelGroupBuffer, Screen.height / 8 * 2, Screen.height / 4, textLength);
        agilityRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8 * 2, buttonLength, buttonLength);
        
        intelligenceTextRect = new Rect(levelGroupBuffer, Screen.height / 8 * 3, Screen.height / 4, textLength);
        intelligenceRect = new Rect(levelGroupBuffer + textLength + levelGroupBuffer, Screen.height / 8 * 3, buttonLength, buttonLength);

        quitRect = new Rect(Screen.width/2-buttonLength, 0, buttonLength, buttonLength);
        goldRect = new Rect(inventoryGroupRect.width/16, 15 * inventoryGroupRect.height/16, inventoryGroupRect.width/4, inventoryGroupRect.height/16);


        selectedTexture = (Texture2D)Resources.Load("Images/SelectedIcon");


        //Crafting dialog rects
        craftingDialogGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 2, 3 * Screen.height / 4);
        craftingDialogIntroRect = new Rect(craftingDialogGroupRect.width/4, craftingDialogGroupRect.height/16, 1 * craftingDialogGroupRect.width / 2, craftingDialogGroupRect.height/16);
        craftingDialogBackgroundRect = new Rect(0, 0, craftingDialogGroupRect.width, craftingDialogGroupRect.height);
        craftingDialogImageRect = new Rect( 0, 0, craftingDialogGroupRect.width / 4, craftingDialogGroupRect.height / 4);
        craftingDialogTextRect = new Rect( 1 * craftingDialogGroupRect.width / 16, craftingDialogGroupRect.height / 4, craftingDialogGroupRect.width / 2, craftingDialogGroupRect.height / 4);
        craftingDialogOptionButton = new Rect( 1 * craftingDialogGroupRect.width / 16, craftingDialogGroupRect.height / 2, craftingDialogGroupRect.width / 8, craftingDialogGroupRect.height / 8);
        craftingProgressBarRect = new Rect( 1 * craftingDialogGroupRect.width / 16, craftingDialogGroupRect.height / 2, craftingDialogGroupRect.width / 8, craftingDialogGroupRect.height / 8);

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
                Rect slot = new Rect(
                    buffer*(i+1) + buttonLength*i, 
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

    public static void drawNpcs(List<NpcScript> npcs, TownScript townScript)
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
                    npcScript.startInteraction(townScript);
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
                slotNum += (9 * shopkeeper.shopkeeperPageNumber);
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
                    String tooltipMessage = item.getTooltip() + ". Cost: " + item.getCost();
                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - shopkeeperGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - shopkeeperGroupRect.y,
                            tooltipMessage.Length*8, Screen.height / 16);
                        GUI.Box(mouseTextRect, tooltipMessage);
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
                if (GUI.Button(shopkeeperLeftButtonRect, "<-"))
                {
                    shopkeeper.movePage(-1);
                }
                if (GUI.Button(shopkeeperRightButtonRect, "->"))
                {
                    shopkeeper.movePage(1);
                }
            }
        }
        GUI.EndGroup();
    }

    public static void professionDialog(TownProfessionNpc selectedProfession, PlayerScript playerScript, TownScript townScript)
    {
        int buttonLength = (int)(professionDialogGroupRect.width / 4);
        int buffer = (int)professionDialogGroupRect.width/16;
        GUI.BeginGroup(professionDialogGroupRect);
        GUI.Box(professionDialogBackgroundRect, "");
        GUI.Box(professionDialogIntroRect, selectedProfession.getName());
        //GUI.DrawTexture(backgroundRect, backgroundTexture);

        GUI.DrawTexture( professionDialogImageRect, selectedProfession.getTexture() );
        
        //First dialog option to let player choose between dungeon and crafting
        if(selectedProfession.getDialogPhase() == "TownDialog") {

            GUI.Label(professionDialogTextRect, selectedProfession.getTownDialog());

            //Button to select start dungeon
            if (GUI.Button(professionDialogStartDungeonButton, "Yes")) {
                selectedProfession.setDialogPhase("DungeonSelection");
            }
            //Button to select start dungeon
            if (GUI.Button(professionDialogStartCraftingButton, "Crafting")) {
                selectedProfession.setDialogPhase("CraftingSelection");
            }
        }
        //Second dialog option lets the player choose which floor of the dungeon to go to
        else if(selectedProfession.getDialogPhase() == "DungeonSelection") {
            string dungeonSelectionText = "Which floor do you want to start on?";
            GUI.Label(professionDialogTextRect, dungeonSelectionText);

            int maxDungeonFloorNumCompleted = playerScript.getMaxDungeonFloorNumCompleted();
            for(int i = 0; i < 5; i++) {
                Rect floorButtonRect = new Rect( 
                    professionDialogDungeonFloorButton.x, 
                    professionDialogDungeonFloorButton.y + (professionDialogDungeonFloorButton.height * i + buffer), 
                    professionDialogDungeonFloorButton.width, 
                    professionDialogDungeonFloorButton.height
                );

                int floorNum = i + 1 + (dungeonFloorOffset * 5);
                if(maxDungeonFloorNumCompleted+1 < floorNum) {
                    GUI.Button(floorButtonRect, "" + floorNum, disabledButtonStyle);
                }
                else {
                    if(GUI.Button(floorButtonRect, "" + floorNum)) {
                        playerScript.setDungeonFloor(floorNum);
                        townScript.startDungeon();
                    }
                }
            }

            if(GUI.Button(professionDialogDungeonUpButton, "^")) {
                if(dungeonFloorOffset > 0) {
                    dungeonFloorOffset -= 1;
                }
            }
            if(GUI.Button(professionDialogDungeonDownButton, "V")) {
                dungeonFloorOffset += 1;
            }
        }
        //Third dialog option lets the player choose which crafting
        else if(selectedProfession.getDialogPhase() == "CraftingSelection") {
            GUI.Label(professionDialogTextRect, selectedProfession.getCraftingDialog());

            //Button to select start dungeon
            if (GUI.Button(professionDialogStartDungeonButton, "Mining")) {
                playerScript.setCurrentCrafting("Mining");
                townScript.startCrafting();
            }
            //Button to select start dungeon
            if (GUI.Button(professionDialogStartCraftingButton, "Smithing")) {
                playerScript.setCurrentCrafting("Smithing");
                townScript.startCrafting();
            }
        }
        
        if (GUI.Button(professionDialogCloseButton, "X"))
        {
            townScript.setSelectedProfession(null);
        }

        GUI.EndGroup();
    }

    public static void drawTownOptions(PlayerScript playerScript)
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
            //0 = Open Stash
            if(i == 0) {
                if (GUI.Button(slot, "Stash")) {
                    // Debug.Log("Character menu");
                    playerScript.toggleMenu("Stash");
                }
                // GUI.DrawTexture( slot, CHARACTER_MENU_ICON );
            }
            else {
                //...
            }

         }
        
        GUI.EndGroup();
    }

    public static void drawPlayerMenuOptions(PlayerScript playerScript)
    {
        int buffer = (int)playerMenuOptionsGroupRect.width/16;
        int buttonLength = (int)(playerMenuOptionsGroupRect.width / 3 - buffer);
        int buttonHeight = (int)(playerMenuOptionsGroupRect.height  - buffer);
        GUI.BeginGroup(playerMenuOptionsGroupRect);
        GUI.Box(playerMenuOptionsBackgroundRect, "");
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
                    // Debug.Log("Character menu");
                    playerScript.toggleMenu("Character");
                }
                // GUI.DrawTexture( slot, CHARACTER_MENU_ICON );
            }
            else if(i == 1) {
                if (GUI.Button(slot, "Inventory")) {
                    // Debug.Log("Inventory menu");
                    playerScript.toggleMenu("Inventory");
                }
                // GUI.DrawTexture( slot, INVENTORY_MENU_ICON );
            }
            else if(i == 2) {
                if (GUI.Button(slot, "Main")) {
                    // Debug.Log("Main menu");
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
        GUI.BeginGroup(mainMenuGroupRect);

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
            playerScript.closeMenu("Character");
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

        Dictionary<string, Armor> playerEquipment = playerScript.getEquipment().getItemMap();

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

    public static void playerInventoryMenu(PlayerScript playerScript, ShopKeeperScript shopkeeper, bool stashOpen)
    {
        int buttonLength = (int)(inventoryGroupRect.width / 4);
        int buffer = (int)inventoryGroupRect.width/16;
        GUI.BeginGroup(inventoryGroupRect);
        GUI.Box(backgroundRect, "");
        GUI.Box(goldRect, playerScript.getGold() + " gp");

        String introText = "Inventory";
        // if(null != shopkeeper) {
        //     introText += ", selling to " + shopkeeper.getName();
        // }
        // else if(stashOpen) {
        //     introText += ", transfering to/from stash";
        // }
        GUI.Box(inventoryIntroRect, introText);

        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        for( int col = 0; col < 3; col++ ) {
            for( int row = 0; row < 3; row++ ) {
                int slotNum =  ( col * 3 ) + row;
                slotNum += (9 * playerScript.getInventory().getInventoryPageNumber());
                if(playerScript.getInventory().getSize() > slotNum) {
                    Item item = playerScript.getInventory().getItems()[slotNum];
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    GUI.DrawTexture( slot, item.getIcon() );

                    if (GUI.Button(slot, ""+slotNum)) {
                        if(null != shopkeeper) {
                            playerScript.sellItem(item, shopkeeper);
                        }
                        else if(stashOpen) {
                            playerScript.getInventory().getStashItems().Add(item);
                            playerScript.getInventory().getItems().Remove(item);
                        }
                        else {
                            playerScript.useItem(item);
                        }
                    }

                    //cursor tooltip
                    String tooltipMessage = item.getTooltip();
                    if(null != shopkeeper) {
                        tooltipMessage += ". Sellprice: " + (item.getCost() / 2);
                    }

                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - inventoryGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - inventoryGroupRect.y,
                            tooltipMessage.Length * 8, Screen.height / 16);
                        GUI.Box(mouseTextRect, tooltipMessage);
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

        if(GUI.Button(inventoryLeftButton, "<")) {
            playerScript.getInventory().moveInventoryPage(-1);
        }

        if(GUI.Button(inventoryRightButton, ">")) {
            playerScript.getInventory().moveInventoryPage(1);
        }

        GUI.EndGroup();
    }

    public static void playerStashMenu(PlayerScript playerScript)
    {
        int buttonLength = (int)(stashGroupRect.width / 4);
        int buffer = (int)stashGroupRect.width/16;
        GUI.BeginGroup(stashGroupRect);
        GUI.Box(backgroundRect, "");
        GUI.Box(stashIntroRect, "Player stash");

        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        for( int col = 0; col < 3; col++ ) {
            for( int row = 0; row < 3; row++ ) {
                int slotNum =  ( col * 3 ) + row;
                slotNum += (9 * playerScript.getInventory().getStashPageNumber());
                if(playerScript.getInventory().getStashItems().Count > slotNum) {
                    Item item = playerScript.getInventory().getStashItems()[slotNum];
                    Rect slot = new Rect(buffer*(row+1) + buttonLength*row, 
                        buffer*(col+1) + buttonLength*(col+1), 
                        buttonLength, buttonLength);

                    GUI.DrawTexture( slot, item.getIcon() );

                    if (GUI.Button(slot, ""+slotNum)) {
                        if(playerScript.getInventory().addItem(item)) {
                            playerScript.getInventory().getStashItems().Remove(item);
                        }
                    }

                    //cursor tooltip
                    String tooltipMessage = item.getTooltip();

                    if (null != item && slot.Contains(Event.current.mousePosition))
                    {
                        Rect mouseTextRect = new Rect(
                            Input.mousePosition.x - stashGroupRect.x + (buffer / 2),
                            Screen.height - Input.mousePosition.y - stashGroupRect.y,
                            tooltipMessage.Length * 8, Screen.height / 16);
                        GUI.Box(mouseTextRect, tooltipMessage);
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

        if(GUI.Button(stashLeftButton, "<")) {
            playerScript.getInventory().moveStashPage(-1);
        }

        if(GUI.Button(stashRightButton, ">")) {
            playerScript.getInventory().moveStashPage(1);
        }
        GUI.EndGroup();
    }
    
    public static void drawPlayerAbilities(PlayerScript playerScript, DungeonScript dungeonScript)
    {
        int buffer = (int)abilitiesGroupRect.width/16;
        int buttonLength = (int)(abilitiesGroupRect.width / 4 - buffer);
        int buttonHeight = (int)(abilitiesGroupRect.height  - buffer);
        GUI.BeginGroup(abilitiesGroupRect);
        GUI.Box(abilitiesBackgroundRect, "");
         List<Ability> playerAbilities = playerScript.getAbilities();
      //   GUI.DrawTexture(backgroundRect, backgroundTexture);
         for(int i = 0; i < 4; i++) {

            //Create the rect for the slot for the ability
            Rect slot = new Rect(
               buffer/2*(i+1) + buttonLength*i, 
               buffer/2, 
               buttonLength, 
               buttonHeight);
            
            if(playerAbilities.Count > i) {

               Ability ability = playerAbilities[i];

               //Draw selection icon if this is the selected ability
                if(null != dungeonScript.selectedPlayerAbility && ability == dungeonScript.selectedPlayerAbility) {
                    GUI.DrawTexture(
                        new Rect(slot.x-5, slot.y-5, slot.width + 10, slot.height + 10),
                        selectedTexture
                    );
                }

               //Button to select the ability
               if (GUI.Button(slot, "")) {
                  dungeonScript.selectPlayerAbility(ability);
               }

               //Draw the icon over the button
               GUI.DrawTexture( slot, ability.getIcon() );
               
               //cursor tooltip
               if (null != ability && slot.Contains(Event.current.mousePosition)){
                  String abilityTooltip = "Tooltip for " + ability.getName();
                  Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - abilitiesGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - abilitiesGroupRect.y,
                        abilityTooltip.Length*8, Screen.height / 16 / 2);
                  GUI.Box(mouseTextRect, abilityTooltip);
               }
            }
            else {
               //Empty button slot
               if (GUI.Button(slot, "")) {
                  
               }
            }
         }
        
        GUI.EndGroup();
    }

    public static void drawPlayerItems(PlayerScript playerScript, DungeonScript dungeonScript)
    {
        int buffer = (int)itemsGroupRect.width/16;
        int buttonLength = (int)(itemsGroupRect.width / 4 - buffer);
        int buttonHeight = (int)(itemsGroupRect.height  - buffer);
        GUI.BeginGroup(itemsGroupRect);
        GUI.Box(itemsBackgroundRect, "");
         List<Item> playerItems = playerScript.getInventory().getItems();
      //   GUI.DrawTexture(backgroundRect, backgroundTexture);
         for(int i = 0; i < 4; i++) {

            //Create the rect for the slot for the ability
            Rect slot = new Rect(
               buffer/2*(i+1) + buttonLength*i, 
               buffer/2, 
               buttonLength, 
               buttonHeight);
            
            if(playerItems.Count > i) {

               Item item = playerItems[i];

               //Button to select the ability
               if (GUI.Button(slot, "")) {
                  dungeonScript.usePlayerItem(item);
               }

               //Draw the icon over the button
               GUI.DrawTexture( slot, item.getIcon() );
               
               //cursor tooltip
               if (null != item && slot.Contains(Event.current.mousePosition)){
                  String abilityTooltip = "Tooltip for " + item.getTooltip();
                  Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - itemsGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - itemsGroupRect.y,
                        abilityTooltip.Length*8, Screen.height / 16 / 2);
                  GUI.Box(mouseTextRect, abilityTooltip);
               }
            }
            else {
               //Empty button slot
               if (GUI.Button(slot, "")) {
                  
               }
            }
         }
        
        GUI.EndGroup();
    }

    public static void drawRewardScreen(PlayerScript playerScript, DungeonScript dungeonScript, bool lastRoomComplete)
    {
        int buttonLength = (int)(dungeonRewardsGroupRect.width / 4);
        int buffer = (int)dungeonRewardsGroupRect.width/16;
        GUI.BeginGroup(dungeonRewardsGroupRect);
        GUI.Box(dungeonRewardsBackgroundRect, "");
        GUI.Box(dungeonRewardsIntroRect, "Rewards for room " + dungeonScript.getRoomNum() + "/" + dungeonScript.getMaxRoomNum());
        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        
        //Text for the rewards
        String lootText = "Earned " + dungeonScript.getExpFromThisRoom() + " exp and " + dungeonScript.getGoldFromThisRoom() 
                            + " gold, as well as these items from the dungeon room: ";
        GUI.Label(dungeonRewardsTextRect, lootText, wordWrapLabelStyle);

        //Buttons for each of the items the player can loot
        List<Item> lootFromThisRoom = dungeonScript.getLootFromThisRoom();
        for(int i = 0; i < lootFromThisRoom.Count; i++) {
            Item thisItem = lootFromThisRoom[i];
            Rect thisButtonRect = new Rect(
                dungeonRewardsFirstButton.x + (i * dungeonRewardsFirstButton.width),
                dungeonRewardsFirstButton.y,
                dungeonRewardsFirstButton.width,
                dungeonRewardsFirstButton.height
            );

            GUI.DrawTexture( thisButtonRect, thisItem.getIcon() );
            if(GUI.Button(thisButtonRect, "")) {
                if(playerScript.getInventory().addItem(thisItem)) {
                    lootFromThisRoom.Remove(thisItem);
                }
            }
        }

        if(GUI.Button(dungeonRewardsSendToStashButton, "Send to stash")) {
            Debug.Log("Send items to stash");
            playerScript.getInventory().getStashItems().AddRange(lootFromThisRoom);
            lootFromThisRoom.Clear();
        }
        
        if(GUI.Button(dungeonRewardsGoToTownButton, "Go to town")) {
            dungeonScript.goBackToTown();
        }
        
        if(!lastRoomComplete) {
            if(GUI.Button(dungeonRewardsContinueButton, "Next room")) {
                dungeonScript.nextRoom();
            }
        }

        GUI.EndGroup();
    }

    public static void craftingDialog(PlayerScript playerScript, CraftingScript craftingScript)
    {
        int buttonLength = (int)(craftingDialogGroupRect.width / 4);
        int buffer = (int)craftingDialogGroupRect.width/16;
        GUI.BeginGroup(craftingDialogGroupRect);
        GUI.Box(craftingDialogBackgroundRect, "");
        GUI.Box(craftingDialogIntroRect, playerScript.getCurrentCrafting());
        //GUI.DrawTexture(backgroundRect, backgroundTexture);
        
        //Let player choose which product to craft
        if(null == craftingScript.getProductCurrentlyCrafting()) {

            Dictionary<string, List<string>> craftingOptionsMap = craftingScript.getCraftingOptionsMap();
            List<string> craftingOptions = craftingOptionsMap[playerScript.getCurrentCrafting()];

            GUI.Label(craftingDialogTextRect, "Which product do you want to craft?");
            for(int i = 0; i < craftingOptions.Count; i++) {
                Rect craftingOptionButton = new Rect(
                    craftingDialogOptionButton.x,
                    craftingDialogOptionButton.y,
                    craftingDialogOptionButton.width,
                    craftingDialogOptionButton.height
                );
                if(GUI.Button(craftingOptionButton, craftingOptions[i])) {
                    craftingScript.setProductCurrentlyCrafting(craftingOptions[i]);
                }
            }
            
        }
        //Crafting
        else {
            //Draw button to increment crafting manually
            if(GUI.Button(craftingDialogOptionButton, "")) {
                Debug.Log("Crafting 1x" + craftingScript.getProductCurrentlyCrafting());
            }

            //Draw progress bar for the crafting
            drawCraftingProgressBar(craftingScript);
        }

        GUI.EndGroup();
    }


    
    public static void drawCraftingProgressBar(CraftingScript craftingScript)
    { 
        int craftingProgress = craftingScript.getCurrentCraftingProgress();
        int maxCraftingProgress = craftingScript.getMaxCraftingProgress();
        if(null == greenStyle) {
            initStyles();
        }
        float craftingBarLength = regularBarLength * (craftingProgress / (float)maxCraftingProgress);

        if (craftingBarLength > 0)
        {
            GUI.Box(new Rect(10, 10, craftingBarLength, 20), "", greenStyle);
        }
        GUI.Box(new Rect(10, 10, regularBarLength, 20), craftingProgress + "/" + maxCraftingProgress);
    }

}
