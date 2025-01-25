using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHandler {

    GameScript gameScript;
    protected static List<EnemyScript> allEnemies;
    protected static  Dictionary<string, EnemyScript> enemiesMappedByName;

    public static void loadEnemiesManually() {
        allEnemies = new List<EnemyScript>();
        
        allEnemies.Add(new EnemyScript("Cube", (Texture2D)Resources.Load("Images/SawahCube1")));
        allEnemies.Add(new EnemyScript("Dragon", (Texture2D)Resources.Load("Images/SawahDragon1")));

        loaEnemiesIntoEnemyMaps(allEnemies);
    }
    
    private static void loaEnemiesIntoEnemyMaps(List<EnemyScript> enemyList) {
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

    public static List<EnemyScript> generateEnemies(int numEnemies, List<EnemyScript> enemyList) {
        List<EnemyScript> enemiesToReturn = new List<EnemyScript>();

        for(int i = 0; i < numEnemies; i++) {
            EnemyScript enemyToCopy = enemyList[Random.Range(0, enemyList.Count)];
            enemiesToReturn.Add(new EnemyScript(enemyToCopy.getName(), enemyToCopy.getTexture()));
        }

        return enemiesToReturn;
    }
}
