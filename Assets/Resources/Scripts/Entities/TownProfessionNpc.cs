
using UnityEngine;

public class TownProfessionNpc : NpcScript {

    string townDialog, craftingDialog, dialogPhase;

    public TownProfessionNpc(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        initValues();

        dialogPhase = "TownDialog";

        if(this.npcName == "Sarah the Blacksmith") {
            townDialog = "Heya! Whatcha wanna do, go into the dungeon?";
            craftingDialog = "Cool lets make something :) Which crafting do you wanna do?";
        }
        else {
            townDialog = "Unimplemented dialog lol rip";
        }
    }
    
    public override void startInteraction(TownScript townScript)
    {
        townScript.setSelectedProfession(this);
        townScript.setupNpcDialogPanel(this);
    }

    public string getTownDialog() {
        return this.townDialog;
    }

    public string getCraftingDialog() {
        return this.craftingDialog;
    }

    public string getDialogPhase() {
        return this.dialogPhase;
    }

    public void setDialogPhase(string dialogPhase) {
        this.dialogPhase = dialogPhase;
    }

}
