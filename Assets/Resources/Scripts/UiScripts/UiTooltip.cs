using UnityEngine;
using TMPro;

public class UiTooltip : MonoBehaviour
{

     public static UiTooltip Instance;
    
    public TextMeshProUGUI tooltipText;
    public RectTransform background;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
        canvasGroup = GetComponent<CanvasGroup>();
        HideTooltip();
    }

    public void Update()
    {
        if (canvasGroup.alpha > 0) {
            transform.position = Input.mousePosition + new Vector3(10, background.rect.height, 0);
        }
    }

    public void ShowTooltip(string text, Vector2 position)
    {
        if(null == text || text == "") {
            return;
        }
        tooltipText.text = text;
        background.sizeDelta = new Vector2(tooltipText.preferredWidth + 20, tooltipText.preferredHeight + 10);
        // transform.position = position;
        canvasGroup.alpha = 0.65f; // Show
        transform.position = Input.mousePosition + new Vector3(10, background.rect.height, 0);
        transform.SetAsLastSibling(); // Moves tooltip to the top of the UI
    }

    public void HideTooltip()
    {
        canvasGroup.alpha = 0; // Hide
    }
}