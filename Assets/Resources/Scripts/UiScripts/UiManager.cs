using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; } // Singleton instance

    public GameObject buttonPrefab;
    
    public static Transform playerOptionsPanel, characterSheetPanel;
    public static Transform playerInventoryPanel, playerStashPanel;
    public static Transform townOptionsButtonPanel, townProfessionsPanel, npcDialogPanel;
    public static Transform shopkeeperInventoryPanel;

    //GameObjects used to enable/disable in the NPC dialog
    public static GameObject npcIconGameObject, textPanelGameObject, buttonOptionsPanelGameObject, closeButtonGameObject, dungeonFloorsPanelGameObject, 
        dungeonFloorsTextPanel, dungeonFloorsUpButton, dungeonFloorsDownButton;

    public static TextMeshProUGUI playerSkillPointsText, playerStrText, playerIntelText, playerAgilText;
    public static List<GameObject> playerSkillButtons;

    public static List<Transform> closeTownProfessionsPanels = new List<Transform>();
    public static List<Transform> openPanels = new List<Transform>();

    void Awake()
    {
        Debug.Log("UiManager :: Awake");
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Prevent duplicates
            
        buttonPrefab = Resources.Load<GameObject>("Prefabs/ButtonPrefab");
    }

    void Start()
    {

    }

    public GameObject CreateButton(Transform panel, UiButton.ButtonType buttonType, string text, Item.Rarity rarity, Texture2D icon, UnityAction onClick, bool disabled)
    {
        GameObject newButton = null;

        // Find the first empty slot in the container
        Transform emptySlot = panel
            .GetComponentsInChildren<UiSlot>()
            .Select(slot => slot.transform)
            .FirstOrDefault(slot => slot.childCount == 0); // Check if the slot has no children

        if (emptySlot != null) 
        {
            newButton = Instantiate(buttonPrefab, emptySlot); // Spawn inside the slot
            UiButton uiButton = newButton.GetComponent<UiButton>();
            if (uiButton != null) {
                uiButton.Setup(text, onClick, buttonType, rarity, icon, disabled);
            }
        }
        else
        {
            Debug.Log("No empty slots available!");
        }
        return newButton;
    }

    public void RemoveButton(Transform panel, string buttonText) {
        UiSlot[] slots = panel.GetComponentsInChildren<UiSlot>();
        foreach(UiSlot slot in slots) {
            UiButton button = slot.GetComponentInChildren<UiButton>();
            if(null != button && null != button.buttonText && button.buttonText.text == buttonText) {
                //Remove this button from the slot
                GameObject.Destroy(button.gameObject);
                return;
            }
        }
        //If we couldn't remove the button, then there might be some issue with how we implemented 
        // finding the button by text..... (likely to happen at some point)
        throw new System.Exception("Failed to remove button: " + buttonText);
    }

    public static void togglePanel(Transform panel)
    {
        // If this panel is configured to close the town professions panel, do that
        if (closeTownProfessionsPanels.Contains(panel))
        {
            townProfessionsPanel.gameObject.SetActive(false);
            openPanels.Remove(townProfessionsPanel);
        }

        // Toggle the panel open/close
        if (panel.gameObject.activeSelf)
        {
            closePanel(panel);
        }
        else
        {
            openPanel(panel);
        }

        //If the panel was the shopkeeper panel, toggle the player inventory to be the same
        if(panel.Equals(shopkeeperInventoryPanel)) {
            if(shopkeeperInventoryPanel.gameObject.activeSelf) {
                openPanel(playerInventoryPanel);
            }
            else {
                closePanel(playerInventoryPanel);
            }
        }
    }

    public static void openPanel(Transform panel) {
        panel.gameObject.SetActive(true);
        if (!openPanels.Contains(panel)) openPanels.Add(panel);
    }

    public static void closePanel(Transform panel) {
        panel.gameObject.SetActive(false);
        openPanels.Remove(panel);
        //If none of the close town profession panels are open, then we can open the town professions back up
        if( null != townProfessionsPanel && !openPanels.Any( panel => closeTownProfessionsPanels.Any( closePanel => closePanel == panel) ) ) {
            townProfessionsPanel.gameObject.SetActive(true);
            if (!openPanels.Contains(panel)) openPanels.Add(townProfessionsPanel);
        }
    }

    public void closeAllPanels()
    {
        foreach (Transform panel in openPanels)
        {
            panel.gameObject.SetActive(false);
        }
        openPanels.Clear();
    }

    public static bool isShopkeeperPanelOpen() {
        return shopkeeperInventoryPanel.gameObject.activeSelf;
    }

    public static bool isStashOpen() {
        return playerStashPanel.gameObject.activeSelf;
    }

    public static void disablePanel(GameObject panel) {
        panel.SetActive(false);
    }

    public static void enablePanel(GameObject panel) {
        panel.SetActive(true);
    }
}