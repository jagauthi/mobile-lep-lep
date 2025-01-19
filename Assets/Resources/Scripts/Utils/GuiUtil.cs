using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuiUtil : MonoBehaviour {
    
    public static float regularBarLength = Screen.width / 3;
    public static float expBarLength = Screen.width / 3;

    private static GUIStyle redStyle, blackStyle, greenStyle, blueStyle;

    private static Rect enemyGroupRect, enemyBackgroundRect;

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
        enemyBackgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 2 * Screen.height / 4);
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

    public static void drawEnemyHealthBar(EnemyScript enemyScript, int enemyNumber)
    {
        int xValue = 3 * Screen.width / 5;
        int yValue = 10 + (enemyNumber * 30);

        if(null == redStyle) {
            initStyles();
        }
        if (enemyScript != null)
        {
            float healthBarLength = (regularBarLength) * (enemyScript.currentHealth / (float)enemyScript.maxHealth);
            if (healthBarLength > 0)
            {
                GUI.Box(new Rect(xValue, yValue, healthBarLength, 20), "", redStyle);
            }
            GUI.Box(new Rect(xValue, yValue, regularBarLength, 20), enemyScript.currentHealth + "/" + enemyScript.maxHealth);
        }
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


        for(int enemyNumber = 0; enemyNumber < enemies.Count; enemyNumber++) {
            int yValue = 10 + (enemyNumber * 30);
            EnemyScript enemyScript = enemies[enemyNumber];
            if (enemyScript != null)
            {
                Rect slot = new Rect(buffer*(enemyNumber+1) + buttonLength*enemyNumber, 
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
}
