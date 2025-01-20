using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuiUtil : MonoBehaviour {
    
    public static float regularBarLength = Screen.width / 3;
    public static float expBarLength = Screen.width / 3;

    private static GUIStyle redStyle, blackStyle, greenStyle, blueStyle;

    private static Rect enemyGroupRect, enemyBackgroundRect, npcGroupRect, npcBackgroundRect;

    private static Rect shopkeeperGroupRect, shopkeeperBackgroundRect, shopkeeperCloseButton, shopkeeperIntroRect;

    public static ShopKeeperScript shopkeeperShowingInventory;

    public static void initStyles() {

        redStyle = new GUIStyle(GUI.skin.box);
        redStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.75f));

        blackStyle = new GUIStyle(GUI.skin.box);
        blackStyle.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
        
        greenStyle = new GUIStyle(GUI.skin.box);
        greenStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.75f));
        
        blueStyle = new GUIStyle(GUI.skin.box);
        blueStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.75f));
        
        enemyGroupRect = new Rect(4 * Screen.width / 8, 2* Screen.height / 8, 3* Screen.width / 8, 2 * Screen.height / 4);
        enemyBackgroundRect = new Rect(0, 0, enemyGroupRect.width, enemyGroupRect.height);
        
        npcGroupRect = new Rect(1 * Screen.width / 4, 1 * Screen.height / 4, 1 * Screen.width / 2, 1 * Screen.height / 2);
        npcBackgroundRect = new Rect(0, 0, npcGroupRect.width, npcGroupRect.height);
        
        shopkeeperGroupRect = new Rect(2 * Screen.width / 8, Screen.height / 8, Screen.width / 4, 3 * Screen.height / 4);
        shopkeeperIntroRect = new Rect(shopkeeperGroupRect.width/16, shopkeeperGroupRect.height/16, 3 * shopkeeperGroupRect.width / 4, shopkeeperGroupRect.height/16);
        shopkeeperBackgroundRect = new Rect(0, 0, shopkeeperGroupRect.width, shopkeeperGroupRect.height);
        shopkeeperCloseButton = new Rect( 13 * shopkeeperGroupRect.width / 16, Screen.height / 32, shopkeeperGroupRect.width / 8, Screen.height / 16);
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
                    Debug.Log("Clicked on NPC " + npcScript.getName());
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
        if(!shopkeeper.showingInventory) {
            return;
        }
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
}
