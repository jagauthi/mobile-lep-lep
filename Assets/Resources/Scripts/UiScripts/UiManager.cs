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

        GameObject newButton = Instantiate(buttonPrefab, panel);
        newButton.GetComponent<UiButton>().Setup(text, onClick);
    }
}