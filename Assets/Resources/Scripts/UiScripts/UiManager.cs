using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; } // Singleton instance

    public GameObject buttonPrefab;
    public Transform townOptionsPanel;

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

    public GameObject CreateButton(Transform panel, UiButton.ButtonType buttonType, string text, Item.Rarity rarity, Texture2D icon, UnityAction onClick)
    {
        GameObject newButton = null;
        Debug.Log("UiManager :: CreateButton");

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
                uiButton.Setup(text, onClick, buttonType, rarity, icon);
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
    }
}