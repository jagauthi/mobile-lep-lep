using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    protected int DEFAULT_SPEED = 10;
    protected int DEFAULT_DAMAGE = 10;

    protected GameScript gameScript;
    public GameObject player, gameEngine;
    
    protected int currentHealth, currentResource, strength, intelligence, agility;
    protected int level = 1;
    protected int exp = 0;
    protected int skillPoints = 5;
    protected int gold;
    protected bool levelUpMenuToggle = false;
    protected bool stashOpen = false;
    protected string currentCrafting;

    int dungeonFloorNum, maxDungeonFloorNumCompleted;

    Hashtable abilityMap = new Hashtable();
    protected List<Ability> abilities;
    protected List<Quest> activeQuests;
    protected Inventory inventory;
    protected Equipment equipment;
    protected Weapon[] weapons;
    Transform playerOptionsPanel, characterSheetPanel;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    protected void Start ()
    {
        //Cursor.visible = false;
        Debug.Log("PlayerScript Start");
        basicInits();
        initStats();

        initPlayerOptionsPanel();
        initCharacterSheetPanel();

    }

    private void initPlayerOptionsPanel()
    {
        if (null == playerOptionsPanel)
        {
            GameObject playerOptionsPanelGameObject = (GameObject)Resources.Load("Prefabs/PlayerOptionsPanel");
            playerOptionsPanel = MonoBehaviour.Instantiate(playerOptionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas"); 
            playerOptionsPanel.SetParent(canvas.transform, false);
            UiManager.playerOptionsPanel = playerOptionsPanel;
            UiManager.openPanel(UiManager.playerOptionsPanel);
            UiManager.addPanelToList(UiManager.playerOptionsPanel, UiManager.playerInitOnPanels);
        }

        GameObject newSlot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), playerOptionsPanel);
        newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.PlayerMenuOption);
        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Character", Item.Rarity.None, (Texture2D)Resources.Load("Images/CharacterMenuIcon"), () => toggleMenu("Character"), false);

        GameObject newSlot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), playerOptionsPanel);
        newSlot2.GetComponent<UiSlot>().setType(UiButton.ButtonType.PlayerMenuOption);
        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Inventory", Item.Rarity.None, (Texture2D)Resources.Load("Images/InventoryMenuIcon"), () => toggleMenu("Inventory"), false);

        GameObject newSlot3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), playerOptionsPanel);
        newSlot3.GetComponent<UiSlot>().setType(UiButton.ButtonType.PlayerMenuOption);
        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Main", Item.Rarity.None, (Texture2D)Resources.Load("Images/MainMenuIcon"), () => toggleMenu("Main"), false);
    }

    private void initCharacterSheetPanel()
    {
        if (null == characterSheetPanel)
        {
            GameObject characterSheetPanelGameObject = (GameObject)Resources.Load("Prefabs/CharacterSheetPanel");
            characterSheetPanel = MonoBehaviour.Instantiate(characterSheetPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            characterSheetPanel.SetParent(canvas.transform, false);
            UiManager.characterSheetPanel = characterSheetPanel;
            UiManager.closeTownProfessionsPanels.Add(characterSheetPanel);
            UiManager.openPanel(UiManager.characterSheetPanel);
        }

        Transform statsPanel = characterSheetPanel.Find("StatsSection");

        GameObject pointsTextGameObject = statsPanel.Find("PointsText").gameObject;
        TextMeshProUGUI pointsText = pointsTextGameObject.GetComponent<TextMeshProUGUI>();
        UiManager.playerSkillPointsText = pointsText;

        List<GameObject> playerSkillButtons = new List<GameObject>();

        Transform strengthSection = statsPanel.Find("StrStat");
        TextMeshProUGUI strText = strengthSection.Find("StatText").gameObject.GetComponent<TextMeshProUGUI>();
        UiManager.playerStrText = strText;
        GameObject strButtonGO = strengthSection.Find("Button").gameObject;
        playerSkillButtons.Add(strButtonGO);
        Button strButton = strButtonGO.GetComponent<Button>();
        strButton.onClick.RemoveAllListeners();
        strButton.onClick.AddListener(() => {
            setStrength(getStrength() + 1);
            setSkillPoints(getSkillPoints() - 1);
            updatePlayerSkillsSection();
        }); 

        Transform intelSection = statsPanel.Find("IntelStat");
        TextMeshProUGUI intelText = intelSection.Find("StatText").gameObject.GetComponent<TextMeshProUGUI>();
        UiManager.playerIntelText = intelText;
        GameObject intelButtonGO = intelSection.Find("Button").gameObject;
        playerSkillButtons.Add(intelButtonGO);
        Button intelButton = intelButtonGO.GetComponent<Button>();
        intelButton.onClick.RemoveAllListeners();
        intelButton.onClick.AddListener(() => {
            setIntelligence(getIntelligence() + 1);
            setSkillPoints(getSkillPoints() - 1);
            updatePlayerSkillsSection();
        }); 

        Transform agilSection = statsPanel.Find("AgilStat");
        TextMeshProUGUI agilText = agilSection.Find("StatText").gameObject.GetComponent<TextMeshProUGUI>();
        UiManager.playerAgilText = agilText;
        GameObject agilButtonGO = agilSection.Find("Button").gameObject;
        playerSkillButtons.Add(agilButtonGO);
        Button agilButton = agilButtonGO.GetComponent<Button>();
        agilButton.onClick.RemoveAllListeners();
        agilButton.onClick.AddListener(() => {
            setAgility(getAgility() + 1);
            setSkillPoints(getSkillPoints() - 1);
            updatePlayerSkillsSection();
        }); 

        UiManager.playerSkillButtons = playerSkillButtons;

        updatePlayerSkillsSection();

        closeCharacterSheet();
    }

    private void updatePlayerSkillsSection() {

        //Skill points
        UiManager.playerSkillPointsText.text = "Points: " + skillPoints;
        foreach(GameObject skillButton in UiManager.playerSkillButtons) {
            if(skillPoints > 0) {
                skillButton.SetActive(true);
            }
            else {
                skillButton.SetActive(false);
            }
        }

        //Skill texts
        UiManager.playerStrText.text = "Strength: " + getStrength();
        UiManager.playerIntelText.text = "Intelligence: " + getIntelligence();
        UiManager.playerAgilText.text = "Agility: " + getAgility();
    }

    protected void basicInits()
    {
        if(null == player) {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        abilities = new List<Ability>();
        loadAbilities();

        activeQuests = new List<Quest>();
        inventory = new Inventory();

        equipment = new Equipment();
        inventory.addItem(ItemHandler.getItemMap()["Health Potion"]);
        inventory.addItem(ItemHandler.getItemMap()["Mana Potion"]);
        inventory.addItem(ItemHandler.getItemMap()["Iron Chest"]);

        gold = 10;

        dungeonFloorNum = 1;
        maxDungeonFloorNumCompleted = 0;

        retrieveGameScript();
    }

    public GameScript retrieveGameScript() {
        if(gameScript == null) {
            findGameScript();
        }
        return gameScript;
    }

    protected void findGameScript() {
        gameEngine = GameObject.FindGameObjectWithTag("GameEngine");
        if(gameEngine != null) {
            gameScript = gameEngine.GetComponent<GameScript>();
        }
    }

    protected void initStats()
    {
        strength = 1;
        intelligence = 1;
        agility = 1;
        currentHealth = getMaxHealth();
        currentResource = getMaxResource();

        currentHealth -= 50;
    }

    protected virtual void loadAbilities()
    {
        initAbilities();
        abilities.Add((Ability)abilityMap["Double Hammer Strike"]);
        abilities.Add((Ability)abilityMap["Melee Attack"]);
    }

    void initAbilities()
    {
        abilityMap.Add("Melee Attack", new Ability("Melee Attack", "Melee", 0, 10,
                (Texture2D)Resources.Load("Images/WeaponIcon")));
                
        abilityMap.Add("Double Hammer Strike", new Ability("Double Hammer Strike", "Melee", 10, 40,
                (Texture2D)Resources.Load("Images/DoubleHammerStrike")));

        abilityMap.Add("Ranged Attack", new Ability("Arrow", "RangedProjectile", 10, 40,
                (Texture2D)Resources.Load("Images/ArrowIcon")));

        abilityMap.Add("Fireball", new Ability("Fireball", "MagicProjectile", 10, 40,
                (Texture2D)Resources.Load("Images/FireballIcon")));

        abilityMap.Add("Frostball", new Ability("Frostball", "MagicProjectile", 5, 20,
                (Texture2D)Resources.Load("Images/FrostballIcon")));
    }

    protected void Update() {
       basicUpdates();
    }

    protected void basicUpdates() {

    }

    void OnDestroy()
    {

    }

    protected void OnGUI(){
        // drawBasics();

        // GuiUtil.drawPlayerMenuOptions(this);  

        // if(characterMenuOpen) {
        //     GuiUtil.characterMenu(this);
        // }    
        // if(inventoryMenuOpen) {
        //     GuiUtil.playerInventoryMenu(this, null, isStashOpen());
        // }  
        // if(stashOpen) {
        //     GuiUtil.playerStashMenu(this);
        // }
        // if(mainMenuOpen) {
        //     GuiUtil.mainMenu();
        // }  
    }

    protected void drawBasics() {
        GuiUtil.drawHealthBar(currentHealth, getMaxHealth(), level);
        GuiUtil.drawExpBar(exp, GetExpToNextLevel(), skillPoints);
        GuiUtil.drawResourceBar(currentResource, getMaxResource());
        //drawAbilities();
    }

    protected void drawAbilities()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            GUI.DrawTexture(
                new Rect((i * Screen.width / 16) + 20, 7 * Screen.height / 8- 20, Screen.height / 8, Screen.height / 8),
                abilities[i].getIcon()
            );
        }
    }

    public void loseHealth(int x)
    {
        float armorBlock = 0;
        foreach(Item item in equipment.getItems())
        {
            if(null != item && item.getType() == "Armor") {
                armorBlock += ((Armor)item).getArmorPower();
            }
        }
        armorBlock = 1 - (armorBlock / 100);
        float newDamage = x * armorBlock;
        // Debug.Log("Armor block: " + armorBlock);
        // Debug.Log("Old damage: " + x + ", New Damage: " + newDamage);
        currentHealth -= (int)newDamage;
    }

    public bool isDead() {
        return currentHealth <= 0;
    }

    protected int getMaxHealth()
    {
        return 100 + (level * 20) + (strength*10);
    }

    protected virtual int getMaxResource() {
        return 100 + (intelligence*20);
    }

    public bool gainHealth(int health) { 
        if(currentHealth >= getMaxHealth()) {
            return false;
        }
        else {
            currentHealth += health;
            if(currentHealth >= getMaxHealth()) {
                fullHeal();
            }
            return true;
        }
    }

    public void gainExp(int x)
    {
        exp += x;
        while (exp >= GetExpToNextLevel())
        {
            int leftoverXP = exp - GetExpToNextLevel();
            LevelUp();
            exp += leftoverXP;
        }
    }

    public bool gainResource(int x) { 
        if(currentResource >= getMaxResource()) {
            return false;
        }
        else {
            currentResource += x;
            if(currentResource > getMaxResource()) {
                currentResource = getMaxResource();
            }
            return true;
        }
    }

    public bool loseResource(int x) {
        if(currentResource >= x) {
            currentResource -= x;
            return true;
        }
        else {
            return false;
        }
    }

    public void setStrength(int newStrength) {
        this.strength = newStrength;
    }

    public void setIntelligence(int intelligence) {
        this.intelligence = intelligence;
    }

    public void setAgility(int agility) {
        this.agility = agility;
    }

    public void setSkillPoints(int skillPoints) {
        this.skillPoints = skillPoints;
    }

    public int getSkillPoints() {
        return skillPoints;
    }

    public int getStrength() {
        return strength;
    }

    public int getIntelligence() {
        return intelligence;
    }

    public int getAgility() {
        return agility;
    }

    public int getGold() {
        return gold;
    }

    public void completeQuest(Quest quest) {
        removeQuest(quest);
        gainExp(quest.expReward);
        gainGold(quest.goldReward);
    }

    public void removeQuest(Quest quest) {
        activeQuests.Remove(quest);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void fullHeal() {
        this.currentHealth = this.getMaxHealth();
    }

    protected int GetExpToNextLevel()
    {
        return (level * 50);
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public void gainGold(int x) {
        this.gold += x;
    }

    public void loseGold(int x) {
        if(this.gold - x <= 0) {
            this.gold = 0;
        }
        else {
            this.gold -= x;
        }
    }

    public bool getLevelupToggle() {
        return levelUpMenuToggle;
    }

    public void setLevelupToggle(bool levelUpMenuToggle) {
        this.levelUpMenuToggle = levelUpMenuToggle;
    }

    public Inventory getInventory() {
        return inventory;
    }

    protected void LevelUp()
    {
        level++;
        skillPoints += 5;
        currentHealth = getMaxHealth();
        currentResource = getMaxResource();
        exp = 0;
    }

    public void addQuest(Quest quest) {
        activeQuests.Add(quest);
    }

    public bool hasQuest(Quest quest) {
        return activeQuests.Contains(quest); 
    }

    public bool useItem(Item item) {
        //If the shopkeeper is open, we should try to sell to them
        if(UiManager.isShopkeeperPanelOpen()) {
            ShopKeeperScript shopkeeper = getShopkeeper();
            return sellItem(item, shopkeeper);
        }
        else if(UiManager.isStashOpen()) {
            return inventory.transferFromInventoryToStash(item);
        }
        //Otherwise if it's just in our inventory, use the item
        else {
            if(item.use()) {
                inventory.loseItem(item);
                return true;
            }
        }
        Debug.Log("Didn't use item?");
        return false;
    }

    public ShopKeeperScript getShopkeeper() {
        if(null != GameObject.FindGameObjectWithTag("Town")) {
            return GameObject.FindGameObjectWithTag("Town").GetComponent<TownScript>().getShopkeeper();
        }
        return null;
    }

    public bool sellItem(Item item, ShopKeeperScript shopkeeper) {
        if(shopkeeper.buyItem(item)) {
            inventory.loseItem(item);
            gold += (item.getCost() / 2);
            return true;
        }
        return false;
    }

    public bool buyItem(Item item, int cost, ShopKeeperScript shopkeeper) {
        if(gold >= cost && inventory.addItem(item)) {
            gold -= cost;
            shopkeeper.loseItem(item);
            return true;
        }
        else {
            Debug.Log("Either not enough gold or not enough inventory space");
            return false;
        }
    }

    public Equipment getEquipment() {
        return equipment;
    }

    public Armor getArmorSlot(string slot) {
        return (Armor)equipment.getItemMap()[slot];
    }

    public List<Ability> getAbilities() {
        return abilities;
    }

    public bool useAbility(Ability ability) {
        return loseResource(ability.getResourceCost());
    }

    public void toggleMenu(String menu) {
        if(menu == "Character") {
            UiManager.togglePanel(characterSheetPanel);
        }
        else if(menu == "Inventory") {
            //inventory.toggleInventory();
            UiManager.togglePanel(inventory.playerInventoryPanel);
        }
        else if(menu == "Stash") {
            // UiManager.Instance.togglePanel(inventory.playerStashPanel);
        }
        else if(menu == "Main") {
            // UiManager.Instance.togglePanel(inventory.mainMenuPanel);
        }
    }

    public void closeCharacterSheet() {
        UiManager.closePanel(characterSheetPanel);
        // characterSheetPanel.gameObject.SetActive(false);
    }
    

    public bool isStashOpen() {
        return stashOpen;
    }

    public void setDungeonFloor(int floorNum) {
        dungeonFloorNum = floorNum;
    }

    public int getDungeonFloorNum() {
        return dungeonFloorNum;
    }

    public void setMaxDungeonFloorNumCompleted(int floorNum) {
        maxDungeonFloorNumCompleted = floorNum;
    }

    public int getMaxDungeonFloorNumCompleted() {
        return maxDungeonFloorNumCompleted;
    }
    
    public string getCurrentCrafting() {
        return currentCrafting;
    }

    public void setCurrentCrafting(string crafting) {
        this.currentCrafting = crafting;
    }
}
