using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {
    
    PlayerScript playerScript;
    AudioControlScript audioControl;
    GameRestClient restClient;
    
    void Awake()
    {
        Debug.Log("GameScript Awake");

        DontDestroyOnLoad(transform.gameObject);

        ItemHandler.loadItemsManually();
        EnemyHandler.loadEnemiesManually();
    }

    protected void Start()
    {
        Debug.Log("GameScript Start");
        

        // restClient = new GameRestClient(this);

        // playerScript = player.GetComponent<PlayerScript>();
        audioControl = GetComponentInChildren<AudioControlScript>();
    }

    void Update()
    {
        checkInput();
    }

    void checkInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
        }
    }

    void OnGUI()
    {
       
    }

    public GameRestClient getRestClient() {
        return restClient;
    }

    public void getItems() {
        IEnumerator getter = restClient.getItems();
        StartCoroutine(getter);
    }
}
