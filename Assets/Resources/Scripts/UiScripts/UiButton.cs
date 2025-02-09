using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UiButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum ButtonType { Ability, Item, ShopkeeperItem, PlayerMenuOption, TownMenuOption }

    public Button button;
    public Text buttonText;
    public Image buttonIcon;
    public Image buttonBackground;
    public Item.Rarity itemRarity;
    public ButtonType buttonType;

    public Transform originalParent;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<Text>(); // Auto-assign Text component

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>(); // Ensure it exists
    }

    public void Setup(string text, UnityAction onClickAction, ButtonType buttonType, Item.Rarity rarity, Texture2D icon)
    {
        if(null == buttonText) {
            Awake();
        }
        this.buttonType = buttonType;
        this.itemRarity = rarity;

        buttonText.text = text;

        if (icon != null) {
            buttonIcon.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.95f, 0.95f));
        }

        SetRarityColor();

        button.onClick.RemoveAllListeners(); // Clear old listeners
        button.onClick.AddListener(onClickAction); // Assign new action
    }

     private void SetRarityColor()
    {
        if (buttonBackground == null) return;

        switch (itemRarity)
        {
            case Item.Rarity.None:
                buttonBackground.color = Color.white;
                break;
            case Item.Rarity.Common:
                buttonBackground.color = Color.gray;
                break;
            case Item.Rarity.Uncommon:
                buttonBackground.color = Color.green;
                break;
            case Item.Rarity.Rare:
                buttonBackground.color = Color.blue;
                break;
            case Item.Rarity.Epic:
                buttonBackground.color = new Color(0.6f, 0, 0.8f); // Purple
                break;
            case Item.Rarity.Legendary:
                buttonBackground.color = new Color(1f, 0.5f, 0); // Orange
                break;
        }
    }

    public bool isNonDraggable() {
        return buttonType == ButtonType.PlayerMenuOption || buttonType == ButtonType.TownMenuOption;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isNonDraggable()) {
            return;
        }
        originalParent = transform.parent; // Remember where it started
        transform.SetParent(transform.root); // Move it to the root so it stays on top
        canvasGroup.blocksRaycasts = false; // Allow it to pass through UI elements while dragging
        canvasGroup.alpha = 0.6f; // Make it slightly transparent
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(isNonDraggable()) {
            return;
        }
        transform.position = eventData.position; // Move button with cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isNonDraggable()) {
            return;
        }
        UiSlot newSlot = GetSlotUnderMouse(eventData);

        if (newSlot == null || !newSlot.AcceptsType(buttonType))
        {
            // No valid slot found, return to original parent
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }

        canvasGroup.blocksRaycasts = true; // Enable interactions again
        canvasGroup.alpha = 1f;
    }

    private UiSlot GetSlotUnderMouse(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            UiSlot slot = result.gameObject.GetComponent<UiSlot>();
            if (slot != null)
            {
                return slot; // Found a valid slot
            }
        }

        return null; // No valid slot found
    }
}