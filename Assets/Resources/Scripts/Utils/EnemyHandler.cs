using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHandler {

    GameScript gameScript;
    protected static List<EnemyScript> allEnemies;
    protected static List<EnemyScript> allBosses;
    protected static  Dictionary<string, EnemyScript> enemiesMappedByName;
    protected static  Dictionary<string, EnemyScript> bossesMappedByName;

    public static void loadEnemiesManually() {
        allEnemies = new List<EnemyScript>();
        allBosses = new List<EnemyScript>();
        
        allEnemies.Add(new EnemyScript("Cube", (Texture2D)Resources.Load("Images/SawahCube1"), 100, 10, 20, 5, 1));
        allBosses.Add(new EnemyScript("Dragon", (Texture2D)Resources.Load("Images/SawahDragon1"), 200, 20, 40, 20, 3));

        loaEnemiesIntoEnemyMaps(allEnemies, enemiesMappedByName);
        loaEnemiesIntoEnemyMaps(allBosses, bossesMappedByName);
    }
    
    private static void loaEnemiesIntoEnemyMaps(List<EnemyScript> enemyList, Dictionary<string, EnemyScript> enemiesMappedByName) {
        enemiesMappedByName = new Dictionary<string, EnemyScript>();
        foreach(EnemyScript enemy in enemyList) {
            //Populate the enemyByName map
            enemiesMappedByName.Add(enemy.getName(), enemy);
        }
    }

    public static Dictionary<string, EnemyScript> getEnemyMap() {
        return enemiesMappedByName;
    }

    public static List<EnemyScript> getEnemyList() {
        return allEnemies;
    }

    public static List<EnemyScript> generateEnemies(int numEnemies, bool bossFlag) {
        List<EnemyScript> enemiesToReturn = new List<EnemyScript>();

        List<EnemyScript> enemyList = null;
        if(bossFlag) {
            enemyList = allBosses;
        }
        else {
            enemyList = allEnemies;
        }

        for(int i = 0; i < numEnemies; i++) {
            EnemyScript enemy = enemyList[Random.Range(0, enemyList.Count)];
            enemiesToReturn.Add(new EnemyScript(enemy.getName(), enemy.getTexture(), enemy.maxHealth, enemy.getDamage(), 
                                                    enemy.getExpWorth(), enemy.getGoldWorth(), enemy.getNumLoot()));
        }

        return enemiesToReturn;
    }
}
