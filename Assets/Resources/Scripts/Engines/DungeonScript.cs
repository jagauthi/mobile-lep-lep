using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonScript : MonoBehaviour
{

    public static float ANIMATION_WAIT_TIME = 0.5f;
    public GameObject playerGameObject;

    private PlayerScript playerScript;
    protected List<EnemyScript> enemies;
    public Ability selectedPlayerAbility;
    bool playerTurn, inProgress;
    
    List<Item> lootFromThisRoom, totalLoot;
    int goldFromThisRoom, totalGold, expFromThisRoom, totalExp;
    int roomNumber, maxRooms;

    Transform dungeonOptionsButtonPanel;
    int dungeonOptionsSlotsMaxCount = 6;
    private List<GameObject> dungeonOptionSlots = new List<GameObject>();


    void Start()
    {
        if(null == playerGameObject) {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
        }
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        int floorNum = playerScript.getDungeonFloorNum();
        enemies = new List<EnemyScript>();
        
        playerTurn = true;
        inProgress = true;

        lootFromThisRoom = new List<Item>();
        totalLoot = new List<Item>();
        goldFromThisRoom = 0;
        totalGold = 0;
        expFromThisRoom = 0;
        totalExp = 0;
        
        roomNumber = 0;

        //There will be max of 3-5 rooms
        maxRooms = UnityEngine.Random.Range(3, 6);

        initDungeonOptionsPanel();

        initDungeonRoom();
    }

    private void initDungeonOptionsPanel()
    {
        if (null == dungeonOptionsButtonPanel)
        {
            GameObject dungeonOptionsPanelGameObject = (GameObject)Resources.Load("Prefabs/DungeonOptionsPanel");
            dungeonOptionsButtonPanel = MonoBehaviour.Instantiate(dungeonOptionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            dungeonOptionsButtonPanel.SetParent(canvas.transform, false);
            UiManager.dungeonOptionsButtonPanel = dungeonOptionsButtonPanel;
            UiManager.openPanel(UiManager.dungeonOptionsButtonPanel);
            UiManager.addPanelToList(UiManager.dungeonOptionsButtonPanel, UiManager.dungeonInitOnPanels);
        }

        for(int i = 0; i < dungeonOptionsSlotsMaxCount; i++) {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), dungeonOptionsButtonPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.DungeonOption);
            dungeonOptionSlots.Add(newSlot);
        }

        updateDungeonButtons();
    }

    public void updateDungeonButtons() {
         List<Ability> playerAbilities = playerScript.getAbilities();
         List<Item> playerItems = playerScript.getInventory().getItems();
         for(int i = 0; i < dungeonOptionsSlotsMaxCount; i++) {
            int slotNum = i;
            if(i < playerAbilities.Count) {
                Ability ability = playerAbilities[i];
                GameObject abilityButton = UiManager.Instance.CreateButton(dungeonOptionsButtonPanel, UiButton.ButtonType.DungeonOption, "", Item.Rarity.None, 
                                ability.getIcon(), () => selectPlayerAbility(ability), false);
            }
            else if(i - playerAbilities.Count < playerItems.Count) {
                Item item = playerItems[i - playerAbilities.Count];
                GameObject itemButton = UiManager.Instance.CreateButton(dungeonOptionsButtonPanel, UiButton.ButtonType.DungeonOption, "", item.getRarity(), item.getIcon(), 
                                () => {
                                    if(playerScript.useItem(item)) {
                                        destroyButton(slotNum);
                                    }
                                }, false);
            }
         }
    }

    private void destroyButton(int i) {
        GameObject buttonToDestroy = dungeonOptionSlots[i].transform.GetChild(0).gameObject;
        GameObject.Destroy(buttonToDestroy);
    }

    public void initDungeonRoom() {
        roomNumber++;

        //Clear the room tracking variables again for the next room
        goldFromThisRoom = 0;
        expFromThisRoom = 0;
        lootFromThisRoom = new List<Item>();

        inProgress = true;
        playerTurn = true;

        if(roomNumber == maxRooms) {
            Debug.Log("Boss room!");
            enemies.AddRange(EnemyHandler.generateEnemies(1, true));
        }
        else if(roomNumber < maxRooms) {
            List<String> typesOfRooms = new List<String>();
            typesOfRooms.Add("Monsters");
            // typesOfRooms.Add("Treasure");

            int roomType = UnityEngine.Random.Range(0, typesOfRooms.Count);
            if(typesOfRooms[roomType] == "Monsters") {
                //Generate 1-3 enemies
                int numberOfEnemies = UnityEngine.Random.Range(1, 4);
                enemies.AddRange(EnemyHandler.generateEnemies(numberOfEnemies, false));
            }
            else if(typesOfRooms[roomType] == "Treasure") {
                
            }
        }
        else {
            Debug.Log("No more rooms");
        }
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
        // drawDungeonThings();        
    }

    protected void drawDungeonThings() { 
        if(inProgress) {
            GuiUtil.drawPlayerAbilities(playerScript, this);
            GuiUtil.drawPlayerItems(playerScript, this);
            GuiUtil.drawEnemies(enemies, attackEnemy);
        }
        else {
            //Draw final rewards room if flag is true
            if(roomNumber == maxRooms) {
                // GuiUtil.drawFinalRewardScreen(playerScript, this);
                GuiUtil.drawRewardScreen(playerScript, this, true);
            }
            else {
                GuiUtil.drawRewardScreen(playerScript, this, false);
            }
        }
    }

    public bool attackEnemy(EnemyScript enemy) {

        StartCoroutine(attackCoRoutine(enemy));
        return true;
    }

    private IEnumerator attackCoRoutine(EnemyScript enemy) {
        Debug.Log("Attack Coroutine");
        //Check if this action can even happen
        if(!playerTurn) {
            Debug.Log("Not player's turn");
            yield return false;
        }
        else if(null == selectedPlayerAbility) {
            Debug.Log("No ability selected");
            yield return false;
        }
        else if(enemy.isDead()) {
            Debug.Log("Enemy " + enemy.getName() + " is already dead");
            yield return false;
        }

        //Now actually use the ability
        else if(!playerScript.useAbility(selectedPlayerAbility)) {
            Debug.Log("Not enough resource to use " + selectedPlayerAbility.getName());
            yield return false;
        }
        else {
       
             /*****
            Uncomment this once there's some animation for attacking
            */
            // Debug.Log("Starting Attack");
            //yield return new WaitForSeconds(ANIMATION_WAIT_TIME);
            // Debug.Log("Finished attack");

            //Finally if it gets here, player takes their turn and initiates the enemy turn
            enemy.loseHealth(selectedPlayerAbility.getPower());
            playerTurn = false;
            yield return new WaitForSeconds(ANIMATION_WAIT_TIME);
            enemy.resetDamageTaken();

            //Reshuffle the dead enemy to the end of the list
            if(enemy.isDead()) {
                enemies.Remove(enemy);
                enemies.Add(enemy);
            }

            takeEnemyTurns();
            
            yield return true;
        }
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
                    SceneManager.LoadScene("TownScene");
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
        lootFromThisRoom = new List<Item>();
        goldFromThisRoom = 0;
        expFromThisRoom = 0;
        for(int i = 0; i < enemies.Count; i++) {
            EnemyScript enemy = enemies[i];

            goldFromThisRoom += enemy.getGoldWorth();
            expFromThisRoom += enemy.getExpWorth();

            //If it's a boss room, guarentee more loot
            if(roomNumber == maxRooms) {
                lootFromThisRoom.AddRange(ItemHandler.generateItems(enemy.getNumLoot(), enemy.getNumLoot(), null));
            }
            else {
                lootFromThisRoom.AddRange(ItemHandler.generateItems(0, enemy.getNumLoot(), null));
            }
        }

        totalGold += goldFromThisRoom;
        totalExp += expFromThisRoom;
        totalLoot.AddRange(lootFromThisRoom);

        //Dispense the rewards to the player
        playerScript.gainExp(expFromThisRoom);
        playerScript.gainGold(goldFromThisRoom);

        Debug.Log("Dungeon room finished, gained " + goldFromThisRoom + " gold and " + expFromThisRoom + " experience");

        if(roomNumber == maxRooms) {
            playerScript.setMaxDungeonFloorNumCompleted(playerScript.getDungeonFloorNum());
        }

        enemies = new List<EnemyScript>();
        inProgress = false;
    }

    public string getLootFromThisRoomString() {
        string returnString = "";

        Dictionary<string, int> itemCountMap = new Dictionary<string, int>();
        for(int i = 0; i < lootFromThisRoom.Count; i++) {
            Item item = lootFromThisRoom[i];
            if(!itemCountMap.ContainsKey(item.getBaseName())) {
                itemCountMap.Add(item.getBaseName(), 0);
            }
            itemCountMap[item.getBaseName()] += 1;
        }
        foreach(var entry in itemCountMap) {
            returnString += entry.Value + "x " + entry.Key + ". ";
        }
        return returnString;
    }

    public List<Item> getLootFromThisRoom() {
        return lootFromThisRoom;
    }

    public int getGoldFromThisRoom() {
        return goldFromThisRoom;
    }

    public int getExpFromThisRoom() {
        return expFromThisRoom;
    }

    public void goBackToTown() {
        inProgress = false;
        SceneManager.LoadScene("TownScene");
    }

    public void nextRoom() {
        initDungeonRoom();
    }

    public int getRoomNum() {
        return roomNumber;
    }

    public int getMaxRoomNum() {
        return maxRooms;
    }
}
