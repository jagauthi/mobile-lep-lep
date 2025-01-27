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

        craftingOptionsMap = new Dictionary<string, List<string>>();
        

        //Mining
        List<string> miningOptions = new List<string>
        {
            "Copper ore", "Iron ore"
        };
        craftingOptionsMap.Add("Mining", miningOptions);
        

        //Smithing
        List<string> smithingOptions = new List<string>
        {
            "Copper bar", "Iron bar"
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
        return 10;
    }

    public int getMaxCraftingProgress() {
        return 50;
    }

    public Dictionary<string, List<string>> getCraftingOptionsMap() {
        return craftingOptionsMap;
    }

    public void setProductCurrentlyCrafting(string product) {
        productCurrentlyCrafting = product;
    }

    public string getProductCurrentlyCrafting() {
        return productCurrentlyCrafting;
    }
}
