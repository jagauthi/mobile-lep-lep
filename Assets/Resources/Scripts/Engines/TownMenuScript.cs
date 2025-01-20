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
         
        if(null == townScript) {
            townScript = townGameObject.GetComponent<TownScript>();
        }
         if(null == playerScript) {
            playerScript = townScript.playerScript;
         }
        
    }
 
}
