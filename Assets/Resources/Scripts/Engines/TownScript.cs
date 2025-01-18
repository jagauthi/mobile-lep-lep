using System.Collections.Generic;
using UnityEngine;

public class TownScript : MonoBehaviour
{
   public GameObject playerGameObject;
   public GameObject townOptionsGameObject;

   private PlayerScript playerScript;
   private TownMenuScript townMenuScript;
   protected List<EnemyScript> enemies;


    void Start()
    {
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        if(null == townMenuScript) {
            townMenuScript = townOptionsGameObject.GetComponent<TownMenuScript>();
        }

        enemies = new List<EnemyScript>();
    }

    public void doPlayerAbility(Ability ability) {
        if(!playerScript.useAbility(ability)) {
            Debug.Log("Not enough resource to use " + ability.getName());
        }
        else {
            Debug.Log("Player used " + ability.getName());
            for(int i = 0; i < enemies.Count; i++) {
                if(enemies[i].isDead()) {
                    continue;
                }
                else {
                    enemies[i].loseHealth(ability.getPower());
                    if(enemies[i].isDead()) {
                        playerScript.gainExp(enemies[i].expWorth);
                    }
                    return;
                }
            }
        }
    }

    public void usePlayerItem(Item item) {
        if(playerScript.useItem(item)) {
            Debug.Log("Player used " + item.getType());
        }
        else {
            Debug.Log("Could not use " + item.getType());
        }
    }
    
    protected void OnGUI(){
        drawDungeonThings();        
    }

    protected void drawDungeonThings() {
        drawEnemies();
        //drawAbilities();
    }

    protected void drawEnemies()
    {
        for(int i = 0; i < enemies.Count; i++) {
            GuiUtil.drawEnemyHealthBar(enemies[i], i);
        }
    }
}
