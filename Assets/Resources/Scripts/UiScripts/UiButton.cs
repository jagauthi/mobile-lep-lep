using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UiButton : MonoBehaviour
{
    public Button button;
    public Text buttonText;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<Text>(); // Auto-assign Text component
    }

    public void Setup(string text, UnityAction onClickAction)
    {
        buttonText.text = text;
        button.onClick.RemoveAllListeners(); // Clear old listeners
        button.onClick.AddListener(onClickAction); // Assign new action
    }
}