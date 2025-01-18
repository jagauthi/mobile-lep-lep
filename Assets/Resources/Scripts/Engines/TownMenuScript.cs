using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TownMenuScript : MonoBehaviour
{

   public GameObject playerGameObject;
   public GameObject townGameObject;

   private PlayerScript playerScript;

   private TownScript townScript;

   private List<Button> playerAbilityButtons;

   private List<Button> playerItemButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      Debug.Log("TownMenuScript start");
         if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
         }
         
        if(null == townScript) {
            townScript = townGameObject.GetComponent<TownScript>();
        }

         initDungeonOptionsBackground();
         initAbilityButtons();
        
    }

    protected void initDungeonOptionsBackground() {

    }

    protected void initAbilityButtons() {
      playerAbilityButtons = new List<Button>();
      playerItemButtons = new List<Button>();

      List<Button> buttonList = GetComponentsInChildren<Button>().ToList<Button>();
      foreach (Button button in buttonList) {
         if(button.tag == "AbilityButton") {
            playerAbilityButtons.Add(button);
         }
         else if(button.tag == "ItemButton") {
            playerItemButtons.Add(button);
         }
      }

      //Initialize the ability buttons with the player abilities
      for(int i = 0; i < playerAbilityButtons.Count; i++) {
         Button thisButton = playerAbilityButtons[i];
         if(playerScript.getAbilities().Count < i+1) {
            thisButton.gameObject.SetActive(false);
            continue;
         }
         Ability playerAbility = playerScript.getAbilities()[i];

         setButtonTexture(thisButton, playerAbility.getIcon());

         thisButton.onClick.RemoveAllListeners();
         thisButton.onClick.AddListener(() => {
            townScript.doPlayerAbility(playerAbility);
         });
      }

      //Initialize the item buttons with the player items
      for(int i = 0; i < playerItemButtons.Count; i++) {
         Button thisButton = playerItemButtons[i];
         if(playerScript.getInventory().getItems().Count < i+1) {
            thisButton.gameObject.SetActive(false);
            continue;
         }
         Item playerItem = playerScript.getInventory().getItems()[i];

         setButtonTexture(thisButton, playerItem.getIcon());

         thisButton.onClick.RemoveAllListeners();
         thisButton.onClick.AddListener(() => {
            townScript.usePlayerItem(playerItem);
            //If we used an item it might have been removed from inventory, so reload the buttons
            initAbilityButtons();
         });
      }
    }

    private static void setButtonTexture(Button thisButton, Texture2D texture)
    {
      Rect smallIconRect = new Rect(0, 0, Screen.height / 8, Screen.height / 8);
      Rect textureRect = new Rect(0, 0, texture.width, texture.height);
      Sprite sprite = Sprite.Create(texture, textureRect, new Vector2(0.5f, 0.5f));
      
      Image buttonImage = thisButton.GetComponent<Image>();
      buttonImage.sprite = sprite;
      buttonImage.GetComponent<RectTransform>().sizeDelta = new Vector2(smallIconRect.width, smallIconRect.height);

      // thisButton.GetComponent<RectTransform>().sizeDelta = new Vector2(smallIconRect.width, smallIconRect.height);
      // buttonImage.preserveAspect = true;
    }
 
}
