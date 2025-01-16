using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateCharacter : MonoBehaviour {
    
    GameObject newPlayer, emptyPlayer;
	Rect mageRect, rangerRect, warriorRect;
	Rect pointsRect, groupRect, backgroundRect, strengthAdd, strengthMinus;
	Rect agilityAdd, agilityMinus, intelligenceAdd, intelligenceMinus, quitRect, createRect;
	Rect agilityTextRect, intelligenceTextRect, strengthTextRect;
	
	string playerClass;
	int skillPoints, strength, agility, intelligence;
    
    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        emptyPlayer = GameObject.FindGameObjectWithTag("EmptyPlayer");
		restartStats();
		initGUI();
    }

    void Update()
    {
        
    }

	void restartStats() {
		skillPoints = 10;
		strength = 5;
		agility = 5;
		intelligence = 5;
		playerClass = "Ranger";
	}

	void initGUI() {
		int buffer = Screen.height / 16;
		int buttonLength = Screen.height / 16;
		int textLength = Screen.height / 8;
		pointsRect = new Rect(buffer, 0, textLength, buttonLength);
		groupRect = new Rect(Screen.width / 8, Screen.height / 8, Screen.width / 2, 3 * Screen.height / 4);
		backgroundRect = new Rect(0, 0, 3 * Screen.width / 4, 3 * Screen.height / 4);
		strengthTextRect = new Rect(buffer, Screen.height / 8, Screen.height / 4, textLength);		strengthAdd = new Rect(buffer + textLength + buttonLength, Screen.height / 8, buttonLength, buttonLength);
		strengthMinus = new Rect(buffer + textLength + buttonLength, Screen.height / 8, buttonLength, buttonLength);
		strengthAdd = new Rect(buffer*2 + textLength + buttonLength*2, Screen.height / 8, buttonLength, buttonLength);
		agilityTextRect = new Rect(buffer, Screen.height / 8 * 2, Screen.height / 4, textLength);
		agilityMinus = new Rect(buffer + textLength + buttonLength, Screen.height / 8 * 2, buttonLength, buttonLength);
		agilityAdd = new Rect(buffer*2 + textLength + buttonLength*2, Screen.height / 8 * 2, buttonLength, buttonLength);
		intelligenceTextRect = new Rect(buffer, Screen.height / 8 * 3, Screen.height / 4, textLength);
		intelligenceMinus = new Rect(buffer + textLength + buttonLength, Screen.height / 8 * 3, buttonLength, buttonLength);
		intelligenceAdd = new Rect(buffer*2 + textLength + buttonLength*2, Screen.height / 8 * 3, buttonLength, buttonLength);
		mageRect = new Rect(Screen.width/4-buttonLength, 0, textLength, buttonLength);
		rangerRect = new Rect(Screen.width/4-buttonLength + (textLength + buffer), 0, textLength, buttonLength);
		warriorRect = new Rect(Screen.width/4-buttonLength + 2*(textLength + buffer), 0, textLength, buttonLength);
		createRect = new Rect(Screen.width/6, Screen.height / 2, textLength*3, textLength);
		quitRect = new Rect(Screen.width/4-buttonLength + 3*(textLength + buffer), 0, buttonLength, buttonLength);
	}

    void OnGUI()
    {
		mainMenu();
    }

    void mainMenu()
    {
		GUI.BeginGroup(groupRect);
        GUI.Box(backgroundRect, "");
		//Class selection buttons
		if (GUI.Button(mageRect, "Mage"))
		{
        	emptyPlayer.GetComponent<Renderer>().material.color = Color.blue;
			playerClass = "Mage";
		}
		if (GUI.Button(rangerRect, "Ranger"))
		{
        	emptyPlayer.GetComponent<Renderer>().material.color = Color.green;
			playerClass = "Ranger";
		}
		if (GUI.Button(warriorRect, "Warrior"))
		{
        	emptyPlayer.GetComponent<Renderer>().material.color = Color.red;
			playerClass = "Warrior";
		}
		//Stat/point labels
        GUI.Label(pointsRect, "Points: " + skillPoints);
        GUI.Label(strengthTextRect, "Strength: " + strength);
        GUI.Label(agilityTextRect, "Agility: " + agility);
        GUI.Label(intelligenceTextRect, "Intelligence: " + intelligence);
		//Stat minus buttons
		if(strength > 1) {
			if (GUI.Button(strengthMinus, "-"))
            {
                strength--;
                skillPoints++;
            }
		}
		if(agility > 1) {
			if (GUI.Button(agilityMinus, "-"))
            {
                agility--;
                skillPoints++;
            }
		}
		if(intelligence > 1) {
			if (GUI.Button(intelligenceMinus, "-"))
            {
                intelligence--;
                skillPoints++;
            }
		}
		//Stat plus buttons
        if (skillPoints > 0) { 
            if (GUI.Button(strengthAdd, "+"))
            {
                strength++;
                skillPoints--;
            }
            if (GUI.Button(agilityAdd, "+"))
            {
                agility++;
                skillPoints--;
            }
            if (GUI.Button(intelligenceAdd, "+"))
            {
                intelligence++;
                skillPoints--;
            }
        }
		//Rest of buttons
        if (GUI.Button(quitRect, "X"))
        {
            Application.Quit();
        }
        if (GUI.Button(createRect, "Create"))
        {
			createCharacter();
        }
        GUI.EndGroup();
    }

    public void createCharacter()
    {
		PlayerScript playerScript;
		Debug.Log("Player class: " + playerClass);
		// if(playerClass == "Mage") {
			GameObject mageObject = (GameObject)Resources.Load("Prefabs/Mage");
			playerScript = Instantiate(mageObject).GetComponent<MageScript>();
		// }
		// else if(playerClass == "Ranger") {
		// 	GameObject rangerObject = (GameObject)Resources.Load("Prefabs/Ranger");
		// 	playerScript = Instantiate(rangerObject).GetComponent<RangerScript>();
		// }
		// else {
		// 	GameObject warriorObject = (GameObject)Resources.Load("Prefabs/Warrior");
		// 	playerScript = Instantiate(warriorObject).GetComponent<WarriorScript>();
		// }
		playerScript.setStrength(strength);
		playerScript.setAgility(agility);
		playerScript.setIntelligence(intelligence);
		playerScript.fullHeal();
		Destroy(emptyPlayer);
		Destroy(this.gameObject);
        SceneManager.LoadScene("NewIntro", LoadSceneMode.Single);
    }
}
