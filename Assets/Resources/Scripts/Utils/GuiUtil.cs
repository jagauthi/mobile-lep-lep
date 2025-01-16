using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GuiUtil : MonoBehaviour {
    
    public static float regularBarLength = Screen.width / 3;
    public static float expBarLength = Screen.width / 3;

    private static GUIStyle redStyle, blackStyle, greenStyle, blueStyle;

    public static void initStyles() {

        redStyle = new GUIStyle(GUI.skin.box);
        redStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.75f));

        blackStyle = new GUIStyle(GUI.skin.box);
        blackStyle.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
        
        greenStyle = new GUIStyle(GUI.skin.box);
        greenStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.75f));
        
        blueStyle = new GUIStyle(GUI.skin.box);
        blueStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.75f));
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
}
