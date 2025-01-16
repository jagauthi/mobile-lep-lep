using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MageScript : PlayerScript {

    protected int currentMana;

    protected new void Start()
    {
        basicInits();
        initStats();
        inventory.addItem(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
    }

    new protected void initStats()
    {
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

    }

    protected new void OnGUI()
    {
        drawBasics();
        // drawManaBar();
        drawAbilities();
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

    public bool gainResource(int x) { 
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
