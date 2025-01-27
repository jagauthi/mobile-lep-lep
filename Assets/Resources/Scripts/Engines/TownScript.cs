using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownScript : MonoBehaviour
{
   public GameObject playerGameObject;

   public PlayerScript playerScript;
   protected List<NpcScript> npcs;
   ShopKeeperScript shopkeeper;
   TownProfessionNpc selectedProfession;


    void Start()
    {
        Debug.Log("TownScript start");
        getPlayerScript();

        shopkeeper = new ShopKeeperScript("Shopkeeper Ben", (Texture2D)Resources.Load("Images/LeatherHelm"));

        TownProfessionNpc blacksmith = new TownProfessionNpc("Sarah the Blacksmith", (Texture2D)Resources.Load("Images/SawahBlacksmith1"));

        npcs = new List<NpcScript>();
        npcs.Add(shopkeeper);
        npcs.Add(blacksmith);
    }

    private void getPlayerScript() {
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
    }
    
    protected void OnGUI(){
        drawTownThings();   
    }

    protected void drawTownThings() { 
        
        //If player has any menus open, we dont want to show most town options
        if(playerScript.anyMenuOpen()) {
            return;
        }

        GuiUtil.drawTownOptions(playerScript);
        
        //Otherwise if shopkeeper has inventory open, show that
        if(shopkeeper.showingInventory) {
            GuiUtil.shopkeeperMenu(shopkeeper, playerScript);

            //Displaying the player inventory which allows to sell items
            GuiUtil.playerInventoryMenu(playerScript, shopkeeper, false);
        }
        //Show the other NPCs if the shopkeeper isn't open
        else {
            if(null == selectedProfession) {
                GuiUtil.drawNpcs(npcs, this);
            }
            else {
                GuiUtil.professionDialog(selectedProfession, playerScript, this);
            }
        }
    }

    public void setSelectedProfession(TownProfessionNpc townProfessionNpc) {
        this.selectedProfession = townProfessionNpc;
    }

    public void startDungeon() {
        if(playerScript.isDead()) {
            Debug.Log("Can't load dungeon when you're dead!");
        }
        else {
            SceneManager.LoadScene("DungeonScene");
        }
    }

    public void startCrafting() {
        if(playerScript.isDead()) {
            Debug.Log("Can't craft when you're dead!");
        }
        else {
            SceneManager.LoadScene("CraftingScene");
        }
    }
    

}
