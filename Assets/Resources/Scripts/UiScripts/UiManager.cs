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

    public void CreateButton(Transform panel, string text, UnityAction onClick)
    {
        Debug.Log("UiManager :: CreateButton");

        // Find the first empty slot in the container
        Transform emptySlot = panel
            .GetComponentsInChildren<UiSlot>()
            .Select(slot => slot.transform)
            .FirstOrDefault(slot => slot.childCount == 0); // Check if the slot has no children

        if (emptySlot != null) 
        {
            GameObject newButton = Instantiate(buttonPrefab, emptySlot); // Spawn inside the slot
            UiButton uiButton = newButton.GetComponent<UiButton>();
            if (uiButton != null) uiButton.Setup(text, onClick);
        }
        else
        {
            Debug.LogWarning("No empty slots available!");
        }

    }
}