using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory {

    List<Item> items;
    List<Item> stashItems;
    int maxSize, inventoryPage, stashPage;
    Transform playerInventoryPanel;
    PlayerScript playerScript;

    private List<GameObject> slots = new List<GameObject>();
    private List<GameObject> itemButtons = new List<GameObject>(); 
    private int totalSlots = 30;
    private int slotsPerPage = 9;
    private int currentPage = 0;

    public Button nextPageButton, prevPageButton;

    public Inventory() {
        items = new List<Item>();
        stashItems = new List<Item>();
        maxSize = 12;
        inventoryPage = 0;
        stashPage = 0;
        
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        initPlayerInventoryPanel();

        CreateInventorySlots();
        LoadInventoryItems();
        UpdatePage();

        closeInventory();
    }

    private void initPlayerInventoryPanel() {


        if(null == playerInventoryPanel) {
            GameObject playerInventoryPanelGameObject = (GameObject)Resources.Load("Prefabs/PlayerInventoryPanel");
            playerInventoryPanel = MonoBehaviour.Instantiate(playerInventoryPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            playerInventoryPanel.SetParent(canvas.transform, false);
        }

        
        Button[] paginationButtons = playerInventoryPanel.GetComponentsInChildren<Button>();
        foreach (Button button in paginationButtons){
            if (button.gameObject.tag == "LeftPagination"){
                prevPageButton = button;
                prevPageButton.onClick.AddListener(PreviousPage);
            }
            else if(button.gameObject.tag == "RightPagination"){
                nextPageButton = button;
                nextPageButton.onClick.AddListener(NextPage);
            }
        }
        
    }

    public void toggleInventory() {
        playerInventoryPanel.gameObject.SetActive(!playerInventoryPanel.gameObject.activeSelf);
    }
    public void closeInventory() {
        playerInventoryPanel.gameObject.SetActive(false);
    }
    public void openInventory() {
        playerInventoryPanel.gameObject.SetActive(true);
    }

    void CreateInventorySlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), playerInventoryPanel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Item);
            slots.Add(newSlot);
        }
    }

    void LoadInventoryItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(i >= totalSlots) {
                Debug.Log("Out of slots");
                return;
            }
            Item item = items[i];
            GameObject newItem = UiManager.Instance.CreateButton(playerInventoryPanel, UiButton.ButtonType.PlayerMenuOption, "Character", Item.Rarity.None, 
                                (Texture2D)Resources.Load("Images/CharacterMenuIcon"), () => playerScript.useItem(item));

            newItem.SetActive(false); // Initially hidden
            itemButtons.Add(newItem);
        }
    }

    void UpdatePage()
    {
        // Hide all slots and buttons
        foreach (GameObject slot in slots) slot.SetActive(false);
        foreach (GameObject item in itemButtons) item.SetActive(false);

        // Show slots & buttons for current page
        int start = currentPage * slotsPerPage;
        int end = Mathf.Min(start + slotsPerPage, totalSlots);

        for (int i = start; i < end; i++)
        {
            slots[i].SetActive(true);
            if(itemButtons.Count > i) {
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

    public Inventory(List<Item> items) {
        this.items = items;
        maxSize = 12;
        inventoryPage = 0;
        stashPage = 0;
    }

    public bool addItem(Item item) {
        if(items.Count < maxSize) {
            items.Add(item);
            UiManager.Instance.CreateButton(playerInventoryPanel, UiButton.ButtonType.Item, item.getBaseName(), item.getRarity(), item.getIcon(), () => playerScript.useItem(item));
            return true;
        }
        else {
            Debug.Log("Inventory at max size");
            return false;
        }
    }

    public List<Item> getItems() {
        return items;
    }

    public void loseItem(Item item) {
        items.Remove(item);
        UiManager.Instance.RemoveButton(playerInventoryPanel, item.getBaseName());
    }

    public int getSize() {
        return items.Count;
    }

    public List<Item> getStashItems() {
        return stashItems;
    }

    public int getInventoryPageNumber() {
        return inventoryPage;
    }

    public void moveInventoryPage(int direction) {
        inventoryPage += direction;
        if(inventoryPage < 0) {
            inventoryPage = 0;
        }
    }

    public int getStashPageNumber() {
        return stashPage;
    }

    public void moveStashPage(int direction) {
        stashPage += direction;
        if(stashPage < 0) {
            stashPage = 0;
        }
    }
}
