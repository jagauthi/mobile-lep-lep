using System.Collections.Generic;
using UnityEngine;

public class TownScript : MonoBehaviour
{
   public GameObject playerGameObject;
   public GameObject townOptionsGameObject;

   public PlayerScript playerScript;
   protected List<NpcScript> npcs;
   ShopKeeperScript shopkeeper;


    void Start()
    {
        Debug.Log("TownScript start");
        if(null == playerGameObject) {
            // Debug.Log("PlayerGameObject null");
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if(null == playerGameObject) {
                // Debug.Log("Still didn't find it");
                playerGameObject = (GameObject)Resources.Load("Prefabs/PlayerGameObject");
                playerScript = Instantiate(playerGameObject).GetComponent<PlayerScript>();
            }
            else {
                playerScript = playerGameObject.GetComponent<PlayerScript>();
            }
        }

        shopkeeper = new ShopKeeperScript("Shopkeeper Ben", (Texture2D)Resources.Load("Images/LeatherHelm"));

        npcs = new List<NpcScript>();
        npcs.Add(new NpcScript("Sawah :3", (Texture2D)Resources.Load("Images/SawahBlacksmith1")));
        npcs.Add(shopkeeper);
    }

    public void usePlayerItem(Item item) {
        if(playerScript.useItem(item)) {
            Debug.Log("Player used " + item.getType());
        }
        else {
            Debug.Log("Could not use " + item.getType());
        }
    }
    
    protected void OnGUI(){
        drawTownThings();   
        GuiUtil.drawPlayerTownOptions(playerScript);  
    }

    protected void drawTownThings() { 
        
        //If player has any menus open, we dont want to show most town options
        if(playerScript.anyMenuOpen()) {
            return;
        }
        
        //Otherwise if shopkeeper has inventory open, show that
        if(shopkeeper.showingInventory) {
            GuiUtil.shopkeeperMenu(shopkeeper, playerScript);

            //Displaying the player inventory which allows to sell items
            GuiUtil.playerInventoryMenu(playerScript, shopkeeper);
        }
        //Show the other NPCs if the shopkeeper isn't open
        else {
            GuiUtil.drawNpcs(npcs);
        }
    }

    

}
