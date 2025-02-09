using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

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

    public bool characterMenuOpen, inventoryMenuOpen, mainMenuOpen;

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
        }

        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Character", Item.Rarity.None, (Texture2D)Resources.Load("Images/CharacterMenuIcon"), () => toggleMenu("Character"));
        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Inventory", Item.Rarity.None, (Texture2D)Resources.Load("Images/InventoryMenuIcon"), () => toggleMenu("Inventory"));
        UiManager.Instance.CreateButton(playerOptionsPanel, UiButton.ButtonType.PlayerMenuOption, "Main", Item.Rarity.None, (Texture2D)Resources.Load("Images/MainMenuIcon"), () => toggleMenu("Main"));
    }

    private void initCharacterSheetPanel()
    {
        if (null == characterSheetPanel)
        {
            GameObject characterSheetPanelGameObject = (GameObject)Resources.Load("Prefabs/CharacterSheetPanel");
            characterSheetPanel = MonoBehaviour.Instantiate(characterSheetPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            characterSheetPanel.SetParent(canvas.transform, false);
        }

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
        if(item.use()) {
            inventory.loseItem(item);
            return true;
        }
        return false;
    }

    public bool sellItem(Item item, ShopKeeperScript shopkeeper) {
        if(shopkeeper.buyItem(item)) {
            inventory.loseItem(item);
            gold += (item.getCost() / 2);
            return true;
        }
        return false;
    }

    public bool buyItem(Item item, int cost) {
        if(gold >= cost && inventory.addItem(item)) {
            gold -= cost;
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
        Debug.Log("Toggle: " + menu);
        if(menu == "Character") {
            toggleCharacterScreen();
        }
        else if(menu == "Inventory") {
            inventory.toggleInventory();
        }
        else if(menu == "Stash") {
            stashOpen = !stashOpen;
            if(stashOpen) {
                inventoryMenuOpen = true;
            }
        }
        else if(menu == "Main") {
            mainMenuOpen = !mainMenuOpen;
            characterMenuOpen = false;
            inventoryMenuOpen = false;
        }
    }

    public void toggleCharacterScreen() {
        characterSheetPanel.gameObject.SetActive(!characterSheetPanel.gameObject.activeSelf);
    }
    

    public void openMenu(String menu) {
        if(menu == "Character") {
            characterMenuOpen = true;
        }
        else if(menu == "Inventory") {
            inventoryMenuOpen = true;
        }
        else if(menu == "Main") {
            mainMenuOpen = true;
            characterMenuOpen = false;
            inventoryMenuOpen = false;
        }
    }

    public void closeMenu(String menu) {
        if(menu == "Character") {
            characterMenuOpen = false;
        }
        else if(menu == "Inventory") {
            inventoryMenuOpen = false;
        }
        else if(menu == "Main") {
            mainMenuOpen = false;
        }
    }

    public bool anyMenuOpen() {
        return characterMenuOpen || inventoryMenuOpen || mainMenuOpen;
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
