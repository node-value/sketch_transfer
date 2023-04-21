using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpAuth {

    public static IEnumerator Login(string username, string password, GameObject authPanel, GameObject afterAuthPanel) {
        using UnityWebRequest www = new(GlobalParams.Map["authURL"] as string, "POST") {
            uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new UserDTO(username, password)))),
            downloadHandler = new DownloadHandlerBuffer()
        };

        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError(www.error);
            yield break;
        }
        Debug.Log("Login successful!");
        GlobalParams.Map.Add("token", JsonUtility.FromJson<TokenDTO>(www.downloadHandler.text).token);
        authPanel.SetActive(false);
        afterAuthPanel.SetActive(true);
    }

    public static IEnumerator Register(string username, string password, GameObject authPanel, GameObject afterAuthPanel) {
        using UnityWebRequest www = new(GlobalParams.Map["regURL"] as string, "POST") {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new UserDTO(username, password)))),
            downloadHandler = new DownloadHandlerBuffer()
        };

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError(www.error);
            yield break;
        } 
        Debug.Log("Login successful!");
        GlobalParams.Map.Add("token", JsonUtility.FromJson<TokenDTO>(www.downloadHandler.text));
        authPanel.SetActive(false);
        afterAuthPanel.SetActive(true);
    }

}
