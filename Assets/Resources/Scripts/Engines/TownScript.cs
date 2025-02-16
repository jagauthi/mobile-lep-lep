using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownScript : MonoBehaviour
{
    public GameObject canvas;
   public GameObject playerGameObject;

   public PlayerScript playerScript;
   protected List<NpcScript> npcs; 
   ShopKeeperScript shopkeeper;
   TownProfessionNpc selectedProfession;
   Transform townOptionsButtonPanel, townProfessionsPanel, npcDialogPanel;
   private int dungeonFloorsPage = 0;

    void Awake()
    {
        if(null == canvas) {
            canvas = GameObject.FindGameObjectWithTag("Canvas");
            if(null == canvas) {
                canvas = (GameObject)Resources.Load("Prefabs/Canvas");
                Instantiate(canvas);
            }
        }
    }

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
        if (null == UiManager.townProfessionsPanel)
        {
            GameObject townProfessionsPanelGameObject = (GameObject)Resources.Load("Prefabs/TownProfessionsPanel");
            townProfessionsPanel = MonoBehaviour.Instantiate(townProfessionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            townProfessionsPanel.SetParent(canvas.transform, false);
            UiManager.townProfessionsPanel = townProfessionsPanel;
            UiManager.openPanel(UiManager.townProfessionsPanel);
            UiManager.addPanelToList(UiManager.townProfessionsPanel, UiManager.townInitOnPanels);

            foreach (NpcScript npc in npcs)
            {
                GameObject newSlot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), townProfessionsPanel);
                newSlot2.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
                UiManager.Instance.CreateButton(townProfessionsPanel, UiButton.ButtonType.TownMenuOption, npc.getName(), Item.Rarity.None, npc.getTexture(), () => npc.startInteraction(this), false);
            }
        }
        else {
            townProfessionsPanel = UiManager.townProfessionsPanel;
        }
    }

    private void initTownOptionsPanel()
    {
        if (null == UiManager.townOptionsButtonPanel)
        {
            GameObject townOptionsPanelGameObject = (GameObject)Resources.Load("Prefabs/TownOptionsPanel");
            townOptionsButtonPanel = MonoBehaviour.Instantiate(townOptionsPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            townOptionsButtonPanel.SetParent(canvas.transform, false);
            UiManager.townOptionsButtonPanel = townOptionsButtonPanel;
            UiManager.openPanel(UiManager.townOptionsButtonPanel);
            UiManager.addPanelToList(UiManager.townOptionsButtonPanel, UiManager.townInitOnPanels);

            GameObject newSlot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), townOptionsButtonPanel);
            newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
            UiManager.Instance.CreateButton(townOptionsButtonPanel, UiButton.ButtonType.TownMenuOption, "Stash", Item.Rarity.None, (Texture2D)Resources.Load("Images/StashMenuIcon"), () => UiManager.togglePanel(UiManager.playerStashPanel), false);
        }
        else {
            townOptionsButtonPanel = UiManager.townOptionsButtonPanel;
        }
    }

    private void initNpcDialogPanel()
    {
        if (null == UiManager.npcDialogPanel)
        {
            GameObject npcDialogPanelGameObject = (GameObject)Resources.Load("Prefabs/NpcDialogPanel");
            npcDialogPanel = MonoBehaviour.Instantiate(npcDialogPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            npcDialogPanel.SetParent(canvas.transform, false);
            UiManager.npcDialogPanel = npcDialogPanel;
            UiManager.closeTownProfessionsPanels.Add(npcDialogPanel);
            
            UiManager.closePanel(npcDialogPanel);
        }
        else {
            npcDialogPanel = UiManager.npcDialogPanel;
        }
    }

    public void setupNpcDialogPanel(TownProfessionNpc selectedProfession)
    {

        UiManager.togglePanel(npcDialogPanel);

        //NPC icon
        GameObject npcIconGameObject = npcDialogPanel.Find("NpcIcon").gameObject;
        UiManager.npcIconGameObject = npcIconGameObject;
        Rect iconRect = npcIconGameObject.GetComponent<RectTransform>().rect;
        npcIconGameObject.GetComponent<Image>().sprite = Sprite.Create(selectedProfession.getHeadShot(), new Rect(0, 0, selectedProfession.getTexture().width, selectedProfession.getTexture().height), new Vector2(0.5f, 0.5f));


        //Text panel
        GameObject textPanelGameObject = npcDialogPanel.Find("TextPanel").gameObject;
        UiManager.textPanelGameObject = textPanelGameObject;
        Transform textPanel = textPanelGameObject.transform;
        Text text = textPanel.transform.Find("Text").gameObject.GetComponent<Text>();
        text.text = selectedProfession.getTownDialog();


        //Button options
        GameObject buttonOptionsPanelGameObject = npcDialogPanel.Find("ButtonOptions").gameObject;
        UiManager.buttonOptionsPanelGameObject = buttonOptionsPanelGameObject;
        Transform buttonOptionsPanel = buttonOptionsPanelGameObject.transform;
        //Remove existing buttons
        UiManager.clearExistingSlotsAndButtons(buttonOptionsPanel);
        //Dungeon button
        GameObject newSlot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), buttonOptionsPanel);
        newSlot1.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
        UiManager.Instance.CreateButton(buttonOptionsPanel, UiButton.ButtonType.TownMenuOption, "Dungeon", Item.Rarity.None, null, () => {
            enableDungeonFloorsSelection();
            }, false);
        //Crafting button
        GameObject newSlot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), buttonOptionsPanel);
        newSlot2.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
        UiManager.Instance.CreateButton(buttonOptionsPanel, UiButton.ButtonType.TownMenuOption, "Crafting", Item.Rarity.None, null, () => startCrafting(), false);


        //Close button
        GameObject closeButtonGameObject = npcDialogPanel.Find("CloseButton").gameObject;
        UiManager.closeButtonGameObject = closeButtonGameObject;
        Button button = closeButtonGameObject.GetComponent<Button>();
        button.onClick.AddListener(() => UiManager.closePanel(npcDialogPanel));


        /**
        These things below will be disabled at startup, and will be enabled later if dungeon dialog option is selected
        **/

        //Dungeon Floors Selection Buttons
        GameObject dungeonFloorsPanelGameObject = npcDialogPanel.Find("DungeonFloorsPanel").gameObject;
        UiManager.dungeonFloorsPanelGameObject = dungeonFloorsPanelGameObject;
        setupDungeonFloorButtons(dungeonFloorsPanelGameObject);


        //Dungeon Floors Text Panle
        GameObject dungeonFloorsTextPanel = npcDialogPanel.Find("DungeonFloorsTextPanel").gameObject;
        UiManager.dungeonFloorsTextPanel = dungeonFloorsTextPanel;
        Text dungeonFloorsText = dungeonFloorsTextPanel.transform.Find("Text").gameObject.GetComponent<Text>();
        dungeonFloorsText.text = "Which floor?";


        //Dungeon Floors Pagination Buttons
        //Up Button
        GameObject dungeonFloorsUpButton = npcDialogPanel.Find("DungeonFloorsUpButton").gameObject;
        UiManager.dungeonFloorsUpButton = dungeonFloorsUpButton;
        Button upButton = dungeonFloorsUpButton.GetComponent<Button>();
        upButton.onClick.AddListener(() => moveDungeonFloorPage(-1));
        //Down button
        GameObject dungeonFloorsDownButton = npcDialogPanel.Find("DungeonFloorsDownButton").gameObject;
        UiManager.dungeonFloorsDownButton = dungeonFloorsDownButton;
        Button downButton = dungeonFloorsDownButton.GetComponent<Button>();
        downButton.onClick.AddListener(() => moveDungeonFloorPage(1));


        //Enable the panels that should be available at the start, and disable the rest
        UiManager.enablePanel(UiManager.textPanelGameObject);
        UiManager.enablePanel(UiManager.buttonOptionsPanelGameObject);
        UiManager.disablePanel(UiManager.dungeonFloorsPanelGameObject);
        UiManager.disablePanel(UiManager.dungeonFloorsTextPanel);
        UiManager.disablePanel(UiManager.dungeonFloorsUpButton);
        UiManager.disablePanel(UiManager.dungeonFloorsDownButton);
    }

    private void setupDungeonFloorButtons(GameObject dungeonFloorsPanelGameObject)
    {
        Transform dungeonFloorsPanel = dungeonFloorsPanelGameObject.transform;
        
        //First clear existing buttons
        UiManager.clearExistingSlotsAndButtons(dungeonFloorsPanel);

        //Then set up the new buttons
        int maxDungeonFloorNumCompleted = playerScript.getMaxDungeonFloorNumCompleted();
        for (int i = 0; i < 5; i++)
        {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), dungeonFloorsPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.TownMenuOption);
            int floorNum = i + 1 + (dungeonFloorsPage * 5);
            bool disabled = maxDungeonFloorNumCompleted+1 < floorNum;
            UiManager.Instance.CreateButton(dungeonFloorsPanel, UiButton.ButtonType.TownMenuOption, "" + floorNum, Item.Rarity.None, null, () => selectDungeonFloor(floorNum), disabled);
        }
    }

    private void enableDungeonFloorsSelection() {
        //Disable the ones that were enabled at the beginning
        UiManager.disablePanel(UiManager.textPanelGameObject);
        UiManager.disablePanel(UiManager.buttonOptionsPanelGameObject);
        
        //Enable the ones needed for dungeon floor selection
        UiManager.enablePanel(UiManager.dungeonFloorsPanelGameObject);
        UiManager.enablePanel(UiManager.dungeonFloorsTextPanel);
        UiManager.enablePanel(UiManager.dungeonFloorsUpButton);
        UiManager.enablePanel(UiManager.dungeonFloorsDownButton);
    }


    private void selectDungeonFloor(int floorNum) {
        Debug.Log("Selected floor: " + floorNum);
        playerScript.setSelectedProfession(selectedProfession);
        startDungeon(floorNum); 
    }

    private void moveDungeonFloorPage(int direction) {
        dungeonFloorsPage += direction;
        if(dungeonFloorsPage < 0) {
            dungeonFloorsPage = 0;
        }
        else {
            setupDungeonFloorButtons(UiManager.dungeonFloorsPanelGameObject);
        }
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

    public void startDungeon(int floorNum) {
        if(playerScript.isDead()) {
            Debug.Log("Can't load dungeon when you're dead!");
        }
        else {
            playerScript.setDungeonFloor(floorNum);
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
