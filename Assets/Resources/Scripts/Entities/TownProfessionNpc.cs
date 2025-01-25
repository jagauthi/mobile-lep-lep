
using UnityEngine;

public class TownProfessionNpc : NpcScript {

    string dialog;
    int dialogPhase;

    public TownProfessionNpc(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        initValues();

        dialogPhase = 1;

        if(this.npcName == "Sarah the Blacksmith") {
            dialog = "Heya! Whatcha wanna do, go into the dungeon?";
        }
        else {
            dialog = "Unimplemented dialog lol rip";
        }
    }
    
    public override void startInteraction(TownScript townScript)
    {
        townScript.setSelectedProfession(this);
    }

    public string getDialog() {
        return this.dialog;
    }

    public int getDialogPhase() {
        return this.dialogPhase;
    }

    public void setDialogPhase(int dialogPhase) {
        this.dialogPhase = dialogPhase;
    }

    public void incrementDialogPhase() {
        this.dialogPhase++;
    }

}
