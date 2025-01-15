using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{

    protected int DEFAULT_SPEED = 10;
    protected int DEFAULT_DAMAGE = 10;

    protected GameScript gameScript;
    public GameObject player, gameEngine;
    
    protected int currentHealth, strength, intelligence, agility;
    
    protected float regularBarLength = Screen.width / 3;
    protected float healthBarLength = Screen.width / 3;
    protected float expBarLength = Screen.width / 3;
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

    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    protected void Start ()
    {
        //Cursor.visible = false;
        Debug.Log("Start");
        basicInits();
        initStats();
    }

    protected void basicInits()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        abilities = new List<Ability>();
        loadAbilities();
        activeQuests = new List<Quest>();
        inventory = new Inventory();
        equipment = new Equipment();
        inventory.addItem(new Consumable(0, "Health Potion", "Heal", (Texture2D)Resources.Load("Images/HealthPotion"), 50));
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
        currentHealth = 100;
        strength = 1;
        intelligence = 1;
        agility = 1;
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
        healthBarLength = (regularBarLength) * (currentHealth / (float)getMaxHealth());
        expBarLength = (regularBarLength) * (exp / (float)GetExpToNextLevel());
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

    public void levelUpToggle() {
        if(!gameScript.getMenuOpen()) {
            gameScript.setMenuOpen(true);
            levelUpMenuToggle = true;
        }
        else if(inventory.isOpen()) {
            levelUpMenuToggle = !levelUpMenuToggle;
        }
        else if(levelUpMenuToggle) {
            gameScript.setMenuOpen(false);
            levelUpMenuToggle = false;
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
    }

    protected void drawBasics() {
        drawHealthBar();
        drawExpBar();
        drawAbilities();
    }

    protected void drawHealthBar()
    {
        GUIStyle redStyle = new GUIStyle(GUI.skin.box);
        redStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.75f));
        GUIStyle blackStyle = new GUIStyle(GUI.skin.box);
        blackStyle.normal.background = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
        if (healthBarLength > 0)
        {
            GUI.Box(new Rect(10, 10, healthBarLength, 20), "", redStyle);
        }
        GUI.Box(new Rect(10, 10, regularBarLength, 20), currentHealth + "/" + getMaxHealth());
        GUI.Box(new Rect(regularBarLength + 20, 10, 20, 20), "" + level);
    }

    protected void drawExpBar()
    {
        GUIStyle greenStyle = new GUIStyle(GUI.skin.box);
        greenStyle.normal.background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.75f));
        if (expBarLength > 0)
        {
            GUI.Box(new Rect(10, 35, expBarLength, 20), "", greenStyle);
        }
        GUI.Box(new Rect(10, 35, regularBarLength, 20), exp + "/" + GetExpToNextLevel());
        if(skillPoints > 0)
        {
            GUI.Box(new Rect(regularBarLength + 20, 35, 20, 20), "+");
        }
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

    protected void drawEnemyHealthBar(EnemyScript enemyScript)
    {
        if (enemyScript != null)
        {
            GUIStyle redStyle = new GUIStyle(GUI.skin.box);
            redStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, 0.75f));
            float healthBarLength = (regularBarLength) * (enemyScript.currentHealth / (float)enemyScript.maxHealth);
            if (healthBarLength > 0)
            {
                GUI.Box(new Rect(regularBarLength + 50, 10, healthBarLength, 20), "", redStyle);
            }
            GUI.Box(new Rect(regularBarLength + 50, 10, regularBarLength, 20), enemyScript.currentHealth + "/" + enemyScript.maxHealth);
        }
    }

    protected Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    protected void loseHealth(int x)
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
        Debug.Log("Armor block: " + armorBlock);
        Debug.Log("Old damage: " + x + ", New Damage: " + newDamage);
        currentHealth -= (int)newDamage;
    }

    protected bool loseResource(int x)
    {
        //To be implemented by children classes
        return false;
    }

    protected virtual void fillResource() {
        print("Ugh?");
        //To be implemented by children classes
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

    protected void gainExp(int x)
    {
        exp += x;
        while (exp >= GetExpToNextLevel())
        {
            int leftoverXP = exp - GetExpToNextLevel();
            LevelUp();
            exp += leftoverXP;
        }
    }

    public virtual bool gainResource(int x) { 
        return false;
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
        fillResource();
        exp = 0;
    }

    public void addQuest(Quest quest) {
        activeQuests.Add(quest);
    }

    public bool hasQuest(Quest quest) {
        return activeQuests.Contains(quest); 
    }

    public void useItem(Item item) {
        if(item.use()) {
            inventory.loseItem(item);
        }
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
}
