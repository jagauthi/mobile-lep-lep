using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    GameObject backgroundImageGameObject;
    Texture2D townBackground, dungeonBackground, craftingBackground;
    public static string currentScene = "";

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        currentScene = "TownScene";

        backgroundImageGameObject = GameObject.FindGameObjectWithTag("BackgroundImage");
        townBackground = (Texture2D)Resources.Load("Images/TavernBackground1");
        dungeonBackground = (Texture2D)Resources.Load("Images/DungeonBackground1");
        craftingBackground = (Texture2D)Resources.Load("Images/Forge");
    }

    void Start()
    {
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene: " + scene.name);
        currentScene = scene.name;

        UiManager.closeAllPanels();

        //Load all the player init panels
        foreach(Transform panel in UiManager.playerInitOnPanels) {
            UiManager.openPanel(panel);
        }

        //Then init depending on the scene
        if (scene.name == "TownScene") 
        {
            loadBackgroundImage(townBackground);
            foreach(Transform panel in UiManager.townInitOnPanels) {
                UiManager.openPanel(panel);
            }
        }
        else if(scene.name == "DungeonScene")
        {
            loadBackgroundImage(dungeonBackground);
            foreach(Transform panel in UiManager.dungeonInitOnPanels) {
                Debug.Log("Opening " + panel);
                UiManager.openPanel(panel);
            }
        }
        else if(scene.name == "CraftingScene")
        {
            loadBackgroundImage(craftingBackground);
            foreach(Transform panel in UiManager.craftingInitOnPanels) {
                UiManager.openPanel(panel);
            }
        }
        else 
        {
            Debug.Log("Unknown Scene found from CanvasScript");
        }
    }

    private void loadBackgroundImage(Texture2D image) {
        backgroundImageGameObject.GetComponent<Image>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));

    }
}
