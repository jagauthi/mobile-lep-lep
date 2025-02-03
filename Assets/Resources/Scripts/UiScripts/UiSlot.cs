using UnityEngine;
using UnityEngine.EventSystems;

public class UiSlot : MonoBehaviour, IDropHandler
{

    public UiButton.ButtonType allowedType;

    public bool AcceptsType(UiButton.ButtonType type) 
    {
        return type == allowedType;  // Otherwise, only allow the same type
    }

    public void OnDrop(PointerEventData eventData)
    {
        UiButton droppedButton = eventData.pointerDrag.GetComponent<UiButton>();

        if (droppedButton != null && AcceptsType(droppedButton.buttonType))
        {
            UiButton existingButton = GetComponentInChildren<UiButton>();

            if (existingButton != null)
            {
                if (existingButton.buttonType == droppedButton.buttonType) // Only swap same types
                {
                    Transform oldParent = droppedButton.originalParent;
                    existingButton.transform.SetParent(oldParent);
                    existingButton.transform.localPosition = Vector3.zero;

                    droppedButton.transform.SetParent(transform);
                    droppedButton.transform.localPosition = Vector3.zero;
                }
                else
                {
                    // If types don't match, cancel drop
                    droppedButton.transform.SetParent(droppedButton.originalParent);
                    droppedButton.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                droppedButton.transform.SetParent(transform);
                droppedButton.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            // Return to original slot if invalid drop
            droppedButton.transform.SetParent(droppedButton.originalParent);
            droppedButton.transform.localPosition = Vector3.zero;
        }
    }
}