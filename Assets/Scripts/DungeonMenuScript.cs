using UnityEngine;

public class DungeonMenuScript : MonoBehaviour
{

   public GameObject playerGameObject;

   private PlayerScript playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(null == playerScript) {
         playerScript = playerGameObject.GetComponent<PlayerScript>();
        }
        Debug.Log("Got playerScript: " + playerScript);
        
        playerScript.printAbilities(); 
    }

    public void onButton1() {
       Debug.Log("testButton1"); 
    }

    public void onButton2() {
       Debug.Log("testButton2"); 
    }
}
