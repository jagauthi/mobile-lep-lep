using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {

    public string questName, questText, objectiveTarget;
    public bool completed;
    public int numberObjective, currentNumber;
    public int goldReward, expReward;

    public Quest(string questName) {
        this.questName = questName;
        loadQuestInfo(questName);
    }

    void loadQuestInfo(string questName) {
        //load file for questName
        //set all the values for the quest from the file
        questText = "Kill 5 enemies";
        objectiveTarget = "Enemy";
        completed = false;
        currentNumber = 0;
        numberObjective = 5;
        goldReward = 50;
        expReward = 100;
    }
}
