using System.Collections.Generic;
using UnityEngine;

public class DungeonScript : MonoBehaviour
{
   public GameObject playerGameObject;
   public GameObject dungeonOptionsGameObject;

   private PlayerScript playerScript;
   private DungeonMenuScript dungeonMenuScript;
   protected List<EnemyScript> enemies;

   bool playerTurn;


    void Start()
    {
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        if(null == dungeonMenuScript) {
            dungeonMenuScript = dungeonOptionsGameObject.GetComponent<DungeonMenuScript>();
        }

        enemies = new List<EnemyScript>();
        enemies.Add(new EnemyScript("Cube", (Texture2D)Resources.Load("Images/SawahCube1")));
        enemies.Add(new EnemyScript("Dragon", (Texture2D)Resources.Load("Images/SawahDragon1")));
        
        playerTurn = true;
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
        GuiUtil.drawEnemies(enemies);
    }
}
