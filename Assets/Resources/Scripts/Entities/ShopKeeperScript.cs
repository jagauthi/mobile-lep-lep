using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperScript : NpcScript {

    public List<Item> items;
    public Transform shopkeeperInventoryPanel;
    private Dictionary<int, GameObject> slots = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> itemButtons = new Dictionary<int, GameObject>(); 
    private int totalSlots = 30;
    private int slotsPerPage = 9;
    private int currentPage = 0;
    public Button nextPageButton, prevPageButton, closeButton;

    public ShopKeeperScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        initInventory(npcName);
        initValues();
        
        initShopkeeperInventoryPanel();

        UiManager.closePanel(shopkeeperInventoryPanel);
    }

    protected void initInventory(string name)
    {
        items = new List<Item>();

        //Add 2-4 consumeables
        // items.AddRange(ItemHandler.generateItemsOfType(2, 4, "Consumable", 1));

        //Add 1-2 pieces of armor
        items.AddRange(ItemHandler.generateItemsOfType(8, 9, "Armor", playerScript.getMaxDungeonFloorNumCompleted()+1));
    }

    private void initShopkeeperInventoryPanel() {

        if(null != UiManager.shopkeeperInventoryPanel) {
            GameObject.Destroy(UiManager.shopkeeperInventoryPanel.gameObject);
        }

        GameObject shopkeeperInventoryPanelGameObject = (GameObject)Resources.Load("Prefabs/ShopkeeperInventoryPanel");
        shopkeeperInventoryPanel = MonoBehaviour.Instantiate(shopkeeperInventoryPanelGameObject).GetComponent<Transform>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        shopkeeperInventoryPanel.SetParent(canvas.transform, false);
        UiManager.shopkeeperInventoryPanel = shopkeeperInventoryPanel;
        UiManager.closeTownProfessionsPanels.Add(shopkeeperInventoryPanel);
        UiManager.openPanel(UiManager.shopkeeperInventoryPanel);

        //First clear existing slots/buttons
        UiManager.clearExistingSlotsAndButtons(shopkeeperInventoryPanel);

        loadRestOfPanel();
    }

    async void loadRestOfPanel()
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        Button[] paginationButtons = shopkeeperInventoryPanel.GetComponentsInChildren<Button>();
        foreach (Button button in paginationButtons){
            if (button.gameObject.tag == "LeftPagination"){
                prevPageButton = button;
                prevPageButton.onClick.AddListener(PreviousPage);
            }
            else if(button.gameObject.tag == "RightPagination"){
                nextPageButton = button;
                nextPageButton.onClick.AddListener(NextPage);
            }
            else if(button.gameObject.tag == "CloseButton"){
                closeButton = button;
                closeButton.onClick.AddListener(CloseShop);
            }
        }

        CreateInventorySlots();
        LoadInventoryItems();
        UpdatePage();
    }

    void CreateInventorySlots()
    {
        slots.Clear();

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), shopkeeperInventoryPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.ShopkeeperItem);
            slots.Add(i, newSlot);
        }
    }

    void LoadInventoryItems()
    {
        itemButtons.Clear();
        //First initialize each button to null
        for (int i = 0; i < totalSlots; i++)
        {
            itemButtons.Add(i, null);
        }

        //Then add the items that we do have
        for (int i = 0; i < items.Count; i++)
        {
            if(i >= totalSlots) {
                Debug.Log("Out of slots");
                return;
            }
            addItem(items[i]);
        }
    }

    void UpdatePage()
    {
        // Hide all slots and buttons
        foreach (KeyValuePair<int, GameObject> slot in slots) {
            slot.Value.SetActive(false);
        }
        foreach (KeyValuePair<int, GameObject> item in itemButtons) {
            if(null != item.Value) {
                item.Value.SetActive(false);
            }
        }

        // Show slots & buttons for current page
        int start = currentPage * slotsPerPage;
        int end = Mathf.Min(start + slotsPerPage, totalSlots);

        for (int i = start; i < end; i++)
        {
            slots[i].SetActive(true);
            if(itemButtons[i] != null) {
                itemButtons[i].SetActive(true);
            }
        }

        // Enable/Disable buttons based on page limits
        prevPageButton.interactable = currentPage > 0;
        nextPageButton.interactable = end < totalSlots;
    }

    public void NextPage()
    {
        if ((currentPage + 1) * slotsPerPage < totalSlots)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    public void CloseShop()
    {
        UiManager.togglePanel(shopkeeperInventoryPanel);
    }
    
    public override void startInteraction(TownScript townScript)
    {
        UiManager.togglePanel(shopkeeperInventoryPanel);
    }

    
    public int getCost(Item item) {
        return item.getCost();
    }

    public bool buyItem(Item item) {
        bool addedItem = addItem(item);
        if(addedItem) {
            items.Add(item);
            UpdatePage();
            return true;
        }
        return false;
    }

    public bool addItem(Item item) {
        if(items.Count >= totalSlots) {
            Debug.Log("Not enough room to buy");
            return false;
        }
        GameObject newItem = UiManager.Instance.CreateButton(shopkeeperInventoryPanel, UiButton.ButtonType.ShopkeeperItem, item.getBaseName(), item.getRarity(), 
                            item.getIcon(), () => playerScript.buyItem(item, item.getCost(), this), false, item.getTooltip());

        foreach (KeyValuePair<int, GameObject> existingItem in itemButtons) {
            if(null == existingItem.Value) {
                itemButtons[existingItem.Key] = newItem;
                break;
            }
        }

        return true;
    }

    public void loseItem(Item item) {
        Debug.Log("Shopkeeper lose item");
        items.Remove(item);
        foreach (KeyValuePair<int, GameObject> existingItem in itemButtons) {
            if(null != existingItem.Value) {
                UiButton button = existingItem.Value.GetComponent<UiButton>();
                if(null != button && null != button.buttonText && button.buttonText.text == item.getBaseName()) {
                    itemButtons[existingItem.Key] = null;
                    UiManager.Instance.RemoveButton(shopkeeperInventoryPanel, item.getBaseName());
                    break;
                }
            }
        }
        UpdatePage();
    }
}
