using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverScript : NpcScript {

    // bool showingQuests = false;
    protected List<Quest> questList;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
    Rect groupRect = new Rect(Screen.width / 8, Screen.height / 8, 3 * Screen.width / 4, 3 * Screen.height / 4);
    Rect backgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4);
    Rect textRect = new Rect(Screen.width / 8, Screen.height / 8, Screen.width / 2, Screen.height / 8);
    protected Rect exitTextButton = new Rect( Screen.width / 6, Screen.height / 2, Screen.width / 8, Screen.height / 8);
    protected Rect acceptButton = new Rect( Screen.width / 3, Screen.height / 2, Screen.width / 8, Screen.height / 8);

    public QuestGiverScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        initQuests(npcName);
        initValues();
        getGameScript();
    }

    protected void initQuests(string name)
    {
        questList = new List<Quest>();
        Quest q = new Quest("questName1");
        questList.Add(q);
    }
    
    public void showQuests()
    {
        // showingQuests = true;
    }

    void dialogBox()
    {
        
        GUI.Box(screenRect, "");
        GUI.BeginGroup(groupRect);
        GUI.Box(backgroundRect, "");

        if(questList.Count > 0) {
            if(!playerScript.hasQuest(questList[0])) {
                GUI.Label(textRect, questList[0].questText);
                if (GUI.Button(acceptButton, "Accept"))
                {
                    playerScript.addQuest(questList[0]);
                    closeDialog();
                }
            }
            else {
                if(!questList[0].completed) {
                    GUI.Label(textRect, "I've already given you my quest, please complete it first... "
                        + "\n\nProgress: " + questList[0].currentNumber + "/" + questList[0].numberObjective);
                }
                else {
                    GUI.Label(textRect, "Thank you for killing those! :)");
                    if (GUI.Button(acceptButton, "Turn in"))
                    {
                        turnInQuest(questList[0]);
                        closeDialog();
                    }
                }
            }
        }
        else {
            GUI.Label(textRect, "I have no quests for you!");
        }
        if (GUI.Button(exitTextButton, "Leave"))
        {
            closeDialog();
        }
        GUI.EndGroup();
    }

    protected void turnInQuest(Quest quest) {
        playerScript.completeQuest(quest);
        questList.Remove(quest);
    }

    protected void closeDialog() {
        // showingQuests = false;
    }
}
