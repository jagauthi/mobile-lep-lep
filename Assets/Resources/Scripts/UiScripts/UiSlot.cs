using UnityEngine;
using UnityEngine.EventSystems;

public class UiSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        UiButton droppedButton = eventData.pointerDrag.GetComponent<UiButton>();
        if (droppedButton != null)
        {
            droppedButton.transform.SetParent(transform); // Move button to this slot
            droppedButton.transform.localPosition = Vector3.zero; // Center it
        }
    }
}