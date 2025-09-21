using UnityEngine;
using UnityEngine.UIElements;

public class SmithingUiScript : MonoBehaviour
{

    private VisualElement rootVisualElement;
    private int m_ClickCount = 0;
    private const string m_ButtonPrefix = "button";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Store the root from the UI Document component
        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;


        Button craftButton = rootVisualElement.Q<Button>("btn-craft");

        SetupButtonHandler();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Functions as the event handlers for your button click and number counts
    private void SetupButtonHandler()
    {
        VisualElement root = rootVisualElement;

        var buttons = root.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    private void PrintClickMessage(ClickEvent evt)
    {
        Debug.Log("Button was clicked!");
    }
}
