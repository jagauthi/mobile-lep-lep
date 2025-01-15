using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MageScript : PlayerScript {

    protected float manaBarLength;
    protected int currentMana;

    protected new void Start()
    {
        basicInits();
        initStats();
        inventory.addItem(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
    }

    new protected void initStats()
    {
        manaBarLength = regularBarLength;
        if(strength == 0) {
            strength = 1;
        }
        if(intelligence == 0) {
            intelligence = 5;
        }
        if(agility == 0) {
            agility = 3;
        }
        currentHealth = getMaxHealth();
        currentMana = getMaxResource();
    }

    protected new void Update()
    {
        basicUpdates();
        manaBarLength = (regularBarLength) * (currentMana / (float)getMaxResource());

    }

    protected new void OnGUI()
    {
        drawBasics();
        drawManaBar();
        drawAbilities();
    }

    protected void drawManaBar()
    {
        GUIStyle blueStyle = new GUIStyle(GUI.skin.box);
        blueStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.75f));
        if (manaBarLength > 0)
        {
            GUI.Box(new Rect(10, 60, manaBarLength, 20), "", blueStyle);
        }
        GUI.Box(new Rect(10, 60, regularBarLength, 20), currentMana + "/" + getMaxResource());
    }

    protected override void loadAbilities()
    {
        abilities.Add((Ability)gameScript.getAbilityMap()["Fireball"]);
        abilities.Add((Ability)gameScript.getAbilityMap()["Frostball"]);
        abilities.Add((Ability)gameScript.getAbilityMap()["Melee Attack"]);

    }
    

    protected new bool loseResource(int x) {
        if(currentMana >= x) {
            currentMana -= x;
            return true;
        }
        else {
            return false;
        }
    }

    protected override void fillResource() {
        currentMana = getMaxResource();
    }

    public override bool gainResource(int x) { 
        if(currentMana >= getMaxResource()) {
            return false;
        }
        else {
            if(currentMana + x > getMaxResource()) {
                this.fillResource();
            }
            else {
                currentMana += x;
            }
            return true;
        }
    }

    protected override int getMaxResource()
    {
        return 100 + (level * 20) + (intelligence * 20);
    }
}
