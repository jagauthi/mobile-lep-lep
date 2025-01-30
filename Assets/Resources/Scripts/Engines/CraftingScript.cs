using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CraftingScript : MonoBehaviour
{
    public GameObject playerGameObject;

    private PlayerScript playerScript;

    DateTime craftingStartTime;
    int craftingClicks;

    string productCurrentlyCrafting;

    Dictionary<string, List<string>> craftingOptionsMap;



    void Start()
    {
        if(null == playerGameObject) {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
        }
        if(null == playerScript) {
            playerScript = playerGameObject.GetComponent<PlayerScript>();
        }

        craftingClicks = 0;
        craftingOptionsMap = new Dictionary<string, List<string>>();
        

        //Mining
        List<string> miningOptions = new List<string>
        {
            "Copper Ore", "Iron Ore"
        };
        craftingOptionsMap.Add("Mining", miningOptions);
        

        //Smithing
        List<string> smithingOptions = new List<string>
        {
            "Copper Bar", "Iron Bar"
        };
        craftingOptionsMap.Add("Smithing", smithingOptions);
    }

    
    protected void OnGUI(){
        drawCraftingThings();        
    }

    protected void drawCraftingThings() { 
        GuiUtil.craftingDialog(playerScript, this);
    }

    public void goBackToTown() {
        SceneManager.LoadScene("TownScene");
    }

    public DateTime getCraftingStartTime() {
        return craftingStartTime;
    }

    public int getCurrentCraftingProgress() {
        int secondsCrafting = DateTime.Now.Subtract(craftingStartTime).Seconds;
        if(secondsCrafting + craftingClicks >= getMaxCraftingProgress()) {
            Item craftedItem = ItemHandler.getItemMap()[productCurrentlyCrafting];
            if(!playerScript.getInventory().addItem(craftedItem)) {
                playerScript.getInventory().getStashItems().Add(craftedItem);
                Debug.Log("Sent " + craftedItem.getBaseName() + " to stash");
            }
            craftingStartTime = DateTime.Now;
            craftingClicks = 0;
        }
        return secondsCrafting + craftingClicks;
    }

    public int getMaxCraftingProgress() {
        return 30;
    }

    public Dictionary<string, List<string>> getCraftingOptionsMap() {
        return craftingOptionsMap;
    }

    public void setProductCurrentlyCrafting(string product) {
        productCurrentlyCrafting = product;
        craftingClicks = 0;
        craftingStartTime = DateTime.Now;
    }

    public string getProductCurrentlyCrafting() {
        return productCurrentlyCrafting;
    }

    public void clickIncrementCrafting() {
        craftingClicks++;
    }
}
