using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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
    bool playerTurn;
    
    List<Item> lootFromThisRoom, totalLoot;
    int goldFromThisRoom, totalGold, expFromThisRoom, totalExp;
    int roomNumber, maxRooms;

    Transform dungeonOptionsButtonPanel, dungeonEnemiesPanel, dungeonPlayerPlaceholderPanel, dungeonRewardsPanel;
    int dungeonOptionsSlotsMaxCount = 6;
    private List<GameObject> dungeonOptionSlots = new List<GameObject>();
    private List<GameObject> rewardsLootSlots = new List<GameObject>();
    List<GameObject> enemySlots = new List<GameObject>();
    TextMeshProUGUI rewardsText;
    Transform rewardsLootPanel, rewardsActionsPanel;


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

        lootFromThisRoom = new List<Item>();
        totalLoot = new List<Item>();
        goldFromThisRoom = 0;
        totalGold = 0;
        expFromThisRoom = 0;
        totalExp = 0;
        
        roomNumber = 0;

        //There will be max of 3-5 rooms
        maxRooms = UnityEngine.Random.Range(3, 3);

        initDungeonOptionsPanel();
        initDungeonEnemiesPanel();
        initDungeonPlayerPlaceholderPanel();
        initDungeonRewardsPanel();

        initDungeonRoom();
    }

    private void initDungeonOptionsPanel()
    {
        if (null == UiManager.dungeonOptionsButtonPanel)
        {
            GameObject dungeonOptionsPanelGameObject = (GameObject)Resources.Load("Prefabs/DungeonOptionsPanel");
            dungeonOptionsButtonPanel = MonoBehaviour.Instantiate(dungeonOptionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            dungeonOptionsButtonPanel.SetParent(canvas.transform, false);
            UiManager.dungeonOptionsButtonPanel = dungeonOptionsButtonPanel;
            UiManager.openPanel(UiManager.dungeonOptionsButtonPanel);
            UiManager.addPanelToList(UiManager.dungeonOptionsButtonPanel, UiManager.dungeonInitOnPanels);
        }
        else {
            dungeonOptionsButtonPanel = UiManager.dungeonOptionsButtonPanel;
        }

        UiManager.clearExistingSlotsAndButtons(dungeonOptionsButtonPanel);
        loadButtons();
    }

    async void loadButtons()
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        dungeonOptionSlots.Clear();
        for(int i = 0; i < dungeonOptionsSlotsMaxCount; i++) {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), dungeonOptionsButtonPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.DungeonOption);
            dungeonOptionSlots.Add(newSlot);
        }
 
        updateDungeonButtons();
    }

    private void initDungeonEnemiesPanel()
    {
        if (null == UiManager.dungeonEnemiesPanel)
        {
            GameObject dungeonEnemiesPanelGameObject = (GameObject)Resources.Load("Prefabs/DungeonEnemiesPanel");
            dungeonEnemiesPanel = MonoBehaviour.Instantiate(dungeonEnemiesPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            dungeonEnemiesPanel.SetParent(canvas.transform, false);
            UiManager.dungeonEnemiesPanel = dungeonEnemiesPanel;
            UiManager.openPanel(UiManager.dungeonEnemiesPanel);
            UiManager.addPanelToList(UiManager.dungeonEnemiesPanel, UiManager.dungeonInitOnPanels);
        }
        else {
            dungeonEnemiesPanel = UiManager.dungeonEnemiesPanel;
        }
    }

    private void initDungeonPlayerPlaceholderPanel()
    {
        if (null == UiManager.dungeonPlayerPlaceholderPanel)
        {
            GameObject dungeonPlayerPlaceholderPanelGameObject = (GameObject)Resources.Load("Prefabs/DungeonPlayerPlaceholderPanel");
            dungeonPlayerPlaceholderPanel = MonoBehaviour.Instantiate(dungeonPlayerPlaceholderPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            dungeonPlayerPlaceholderPanel.SetParent(canvas.transform, false);
            UiManager.dungeonPlayerPlaceholderPanel = dungeonPlayerPlaceholderPanel;
            UiManager.openPanel(UiManager.dungeonPlayerPlaceholderPanel);
            UiManager.addPanelToList(UiManager.dungeonPlayerPlaceholderPanel, UiManager.dungeonInitOnPanels);
        }
        else {
            dungeonPlayerPlaceholderPanel = UiManager.dungeonPlayerPlaceholderPanel;
        }

        Texture2D playerTexture = playerScript.getSelectedProfession().getTexture();
        GameObject playerImageGameObject = dungeonPlayerPlaceholderPanel.Find("PlayerImage").gameObject;
        playerImageGameObject.GetComponent<Image>().sprite = Sprite.Create(playerTexture, new Rect(0, 0, playerTexture.width, playerTexture.height), new Vector2(0.5f, 0.5f));

    }

    private void initDungeonRewardsPanel()
    {
        if (null == UiManager.dungeonRewardsPanel)
        {
            GameObject dungeonRewardsPanellGameObject = (GameObject)Resources.Load("Prefabs/DungeonRewardsPanel");
            dungeonRewardsPanel = MonoBehaviour.Instantiate(dungeonRewardsPanellGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            dungeonRewardsPanel.SetParent(canvas.transform, false);
            UiManager.dungeonRewardsPanel = dungeonRewardsPanel;
        }
        else {
            dungeonRewardsPanel = UiManager.dungeonRewardsPanel;
        }

        rewardsText = dungeonRewardsPanel.Find("RewardsTextPanel").Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        rewardsLootPanel = dungeonRewardsPanel.Find("LootPanel");
        rewardsActionsPanel = dungeonRewardsPanel.Find("ActionsPanel");

        UiManager.closePanel(UiManager.dungeonRewardsPanel);
    }


    public void updateDungeonButtons() {
         List<Ability> playerAbilities = playerScript.getAbilities();
         List<Item> playerItems = playerScript.getInventory().getItems();
         for(int i = 0; i < dungeonOptionsSlotsMaxCount; i++) {
            int slotNum = i;
            if(i < playerAbilities.Count) {
                Ability ability = playerAbilities[i];
                GameObject abilityButton = UiManager.Instance.CreateButton(dungeonOptionsButtonPanel, UiButton.ButtonType.DungeonOption, "" + slotNum, Item.Rarity.None, 
                                ability.getIcon(), () => selectPlayerAbility(ability), false, ability.getTooltip());
            }
            else if(i - playerAbilities.Count < playerItems.Count) {
                Item item = playerItems[i - playerAbilities.Count];
                GameObject itemButton = UiManager.Instance.CreateButton(dungeonOptionsButtonPanel, UiButton.ButtonType.DungeonOption, "" + slotNum, item.getRarity(), item.getIcon(), 
                                () => {
                                    if(playerScript.useItem(item)) {
                                        destroyButton(slotNum, dungeonOptionSlots);
                                    }
                                }, false, item.getTooltip());
            }
         }
    }

    private void destroyButton(int i, List<GameObject> slots) {
        if(null != slots[i] && slots[i].transform.childCount > 0 && null != slots[i].transform.GetChild(0)) {
            GameObject buttonToDestroy = slots[i].transform.GetChild(0).gameObject;
            GameObject.Destroy(buttonToDestroy);
        }
        else {
            //No button to destroy
            Debug.Log("No button to destroy for slot " + i);
        }
    }

    private void updateDungeonRewardsPanel() {

        //Update rewards text
        rewardsText.text = "Room " + getRoomNum() + "/" + getMaxRoomNum() + " completed, earned " + getExpFromThisRoom() + " exp and " + getGoldFromThisRoom() 
                    + " gold, as well as these items:";
        
        //Update rewards loot
        UiManager.clearExistingSlotsAndButtons(rewardsLootPanel);
        updateRewardsLoot();
        

        //Update rewards actions
        UiManager.clearExistingSlotsAndButtons(rewardsActionsPanel);
        updateStashActionButtions();
    }

    async void updateRewardsLoot()
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        List<Item> lootFromThisRoom = getLootFromThisRoom();
        rewardsLootSlots.Clear();
        //For each item loot, create a slot and a button to obtain the loot
        for(int i = 0; i < lootFromThisRoom.Count; i++) {
            int slotNum = i;
            Item item = lootFromThisRoom[i];
            
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), rewardsLootPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Item);
            rewardsLootSlots.Add(newSlot);
            GameObject newItem = UiManager.Instance.CreateButton(rewardsLootPanel, UiButton.ButtonType.Item, "", item.getRarity(), 
                                item.getIcon(), () => {
                                    if(playerScript.getInventory().addItem(item)) {
                                        Debug.Log("Looted item");
                                        lootFromThisRoom.Remove(item);
                                        destroyButton(slotNum, rewardsLootSlots);
                                    }
                                }, false, item.getTooltip());
        }
    }

    async void updateStashActionButtions()
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        //Stash button
        GameObject sendToSlashSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), rewardsActionsPanel);
        sendToSlashSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.DungeonMenuOption);
        UiManager.Instance.CreateButton(rewardsActionsPanel, UiButton.ButtonType.DungeonMenuOption, "Send to stash", Item.Rarity.None, null, 
                    () => {
                        Debug.Log("Send " + lootFromThisRoom.Count + " items to stash");
                        foreach(Item item in lootFromThisRoom) {
                            playerScript.getInventory().addStashItem(item);
                        }
                        lootFromThisRoom.Clear();
                        for(int i = 0; i < rewardsLootSlots.Count; i++) {
                            destroyButton(i, rewardsLootSlots);
                        }
                    }, false, null);

        //Back to town button
        GameObject backToTownSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), rewardsActionsPanel);
        backToTownSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.DungeonMenuOption);
        UiManager.Instance.CreateButton(rewardsActionsPanel, UiButton.ButtonType.DungeonMenuOption, "Go to town", Item.Rarity.None, null, goBackToTown, false, null);
        
        //Next room button, missing if we completed the last room already
        if(roomNumber != maxRooms) {
            GameObject nextRoomSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), rewardsActionsPanel);
            nextRoomSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.DungeonMenuOption);
            UiManager.Instance.CreateButton(rewardsActionsPanel, UiButton.ButtonType.DungeonMenuOption, "Next room", Item.Rarity.None, null, nextRoom, false, null);
        }
    }

    public void initDungeonRoom() {
        roomNumber++;

        //Clear the room tracking variables again for the next room
        goldFromThisRoom = 0;
        expFromThisRoom = 0;
        lootFromThisRoom = new List<Item>();

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
                int numberOfEnemies = UnityEngine.Random.Range(1, 3);
                enemies.AddRange(EnemyHandler.generateEnemies(numberOfEnemies, false));
            }
            else if(typesOfRooms[roomType] == "Treasure") {
                Debug.Log("Unimplemented treasure room :P");
            }
        }
        else {
            Debug.Log("No more rooms");
        }
        updateEnemyButtons();
        UiManager.openPanel(dungeonEnemiesPanel);
        UiManager.closePanel(dungeonRewardsPanel);
    }

    private void updateEnemyButtons() {
        //First clear existing enemy buttons
        UiManager.clearExistingSlotsAndButtons(dungeonEnemiesPanel);
        loadEnemyButtons();
    }

    async void loadEnemyButtons()
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        //Next create the slots and buttons for each enemy
        for(int i = 0; i < enemies.Count; i++) {
            EnemyScript enemy = enemies[i];
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), dungeonEnemiesPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Enemy);
            enemySlots.Add(newSlot);
            GameObject newEnemy = UiManager.Instance.CreateButton(dungeonEnemiesPanel, UiButton.ButtonType.Enemy, "", Item.Rarity.None, 
                                enemy.getTexture(), () => attackEnemy(enemy), false, enemy.getTooltip());
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

    public bool attackEnemy(EnemyScript enemy) {

        StartCoroutine(attackCoRoutine(enemy));
        return true;
    }

    private IEnumerator attackCoRoutine(EnemyScript enemy) {
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
            // if(enemy.isDead()) {
            //     enemies.Remove(enemy);
            //     enemies.Add(enemy);
            // }

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
                lootFromThisRoom.AddRange(ItemHandler.generateItems(enemy.getNumLoot(), enemy.getNumLoot(), null, playerScript.getDungeonFloorNum()));
            }
            else {
                lootFromThisRoom.AddRange(ItemHandler.generateItems(0, enemy.getNumLoot(), null, playerScript.getDungeonFloorNum()));
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
        
        UiManager.closePanel(dungeonEnemiesPanel);
        UiManager.openPanel(dungeonRewardsPanel);
        updateDungeonRewardsPanel();
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
