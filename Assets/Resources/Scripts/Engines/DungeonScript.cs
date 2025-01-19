using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonScript : MonoBehaviour
{
    public GameObject playerGameObject;
    public GameObject dungeonOptionsGameObject;

    private PlayerScript playerScript;
    private DungeonMenuScript dungeonMenuScript;
    protected List<EnemyScript> enemies;
    public Ability selectedPlayerAbility;
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

    public void selectPlayerAbility(Ability ability) {
        selectedPlayerAbility = ability;
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
        GuiUtil.drawEnemies(enemies, attackEnemy);
    }

    public bool attackEnemy(EnemyScript enemy) {

        //Check if this action can even happen
        if(null == selectedPlayerAbility) {
            Debug.Log("No ability selected");
            return false;
        }
        if(enemy.isDead()) {
            Debug.Log("Enemy " + enemy.getName() + " is already dead");
            return false;
        }

        //Now actually use the ability
        if(!playerScript.useAbility(selectedPlayerAbility)) {
            Debug.Log("Not enough resource to use " + selectedPlayerAbility.getName());
            return false;
        }
        else {
            enemy.loseHealth(selectedPlayerAbility.getPower());
            return true;
        }
    }
}
