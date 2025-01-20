using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

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

    Hashtable abilityMap = new Hashtable();
    protected List<Ability> abilities;
    protected List<Quest> activeQuests;
    protected Inventory inventory;
    protected Equipment equipment;
    protected Weapon[] weapons;

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
        inventory.addItem(new Consumable(0, "Health Potion", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 50));
        inventory.addItem(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));
        inventory.addItem(new Consumable(0, "Mana Potion", "ResourceHeal", (Texture2D)Resources.Load("Images/ManaPotion"), 50));

        gold = 10;

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
    }

    protected virtual void loadAbilities()
    {
        initAbilities();
        abilities.Add((Ability)abilityMap["Fireball"]);
        abilities.Add((Ability)abilityMap["Frostball"]);
        abilities.Add((Ability)abilityMap["Melee Attack"]);
    }

    void initAbilities()
    {
        abilityMap.Add("Melee Attack", new Ability("Melee Attack", "Melee", 5, 30,
                (GameObject)Resources.Load("Prefabs/Weapon"),
                (Texture2D)Resources.Load("Images/WeaponIcon")));

        abilityMap.Add("Ranged Attack", new Ability("Arrow", "RangedProjectile", 10, 40,
                (GameObject)Resources.Load("Prefabs/Arrow"),
                (Texture2D)Resources.Load("Images/ArrowIcon")));

        abilityMap.Add("Fireball", new Ability("Fireball", "MagicProjectile", 10, 40,
                (GameObject)Resources.Load("Prefabs/Fireball"),
                (Texture2D)Resources.Load("Images/FireballIcon")));

        abilityMap.Add("Frostball", new Ability("Frostball", "MagicProjectile", 5, 20,
                (GameObject)Resources.Load("Prefabs/Frostball"),
                (Texture2D)Resources.Load("Images/FrostballIcon")));
    }

    protected void Update() {
       basicUpdates();
    }

    protected void basicUpdates() {

    }

    public void inventoryToggle() {
        if(!gameScript.getMenuOpen()) {
            gameScript.setMenuOpen(true);
            inventory.toggle();
        }
        else if(levelUpMenuToggle) {
            inventory.toggle();
        }
        else if(inventory.isOpen()) {
            gameScript.setMenuOpen(false);
            inventory.toggle();
        }
    }

    protected void closeMenus() {
        levelUpMenuToggle = false;
        inventory.close();
    }

    void OnDestroy()
    {

    }

    protected void OnGUI(){
        drawBasics();      
        if(characterMenuOpen) {
            GuiUtil.characterMenu(this);
        }    
        if(inventoryMenuOpen) {
            GuiUtil.inventoryMenu(this);
        }  
        if(mainMenuOpen) {
            GuiUtil.mainMenu();
        }  
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

    protected virtual void fillResource() {
        print("Ugh?");
        //To be implemented by children classes (maybe?)
    }

    protected int getMaxHealth()
    {
        return 100 + (level * 20) + (strength*10);
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

    public void completeQuest(Quest quest) {
        removeQuest(quest);
        gainExp(quest.expReward);
        gainGold(quest.goldReward);
        //TODO: Do something with gold reward
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

    protected int GetMaxHealth()
    {
        return 50 + (10 * level) + (10 * strength);
    }

    protected virtual int getMaxResource() {
        return 100;
    }

    protected int GetExpToNextLevel()
    {
        return (level * 50);
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
        if(menu == "Character") {
            characterMenuOpen = !characterMenuOpen;
        }
        else if(menu == "Inventory") {
            inventoryMenuOpen = !inventoryMenuOpen;
        }
        else if(menu == "Main") {
            mainMenuOpen = !mainMenuOpen;
            characterMenuOpen = false;
            inventoryMenuOpen = false;
        }
    }
    public bool anyMenuOpen() {
        return characterMenuOpen || inventoryMenuOpen || mainMenuOpen;
    }
}
