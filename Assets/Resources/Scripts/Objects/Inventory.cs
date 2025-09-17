using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory
{

    List<Item> items;
    List<Item> stashItems;
    public Transform playerInventoryPanel, playerStashPanel;
    PlayerScript playerScript;

    private List<GameObject> inventorySlots = new List<GameObject>();
    private List<GameObject> stashSlots = new List<GameObject>();
    private List<GameObject> inventoryButtons = new List<GameObject>();
    private List<GameObject> stashButtons = new List<GameObject>();
    private int inventoryTotalSlots = 27;
    private int stashTotalSlots = 90;
    private int inventorySlotsPerPage = 9;
    private int stashSlotsPerPage = 15;
    private int inventoryCurrentPage = 0;
    private int stashCurrentPage = 0;

    public Button inventoryNextPageButton, inventoryPrevPageButton, stashNextPageButton, stashPrevPageButton;
    TextMeshProUGUI inventoryGoldText;

    public Inventory()
    {
        items = new List<Item>();
        stashItems = new List<Item>();

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        initPlayerInventoryPanel();
        initPlayerStashPanel();
    }

    private void initPlayerInventoryPanel()
    {

        if (null == UiManager.playerInventoryPanel)
        {
            GameObject playerInventoryPanelGameObject = (GameObject)Resources.Load("Prefabs/PlayerInventoryPanel");
            playerInventoryPanel = MonoBehaviour.Instantiate(playerInventoryPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            playerInventoryPanel.SetParent(canvas.transform, false);
            UiManager.playerInventoryPanel = playerInventoryPanel;
            UiManager.closeTownProfessionsPanels.Add(playerInventoryPanel);
        }
        else
        {
            playerInventoryPanel = UiManager.playerInventoryPanel;
        }

        inventoryGoldText = playerInventoryPanel.Find("InventoryTextPanel").Find("Gold").gameObject.GetComponent<TextMeshProUGUI>();

        Button[] paginationButtons = playerInventoryPanel.GetComponentsInChildren<Button>();
        foreach (Button button in paginationButtons)
        {
            if (button.gameObject.tag == "LeftPagination")
            {
                inventoryPrevPageButton = button;
                inventoryPrevPageButton.onClick.RemoveAllListeners();
                inventoryPrevPageButton.onClick.AddListener(() => PreviousPage(playerInventoryPanel, inventoryTotalSlots, inventorySlots, inventoryButtons,
                    ref inventoryCurrentPage, inventorySlotsPerPage, ref inventoryPrevPageButton, ref inventoryNextPageButton));
            }
            else if (button.gameObject.tag == "RightPagination")
            {
                inventoryNextPageButton = button;
                inventoryNextPageButton.onClick.RemoveAllListeners();
                inventoryNextPageButton.onClick.AddListener(() => NextPage(playerInventoryPanel, inventoryTotalSlots, inventorySlots, inventoryButtons,
                    ref inventoryCurrentPage, inventorySlotsPerPage, ref inventoryPrevPageButton, ref inventoryNextPageButton));
            }
        }

        CreateInventorySlots(playerInventoryPanel, inventoryTotalSlots, inventorySlots);
        LoadInventoryItems(playerInventoryPanel, inventoryTotalSlots, inventoryButtons, true);
        UpdatePage(playerInventoryPanel, inventoryTotalSlots, inventorySlots, inventoryButtons,
                    ref inventoryCurrentPage, inventorySlotsPerPage, ref inventoryPrevPageButton, ref inventoryNextPageButton);

        //Turn it off after initializing it
        UiManager.togglePanel(playerInventoryPanel);
    }

    private void initPlayerStashPanel()
    {

        if (null == playerStashPanel)
        {
            GameObject playerStashPanelGameObject = (GameObject)Resources.Load("Prefabs/PlayerStashPanel");
            playerStashPanel = MonoBehaviour.Instantiate(playerStashPanelGameObject).GetComponent<Transform>();
            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            playerStashPanel.SetParent(canvas.transform, false);
            UiManager.playerStashPanel = playerStashPanel;
            UiManager.closeTownProfessionsPanels.Add(playerStashPanel);
        }


        Button[] paginationButtons = playerStashPanel.GetComponentsInChildren<Button>();
        foreach (Button button in paginationButtons)
        {
            if (button.gameObject.tag == "LeftPagination")
            {
                stashPrevPageButton = button;
                stashPrevPageButton.onClick.RemoveAllListeners();
                stashPrevPageButton.onClick.AddListener(() => PreviousPage(playerStashPanel, stashTotalSlots, stashSlots, stashButtons,
                    ref stashCurrentPage, stashSlotsPerPage, ref stashPrevPageButton, ref stashNextPageButton));
            }
            else if (button.gameObject.tag == "RightPagination")
            {
                stashNextPageButton = button;
                stashNextPageButton.onClick.RemoveAllListeners();
                stashNextPageButton.onClick.AddListener(() => NextPage(playerStashPanel, stashTotalSlots, stashSlots, stashButtons,
                    ref stashCurrentPage, stashSlotsPerPage, ref stashPrevPageButton, ref stashNextPageButton));
            }
        }

        CreateInventorySlots(playerStashPanel, stashTotalSlots, stashSlots);
        LoadInventoryItems(playerStashPanel, stashTotalSlots, stashButtons, false);
        UpdatePage(playerStashPanel, stashTotalSlots, stashSlots, stashButtons,
                    ref stashCurrentPage, stashSlotsPerPage, ref stashPrevPageButton, ref stashNextPageButton);

        //Turn it off after initializing it
        UiManager.togglePanel(playerStashPanel);
    }

    void CreateInventorySlots(Transform panel, int totalSlots, List<GameObject> slots)
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/UiSlotPrefab"), panel);
            newSlot.GetComponent<UiSlot>().setType(UiButton.ButtonType.Item);
            slots.Add(newSlot);
        }
    }

    void LoadInventoryItems(Transform panel, int totalSlots, List<GameObject> itemButtons, bool playerInventory)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (i >= totalSlots)
            {
                Debug.Log("Out of slots");
                return;
            }
            Item item = items[i];

            UnityAction itemUseFunction = null;
            //If this is used from the inventory, use the item
            if (playerInventory)
            {
                playerScript.useItem(item);
            }
            //Otherwise, it's from stash, so transfer item from stash to inventory
            else
            {
                transferFromStashToInventory(item);
            }


            GameObject newItem = UiManager.Instance.CreateButton(panel, UiButton.ButtonType.PlayerMenuOption, "", Item.Rarity.None,
                                item.getIcon(), itemUseFunction, false, item.getTooltip());

            newItem.SetActive(false); // Initially hidden
            itemButtons.Add(newItem);
        }
    }

    private bool transferFromStashToInventory(Item item)
    {
        //First add the item to the player's inventory, if successful then remove it from stash
        if (addItem(item))
        {
            loseStashItem(item);
            return true;
        }
        else
        {
            Debug.Log("No room in player inventory");
            return false;
        }
    }

    public bool transferFromInventoryToStash(Item item)
    {
        //First add the item to the stash, if successful then remove it from inventory
        if (addStashItem(item))
        {
            loseItem(item);
            return true;
        }
        else
        {
            Debug.Log("No room in stash");
            return false;
        }
    }

    void UpdatePage(Transform panel, int totalSlots, List<GameObject> slots, List<GameObject> itemButtons, ref int currentPage, int slotsPerPage,
        ref Button prevPageButton, ref Button nextPageButton)
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
            if (itemButtons.Count > i)
            {
                itemButtons[i].SetActive(true);
            }
        }

        // Enable/Disable buttons based on page limits
        prevPageButton.interactable = currentPage > 0;
        nextPageButton.interactable = end < totalSlots;
    }

    public void NextPage(Transform panel, int totalSlots, List<GameObject> slots, List<GameObject> itemButtons, ref int currentPage, int slotsPerPage,
        ref Button prevPageButton, ref Button nextPageButton)
    {
        if ((currentPage + 1) * slotsPerPage < totalSlots)
        {
            currentPage++;
            UpdatePage(panel, totalSlots, slots, itemButtons, ref currentPage, slotsPerPage, ref prevPageButton, ref nextPageButton);
        }
    }

    public void PreviousPage(Transform panel, int totalSlots, List<GameObject> slots, List<GameObject> itemButtons, ref int currentPage, int slotsPerPage,
        ref Button prevPageButton, ref Button nextPageButton)
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage(panel, totalSlots, slots, itemButtons, ref currentPage, slotsPerPage, ref prevPageButton, ref nextPageButton);
        }
    }

    public Inventory(List<Item> items)
    {
        this.items = items;
    }

    public bool addItem(Item item)
    {
        if (items.Count < inventoryTotalSlots)
        {
            items.Add(item);
            Debug.Log("Inventory added item with rarity " + item.getRarity());
            UiManager.Instance.CreateButton(playerInventoryPanel, UiButton.ButtonType.Item, item.getUuid(), item.getRarity(), item.getIcon(), () => playerScript.useItem(item), false, item.getTooltip());
            return true;
        }
        else
        {
            Debug.Log("Inventory at max size");
            return false;
        }
    }

    public bool addStashItem(Item item)
    {
        if (stashItems.Count < stashTotalSlots)
        {
            stashItems.Add(item);
            UiManager.Instance.CreateButton(playerStashPanel, UiButton.ButtonType.Item, item.getUuid(), item.getRarity(), item.getIcon(), () => transferFromStashToInventory(item), false, item.getTooltip());
            return true;
        }
        else
        {
            Debug.Log("Stash at max size");
            return false;
        }
    }

    public List<Item> getItems()
    {
        return items;
    }

    public void loseItem(Item item)
    {
        items.Remove(item);
        UiManager.Instance.RemoveButton(playerInventoryPanel, item.getUuid());
    }

    public void loseStashItem(Item item)
    {
        stashItems.Remove(item);
        UiManager.Instance.RemoveButton(playerStashPanel, item.getUuid());
    }

    public int getSize()
    {
        return items.Count;
    }

    public List<Item> getStashItems()
    {
        return stashItems;
    }

    public void updateGoldText(PlayerScript playerScript)
    {
        inventoryGoldText.text = "Gold: " + playerScript.getGold();
    }

    public Dictionary<string, int> getInventoryContents()
    {
        Dictionary<string, int> inventoryContents = new Dictionary<string, int>();
        foreach (Item item in items)
        {
            if (inventoryContents[item.getBaseName()] == 0)
            {
                inventoryContents.Add(item.getBaseName(), 1);
            }
            else
            {
                inventoryContents[item.getBaseName()] += 1;
            }
        }
        return inventoryContents;
    }

}
