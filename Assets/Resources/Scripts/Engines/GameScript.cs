using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {
    
    int buffer;
    int buttonLength;
    int textLength;
    PlayerScript playerScript;
    AudioControlScript audioControl;
    GameObject player;
    GameRestClient restClient;
    ItemHandler itemHandler;
    
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    protected void Start()
    {
        Debug.Log("GameScript Start");
        // restClient = new GameRestClient(this);
        // itemHandler = new ItemHandler(this);
        // getItems();

        // playerScript = player.GetComponent<PlayerScript>();
        audioControl = GetComponentInChildren<AudioControlScript>();
        buffer = Screen.height / 16;
        buttonLength = Screen.height / 16;
        textLength = Screen.height / 8;
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

    public void setItems(String items) {
        itemHandler.parseItems(items);
        List<Item> allItems = itemHandler.getAllItems();
        print(allItems);
    }
}
