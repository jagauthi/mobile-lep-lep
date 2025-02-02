using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class UiButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button button;
    public Text buttonText;

    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<Text>(); // Auto-assign Text component
    }

    public void Setup(string text, UnityAction onClickAction)
    {
        Debug.Log("Button setup");
        buttonText.text = text;
        button.onClick.RemoveAllListeners(); // Clear old listeners
        button.onClick.AddListener(onClickAction); // Assign new action
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Remember where it started
        transform.SetParent(transform.root); // Move it to the root so it stays on top
        canvasGroup.blocksRaycasts = false; // Allow it to pass through UI elements while dragging
        canvasGroup.alpha = 0.6f; // Make it slightly transparent
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // Move button with cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // Snap back if no valid drop slot
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.alpha = 1f;
    }
}