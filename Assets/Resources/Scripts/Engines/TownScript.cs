using System.Collections.Generic;
using UnityEngine;

public class TownScript : MonoBehaviour
{
   public GameObject playerGameObject;
   public GameObject townOptionsGameObject;

   private PlayerScript playerScript;
   private TownMenuScript townMenuScript;
   protected List<NpcScript> npcs;
   ShopKeeperScript shopkeeper;


    void Start()
    {
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        if(null == townMenuScript) {
            townMenuScript = townOptionsGameObject.GetComponent<TownMenuScript>();
        }

        shopkeeper = new ShopKeeperScript("Shopkeeper", (Texture2D)Resources.Load("Images/LeatherHelm"));

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
    }

    protected void drawTownThings() {
        GuiUtil.drawNpcs(npcs);
        GuiUtil.shopkeeperMenu(shopkeeper, playerScript);
    }

}
