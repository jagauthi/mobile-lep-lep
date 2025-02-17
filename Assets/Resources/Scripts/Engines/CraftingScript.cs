using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftingScript : MonoBehaviour
{
    
    public enum CraftingTypes { None, Mining, Smithing }

    public GameObject playerGameObject;

    private PlayerScript playerScript;

    DateTime craftingStartTime;
    int craftingClicks;

    string productCurrentlyCrafting;

    Dictionary<CraftingTypes, List<string>> craftingOptionsMap;

    Transform craftingDialogPanel, craftingButtonOptionsPanel;

    TextMeshProUGUI npcCraftingText, productCraftingText;
    CraftingTypes selectedCraftingType;
    TownProfessionNpc selectedProfession;
    Transform manualCraftButtonSlot;
    Image craftingProgressBar;


    void Start()
    {
        if(null == playerGameObject) {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
        }
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        craftingClicks = 0;
        craftingOptionsMap = new Dictionary<CraftingTypes, List<string>>();
        
        //Mining
        List<string> miningOptions = new List<string>
        {
            "Copper Ore", "Iron Ore"
        };
        craftingOptionsMap.Add(CraftingTypes.Mining, miningOptions);
        

        //Smithing
        List<string> smithingOptions = new List<string>
        {
            "Copper Bar", "Iron Bar"
        };
        craftingOptionsMap.Add(CraftingTypes.Smithing, smithingOptions);

        selectedCraftingType = CraftingTypes.None;
        
        initCraftingDialogPanel();
    }

    private void initCraftingDialogPanel()
    {
        if (null == UiManager.craftingDialogPanel)
        {
            GameObject craftingDialogPanelGameObject = (GameObject)Resources.Load("Prefabs/CraftingDialogPanel");
            craftingDialogPanel = MonoBehaviour.Instantiate(craftingDialogPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            craftingDialogPanel.SetParent(canvas.transform, false);
            UiManager.craftingDialogPanel = craftingDialogPanel;
            UiManager.openPanel(UiManager.craftingDialogPanel);
            UiManager.addPanelToList(UiManager.craftingDialogPanel, UiManager.craftingInitOnPanels);
        }
        else {
            craftingDialogPanel = UiManager.craftingDialogPanel;
        }

        selectedProfession = playerScript.getSelectedProfession();

        //Npc Info
        Transform npcInfoPanel = craftingDialogPanel.Find("NpcInfoPanel");
        //NPC icon
        GameObject npcIconGameObject = npcInfoPanel.Find("NpcIcon").gameObject;
        npcIconGameObject.GetComponent<Image>().sprite = Sprite.Create(selectedProfession.getHeadShot(), new Rect(0, 0, selectedProfession.getTexture().width, selectedProfession.getTexture().height), new Vector2(0.5f, 0.5f));
        //NpcName
        TextMeshProUGUI npcNameText = npcInfoPanel.Find("NpcName").gameObject.GetComponent<TextMeshProUGUI>();
        npcNameText.text = selectedProfession.getName();
        //BackToTown button
        Button backToTownButton = npcInfoPanel.Find("BackToTownButton").gameObject.GetComponent<Button>();
        backToTownButton.onClick.RemoveAllListeners();
        backToTownButton.onClick.AddListener(() => goBackToTown());

        //NpcDialog
        GameObject craftingTextPanelGameObject = craftingDialogPanel.Find("TextPanel").gameObject;
        UiManager.craftingTextPanelGameObject = craftingTextPanelGameObject;
        npcCraftingText = craftingTextPanelGameObject.transform.Find("DialogText").gameObject.GetComponent<TextMeshProUGUI>();

        //ButtonOptions
        GameObject craftingButtonOptionsPanelGameObject = craftingDialogPanel.Find("ButtonOptionsPanel").gameObject;
        UiManager.craftingButtonOptionsPanelGameObject = craftingButtonOptionsPanelGameObject;
        craftingButtonOptionsPanel = craftingButtonOptionsPanelGameObject.transform;

        


        //Product Crafting Panel
        GameObject productCraftingPanelGameObject = craftingDialogPanel.Find("ProductCraftingPanel").gameObject;
        UiManager.productCraftingPanelGameObject = productCraftingPanelGameObject;
        Transform productCraftingPanel = productCraftingPanelGameObject.transform;

        //Product text
        productCraftingText = productCraftingPanel.Find("ProductText").GetComponent<TextMeshProUGUI>();

        //Product crafting progress bar
        craftingProgressBar = productCraftingPanel.Find("ProgressBar").GetChild(0).GetComponent<Image>();

        //Manual crafting button
        manualCraftButtonSlot = productCraftingPanel.Find("ManualCraftButtonSlot");

        //Return to choose different product button
        Button chooseProductButton = productCraftingPanel.Find("ChooseProductButton").gameObject.GetComponent<Button>();
        chooseProductButton.onClick.RemoveAllListeners();
        chooseProductButton.onClick.AddListener(() => {
            productCurrentlyCrafting = null;
            
            UiManager.enablePanel(UiManager.craftingTextPanelGameObject);
            UiManager.enablePanel(UiManager.craftingButtonOptionsPanelGameObject);
            UiManager.disablePanel(UiManager.productCraftingPanelGameObject);
        });



        setupCraftingDialog();
        
        UiManager.enablePanel(UiManager.craftingTextPanelGameObject);
        UiManager.enablePanel(UiManager.craftingButtonOptionsPanelGameObject);
        UiManager.disablePanel(UiManager.productCraftingPanelGameObject);
    }

    private void setupCraftingDialog() {

        if(null == selectedProfession) {
            selectedProfession = playerScript.getSelectedProfession();
        }

        //If no crafting type is selected, show the dialog to select crafting type
        if(selectedCraftingType == CraftingTypes.None) {
            npcCraftingText.text = selectedProfession.getCraftingDialog();
            List<CraftingTypes> craftingTypes = selectedProfession.getCraftingTypes();
            UiManager.clearExistingSlotsAndButtons(craftingButtonOptionsPanel);
            loadCraftingTypes(craftingTypes);
        }
        //Otherwise if we haven't selected which product to craft yet, display the options for which products can be crafted
        else if(null == productCurrentlyCrafting) {
            npcCraftingText.text = "And which product do you want to make?";
            List<string> productsToCraft = craftingOptionsMap[selectedCraftingType];
            UiManager.clearExistingSlotsAndButtons(craftingButtonOptionsPanel);
            loadProducts(productsToCraft);
        }
        //Otherwise, we know what we are crafting, so display the crafting display for that product
        else {
            productCraftingText.text = "";
            UiManager.Instance.CreateButtonInSlot(manualCraftButtonSlot, UiButton.ButtonType.PlayerMenuOption, "Click!", Item.Rarity.None, 
                            null, () => clickIncrementCrafting(), false);
        }
    }

    async void loadCraftingTypes(List<CraftingTypes> craftingTypes)
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        foreach(CraftingTypes craftingType in craftingTypes) {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), craftingButtonOptionsPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Item);
            CraftingTypes thisCraftingType = craftingType;
            GameObject newItem = UiManager.Instance.CreateButton(craftingButtonOptionsPanel, UiButton.ButtonType.PlayerMenuOption, craftingType.HumanName(), Item.Rarity.None, 
                            null, () => {
                                selectedCraftingType = thisCraftingType;
                                setupCraftingDialog();
                            }, false);
        }
    }

    async void loadProducts(List<string> productsToCraft)
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        foreach(string product in productsToCraft) {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), craftingButtonOptionsPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Item);
            string thisProduct = product;
            GameObject newItem = UiManager.Instance.CreateButton(craftingButtonOptionsPanel, UiButton.ButtonType.PlayerMenuOption, product, Item.Rarity.None, 
                            null, () => {
                                setProductCurrentlyCrafting(thisProduct);
                                setupCraftingDialog();
                            }, false);
        }
    }

    public void Update()
    {
        updateProgressBar();
    }
    
    public void goBackToTown() {
        SceneManager.LoadScene("TownScene");
    }

    public DateTime getCraftingStartTime() {
        return craftingStartTime;
    }

    public int getCurrentCraftingProgress() {
        int secondsCrafting = DateTime.Now.Subtract(craftingStartTime).Seconds;
        if(secondsCrafting + craftingClicks >= getMaxCraftingProgress()) {
            Item craftedItem = ItemHandler.getItemMap()[productCurrentlyCrafting];
            if(!playerScript.getInventory().addItem(craftedItem)) {
                playerScript.getInventory().getStashItems().Add(craftedItem);
                Debug.Log("Sent " + craftedItem.getBaseName() + " to stash");
            }
            craftingStartTime = DateTime.Now;
            craftingClicks = 0;
        }
        return secondsCrafting + craftingClicks;
    }

    public int getMaxCraftingProgress() {
        return 30;
    }

    public Dictionary<CraftingTypes, List<string>> getCraftingOptionsMap() {
        return craftingOptionsMap;
    }

    public void setProductCurrentlyCrafting(string product) {
        productCurrentlyCrafting = product;
        craftingClicks = 0;
        craftingStartTime = DateTime.Now;

        UiManager.disablePanel(UiManager.craftingTextPanelGameObject);
        UiManager.disablePanel(UiManager.craftingButtonOptionsPanelGameObject);
        UiManager.enablePanel(UiManager.productCraftingPanelGameObject);
    }

    public string getProductCurrentlyCrafting() {
        return productCurrentlyCrafting;
    }

    public void clickIncrementCrafting() {
        craftingClicks++;
    }

    private void updateProgressBar() {
        if(null != craftingProgressBar && null != productCurrentlyCrafting) {
            float fillAmount = getCurrentCraftingProgress() / (float)getMaxCraftingProgress();
            craftingProgressBar.fillAmount = fillAmount;
        }
    }
}
