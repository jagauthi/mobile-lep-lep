using UnityEngine;
using System.Collections;

public class NpcScript
{
    protected string npcName;
    protected PlayerScript playerScript;
    protected GameScript gameScript;
    protected Texture2D texture;

    public NpcScript(string npcName, Texture2D texture) {
        this.npcName = npcName;
        this.texture = texture;
        getGameScript();
        DoInits();
    }

    protected void DoInits()
    {
        initValues();
    }

    protected void initValues()
    {
        getPlayerScript();
        // if(player != null && player.GetComponent<MageScript>() != null) {
        //     playerScript = player.GetComponent<MageScript>();
        // }
        // else if(player != null && player.GetComponent<RangerScript>() != null) {
        //     playerScript = player.GetComponent<RangerScript>();
        // }
        // else if(player != null && player.GetComponent<WarriorScript>() != null){
        //     playerScript = player.GetComponent<WarriorScript>();
        // } 
    }

    protected PlayerScript getPlayerScript() {
        if(playerScript == null && null != GameObject.FindGameObjectWithTag("Player")) {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
        return playerScript;
    }

    protected void getGameScript()
    {
        GameObject[] objects = Object.FindObjectsByType <GameObject>(FindObjectsSortMode.None);
        foreach (GameObject o in objects)
        {
            if (o.name.Equals("GameEngine"))
            {
                gameScript = o.GetComponent<GameScript>();
            }
        }
    }

    public string getName() {
        return npcName;
    }

    public Texture2D getTexture() {
        return texture;
    }

    protected void Die()
    {
        Debug.Log("Blehhh X.X");
    }

    public virtual void startInteraction(TownScript townScript) {
        
    }
}
