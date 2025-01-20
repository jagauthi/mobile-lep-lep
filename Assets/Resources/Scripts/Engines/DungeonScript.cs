using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if(null == playerGameObject) {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
        }
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
    }

    protected void drawEnemies()
    {
        GuiUtil.drawEnemies(enemies, attackEnemy);
    }

    public bool attackEnemy(EnemyScript enemy) {

        //Check if this action can even happen
        if(!playerTurn) {
            Debug.Log("Not player's turn");
            return false;
        }
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
       
        //Finally if it gets here, player takes their turn and initiates the enemy turn
        enemy.loseHealth(selectedPlayerAbility.getPower());
        playerTurn = false;
        takeEnemyTurns();
        
        return true;
    }

    public void takeEnemyTurns() {

        int enemiesDead = 0;

        //First loop to do the alive enemies turns and check if everyone's dead
        for(int i = 0; i < enemies.Count; i++) {
            EnemyScript enemy = enemies[i];
            if(!enemy.isDead()) {
                Debug.Log("Enemy " + enemy.getName() + " hits player for " + enemy.damage);
                playerScript.loseHealth(enemy.damage);
                if(playerScript.isDead()) {
                    Debug.Log("Player dead!");
                }
            }
            else {
                enemiesDead++;
            }
        }

        if(enemiesDead < enemies.Count) {
            Debug.Log("Enemy turns ended, player's turn");
            playerTurn = true;
            return;
        }
        
        //Otherwise if made it here, all the enemies are dead, give the loot to the player
        List<Item> lootFromEnemies = new List<Item>();
        int goldFromEnemies = 0;
        int expFromEnemies = 0;

        for(int i = 0; i < enemies.Count; i++) {
            EnemyScript enemy = enemies[i];
            goldFromEnemies += enemy.goldWorth;
            expFromEnemies += enemy.expWorth;
        }

        playerScript.gainExp(expFromEnemies);
        playerScript.gainGold(goldFromEnemies);
        Debug.Log("Dungeon level finished, gained " + goldFromEnemies + " gold and " + expFromEnemies + " experience");
        SceneManager.LoadScene("TownScene");
    }
}
