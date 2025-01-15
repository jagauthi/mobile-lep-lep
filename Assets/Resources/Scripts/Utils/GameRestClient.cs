using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameRestClient {

    GameScript gameScript;

	public GameRestClient(GameScript gameScript) {
        this.gameScript = gameScript;
    }

    public IEnumerator getItems() {
        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:8080/game/getItems", "GET");
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
		yield return unityWebRequest.SendWebRequest();
 
        if(unityWebRequest.isNetworkError ||unityWebRequest.isHttpError) {
            Debug.Log("Error: " + unityWebRequest.error);
        }
        else {
            Debug.Log("Text: " + unityWebRequest.downloadHandler.text);
            gameScript.setItems(unityWebRequest.downloadHandler.text);
        }
    }

    public IEnumerator sendPost(Item item) {
        string jsonString = JsonUtility.ToJson(item);
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());
        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:8080/login", "POST");
		unityWebRequest.uploadHandler = new UploadHandlerRaw (byteData);
        unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
		unityWebRequest.SetRequestHeader ("Content-Type", "application/json");
		yield return unityWebRequest.SendWebRequest();
 
        if(unityWebRequest.isNetworkError ||unityWebRequest.isHttpError) {
            Debug.Log("Error: " + unityWebRequest.error);
        }
        else {
            gameScript.setItems(unityWebRequest.downloadHandler.text);
        }
    }
}
