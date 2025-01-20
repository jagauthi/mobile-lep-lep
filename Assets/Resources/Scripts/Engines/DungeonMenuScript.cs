using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMenuScript : MonoBehaviour
{

   public GameObject playerGameObject;
   public GameObject dungeonGameObject;

   private PlayerScript playerScript;

   private DungeonScript dungeonScript;

   public List<Button> playerAbilityButtons;

   public List<Button> playerItemButtons;

    Rect abilitiesGroupRect, abilitiesBackgroundRect, itemsGroupRect, itemsBackgroundRect;
    private Texture2D selectedTexture;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      if(null == playerGameObject) {
         playerGameObject = GameObject.FindGameObjectWithTag("Player");
      }
      if(null == playerScript) {
         playerScript = playerGameObject.GetComponent<PlayerScript>();
      }
      
      if(null == dungeonScript) {
         dungeonScript = dungeonGameObject.GetComponent<DungeonScript>();
      }

      selectedTexture = (Texture2D)Resources.Load("Images/SelectedIcon");

      abilitiesGroupRect = new Rect(10, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
      abilitiesBackgroundRect = new Rect(0, 0, abilitiesGroupRect.width, abilitiesGroupRect.height);

      itemsGroupRect = new Rect(abilitiesGroupRect.width + 20, (7 * Screen.height / 8) - 10, Screen.width / 3, Screen.height / 8);
      itemsBackgroundRect = new Rect(0, 0, itemsGroupRect.width, itemsGroupRect.height);
 
      // initAbilityButtons();
        
    }
    
    void OnGUI()
    {
         drawPlayerAbilities();
         drawPlayerItems();
    }

    void drawPlayerAbilities()
    {
        int buffer = (int)abilitiesGroupRect.width/16;
        int buttonLength = (int)(abilitiesGroupRect.width / 4 - buffer);
        int buttonHeight = (int)(abilitiesGroupRect.height  - buffer);
        GUI.BeginGroup(abilitiesGroupRect);
        GUI.Box(abilitiesBackgroundRect, "");
         List<Ability> playerAbilities = playerScript.getAbilities();
      //   GUI.DrawTexture(backgroundRect, backgroundTexture);
         for(int i = 0; i < 4; i++) {

            //Create the rect for the slot for the ability
            Rect slot = new Rect(
               buffer/2*(i+1) + buttonLength*i, 
               buffer/2, 
               buttonLength, 
               buttonHeight);
            
            if(playerAbilities.Count > i) {

               Ability ability = playerAbilities[i];

               //Draw selection icon if this is the selected ability
                if(null != dungeonScript.selectedPlayerAbility && ability == dungeonScript.selectedPlayerAbility) {
                    GUI.DrawTexture(
                        new Rect(slot.x-5, slot.y-5, slot.width + 10, slot.height + 10),
                        selectedTexture
                    );
                }

               //Button to select the ability
               if (GUI.Button(slot, "")) {
                  dungeonScript.selectPlayerAbility(ability);
               }

               //Draw the icon over the button
               GUI.DrawTexture( slot, ability.getIcon() );
               
               //cursor tooltip
               if (null != ability && slot.Contains(Event.current.mousePosition)){
                  String abilityTooltip = "Tooltip for " + ability.getName();
                  Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - abilitiesGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - abilitiesGroupRect.y,
                        abilityTooltip.Length*8, Screen.height / 16 / 2);
                  GUI.Box(mouseTextRect, abilityTooltip);
               }
            }
            else {
               //Empty button slot
               if (GUI.Button(slot, "")) {
                  
               }
            }
         }
        
        GUI.EndGroup();
    }

    void drawPlayerItems()
    {
        int buffer = (int)itemsGroupRect.width/16;
        int buttonLength = (int)(itemsGroupRect.width / 4 - buffer);
        int buttonHeight = (int)(itemsGroupRect.height  - buffer);
        GUI.BeginGroup(itemsGroupRect);
        GUI.Box(itemsBackgroundRect, "");
         List<Item> playerItems = playerScript.getInventory().getItems();
      //   GUI.DrawTexture(backgroundRect, backgroundTexture);
         for(int i = 0; i < 4; i++) {

            //Create the rect for the slot for the ability
            Rect slot = new Rect(
               buffer/2*(i+1) + buttonLength*i, 
               buffer/2, 
               buttonLength, 
               buttonHeight);
            
            if(playerItems.Count > i) {

               Item item = playerItems[i];

               //Button to select the ability
               if (GUI.Button(slot, "")) {
                  dungeonScript.usePlayerItem(item);
               }

               //Draw the icon over the button
               GUI.DrawTexture( slot, item.getIcon() );
               
               //cursor tooltip
               if (null != item && slot.Contains(Event.current.mousePosition)){
                  String abilityTooltip = "Tooltip for " + item.getTooltip();
                  Rect mouseTextRect = new Rect(
                        Input.mousePosition.x - itemsGroupRect.x + (buffer / 2),
                        Screen.height - Input.mousePosition.y - itemsGroupRect.y,
                        abilityTooltip.Length*8, Screen.height / 16 / 2);
                  GUI.Box(mouseTextRect, abilityTooltip);
               }
            }
            else {
               //Empty button slot
               if (GUI.Button(slot, "")) {
                  
               }
            }
         }
        
        GUI.EndGroup();
    }
 
}
