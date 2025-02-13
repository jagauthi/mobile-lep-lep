using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownScript : MonoBehaviour
{
   public GameObject playerGameObject;

   public PlayerScript playerScript;
   protected List<NpcScript> npcs; 
   ShopKeeperScript shopkeeper;
   TownProfessionNpc selectedProfession;
   Transform townOptionsButtonPanel, townProfessionsPanel, npcDialogPanel;
   private int dungeonFloorsPage = 0;

   private GameObject dungeonFloorsPanelGameObject


    void Start()
    {
        Debug.Log("TownScript start");
        getPlayerScript();

        shopkeeper = new ShopKeeperScript("Shopkeeper Ben", (Texture2D)Resources.Load("Images/LeatherHelm"));

        TownProfessionNpc blacksmith = new TownProfessionNpc("Sarah the Blacksmith", (Texture2D)Resources.Load("Images/SawahBlacksmith1"), (Texture2D)Resources.Load("Images/SawahBlacksmithHeadshot"));

        npcs = new List<NpcScript>();
        npcs.Add(shopkeeper);
        npcs.Add(blacksmith);

        initTownOptionsPanel();
        initTownProfessionsPanel();
        initNpcDialogPanel();

    }

    private void initTownProfessionsPanel()
    {
        if (null == townProfessionsPanel)
        {
            GameObject townProfessionsPanelGameObject = (GameObject)Resources.Load("Prefabs/TownProfessionsPanel");
            townProfessionsPanel = MonoBehaviour.Instantiate(townProfessionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            townProfessionsPanel.SetParent(canvas.transform, false);
            UiManager.townProfessionsPanel = townProfessionsPanel;
        }

        foreach (NpcScript npc in npcs)
        {
            GameObject newSlot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), townProfessionsPanel);
            newSlot2.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
            UiManager.Instance.CreateButton(townProfessionsPanel, UiButton.ButtonType.TownMenuOption, npc.getName(), Item.Rarity.None, npc.getTexture(), () => npc.startInteraction(this));
        }
    }

    private void initTownOptionsPanel()
    {
        if (null == townOptionsButtonPanel)
        {
            GameObject townOptionsPanelGameObject = (GameObject)Resources.Load("Prefabs/TownOptionsPanel");
            townOptionsButtonPanel = MonoBehaviour.Instantiate(townOptionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            townOptionsButtonPanel.SetParent(canvas.transform, false);
            UiManager.townOptionsButtonPanel = townOptionsButtonPanel;
        }

        GameObject newSlot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), townOptionsButtonPanel);
        newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
        UiManager.Instance.CreateButton(townOptionsButtonPanel, UiButton.ButtonType.TownMenuOption, "Stash", Item.Rarity.None, (Texture2D)Resources.Load("Images/StashMenuIcon"), () => playerScript.toggleMenu("Stash"));
    }

    private void initNpcDialogPanel()
    {
        GameObject npcDialogPanelGameObject = (GameObject)Resources.Load("Prefabs/NpcDialogPanel");
        npcDialogPanel = MonoBehaviour.Instantiate(npcDialogPanelGameObject).GetComponent<Transform>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        npcDialogPanel.SetParent(canvas.transform, false);
        UiManager.npcDialogPanel = npcDialogPanel;
        UiManager.closeTownProfessionsPanels.Add(npcDialogPanel);
        
        UiManager.closePanel(npcDialogPanel);
    }

    public void setupNpcDialogPanel(TownProfessionNpc selectedProfession) {


//////////////////////
        //TODO: Put these things either in this class or in UI Manager so that they can be enabled/disabled at differnet parts of the NPC dialog
        GameObject npcIconGameObject, textPanelGameObject, buttonOptionsPanelGameObject, closeButtonGameObject, dungeonFloorsPanelGameObject, dungeonFloorsTextPanel, dungeonFloorsUpButton, dungeonFloorsDownButton
//////////////////////

        UiManager.togglePanel(npcDialogPanel);

        //NPC icon
        GameObject npcIconGameObject = npcDialogPanel.Find("NpcIcon").gameObject;
        Rect iconRect = npcIconGameObject.GetComponent<RectTransform>().rect;
        npcIconGameObject.GetComponent<Image>().sprite = Sprite.Create(selectedProfession.getHeadShot(), new Rect(0, 0, selectedProfession.getTexture().width, selectedProfession.getTexture().height), new Vector2(0.5f, 0.5f));


        //Text panel
        GameObject textPanelGameObject = npcDialogPanel.Find("TextPanel").gameObject;
        Transform textPanel = textPanelGameObject.transform;
        Text text = textPanel.transform.Find("Text").gameObject.GetComponent<Text>();
        text.text = selectedProfession.getTownDialog();


        //Button options
        GameObject buttonOptionsPanelGameObject = npcDialogPanel.Find("ButtonOptions").gameObject;
        Transform buttonOptionsPanel = buttonOptionsPanelGameObject.transform;
        //Remove existing buttons
        for(int i = 0; i < buttonOptionsPanel.childCount; i++) {
            GameObject childGameObject = buttonOptionsPanel.GetChild(i).gameObject;
            GameObject.Destroy(childGameObject);
        }
        //Dungeon button
        GameObject newSlot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), buttonOptionsPanel);
        newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
        UiManager.Instance.CreateButton(buttonOptionsPanel, UiButton.ButtonType.TownMenuOption, "Dungeon", Item.Rarity.None, null, () => startDungeon());
        //Crafting button
        GameObject newSlot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), buttonOptionsPanel);
        newSlot2.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
        UiManager.Instance.CreateButton(buttonOptionsPanel, UiButton.ButtonType.TownMenuOption, "Crafting", Item.Rarity.None, null, () => startCrafting());
        
        
        //Close button
        GameObject closeButtonGameObject = npcDialogPanel.Find("CloseButton").gameObject;
        Button button = closeButtonGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => UiManager.closePanel(npcDialogPanel));


        /**
        These things below will be disabled at startup, and will be enabled later if dungeon dialog option is selected
        **/

        //Dungeon Floors Selection Buttons
        GameObject dungeonFloorsPanelGameObject = npcDialogPanel.Find("DungeonFloorsPanel").gameObject;
        Transform dungeonFloorsPanel = dungeonFloorsPanelGameObject.transform;
        for(int i = 0; i < 5; i++) {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), dungeonFloorsPanel);
            newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
            UiManager.Instance.CreateButton(dungeonFloorsPanel, UiButton.ButtonType.TownMenuOption, "Dungeon", Item.Rarity.None, null, () => selectDungeonFloor(i));
        }


        //Dungeon Floors Text Panle
        GameObject dungeonFloorsTextPanel = npcDialogPanel.Find("DungeonFloorsTextPanel").gameObject;
        Text dungeonFloorsText = textPanel.transform.Find("Text").gameObject.GetComponent<Text>();
        dungeonFloorsText.text = "Which floor?";


        //Dungeon Floors Pagination Buttons
        GameObject dungeonFloorsUpButton = npcDialogPanel.Find("DungeonFloorsUpButton").gameObject;
        Button upButton = dungeonFloorsUpButton.GetComponent<Button>();
        upButton.onClick.AddListener(() => moveDungeonFloorPage(1));

        GameObject dungeonFloorsDownButton = npcDialogPanel.Find("DungeonFloorsDownButton").gameObject;
        Button downButton = dungeonFloorsDownButton.GetComponent<Button>();
        downButton.onClick.AddListener(() => moveDungeonFloorPage(-1));
    }

    private void selectDungeonFloor(int floorNum) {
        int adjustedFloor = floorNum + (dungeonFloorsPage * 5);
        Debug.Log("Selected floor: " + adjustedFloor);
    }

    private void moveDungeonFloorPage(int direction) {
        dungeonFloorsPage += direction;
    }

    private void getPlayerScript() {
        if(null == playerGameObject) {
            // Debug.Log("PlayerGameObject null");
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if(null == playerGameObject) {
                // Debug.Log("Still didn't find it");
                playerGameObject = (GameObject)Resources.Load("Prefabs/PlayerGameObject");
                playerScript = Instantiate(playerGameObject).GetComponent<PlayerScript>();
            }
            else {
                playerScript = playerGameObject.GetComponent<PlayerScript>();
            }
        }
    }
    
    protected void OnGUI(){
        // drawTownThings();   
    }

    protected void drawTownThings() { 
        
        //If player has any menus open, we dont want to show most town options
        // if(playerScript.anyMenuOpen()) {
        //     return;
        // }

        GuiUtil.drawTownOptions(playerScript);
        
        //Otherwise if shopkeeper has inventory open, show that
        // if(shopkeeper.showingInventory) {
        //     GuiUtil.shopkeeperMenu(shopkeeper, playerScript);

        //     //Displaying the player inventory which allows to sell items
        //     GuiUtil.playerInventoryMenu(playerScript, shopkeeper, false);
        // }
        //Show the other NPCs if the shopkeeper isn't open
        // else {
        //     if(null == selectedProfession) {
        //         GuiUtil.drawNpcs(npcs, this);
        //     }
        //     else {
        //         GuiUtil.professionDialog(selectedProfession, playerScript, this);
        //     }
        // }
    }

    public void setSelectedProfession(TownProfessionNpc townProfessionNpc) {
        this.selectedProfession = townProfessionNpc;
    }

    public void startDungeon() {
        if(playerScript.isDead()) {
            Debug.Log("Can't load dungeon when you're dead!");
        }
        else {
            SceneManager.LoadScene("DungeonScene");
        }
    }

    public void startCrafting() {
        if(playerScript.isDead()) {
            Debug.Log("Can't craft when you're dead!");
        }
        else {
            SceneManager.LoadScene("CraftingScene");
        }
    }

    public ShopKeeperScript getShopkeeper() {
        return shopkeeper;
    }
    

}
