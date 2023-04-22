using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ProjectConnectionController : MonoBehaviour {

    private WebSocket wsCheck, wsInitial;

    public Transform refObject;
    public GameObject prefab;

    void Start() {
        wsCheck = ConnectToWebSocket(wsCheck, HandleCheckMessage, "check");
        wsInitial = ConnectToWebSocket(wsInitial, HandleInitialMessage, "initial");
        UnityMainThreadDispatcher.Instance().Enqueue(CheckOnStart(wsCheck));
    }

    WebSocket ConnectToWebSocket(WebSocket socket, EventHandler<MessageEventArgs> handler, string endPoint) {
        if (GlobalParams.Map.ContainsKey("token")) {
            socket = new WebSocket($"{GlobalParams.Map["projectBaseURL"] as string}/{endPoint}");
            socket.SetCookie(new Cookie("Bearer", GlobalParams.Map["token"] as string));
            socket.OnOpen += (sender, e) => Debug.Log($"Connection {endPoint} opened");
            socket.OnMessage += handler;
            socket.Connect();
            return socket;
        } Debug.Log("Auth token is absent, try to login again");
        return null;
    }

    void HandleCheckMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received a message: " + e.Data);
        if (response.data == "GET_INITIAL") {
            Debug.Log("Sending initial data");
            UnityMainThreadDispatcher.Instance().Enqueue(SendInitialData(response, refObject));
        } else {
            Debug.Log("Requested master id offline, either connect to another or load in master mode");
            //SceneManager.LoadScene("Menu");
            //SceneManager.UnloadSceneAsync("MainScene");
        }
    }

    IEnumerator SendInitialData(ProjectDataDTO response, Transform referObject) {
        yield return null;
        wsInitial.Send(JsonUtility.ToJson(
            new ProjectDataDTO(
                response.receiver,
                response.sender,
                PersistanceManager.GetProjectDataRaw(referObject)
        )));
        Debug.Log("Respond to initial request: Sent all project data");
    }

    void HandleInitialMessage(object sender, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);

        Debug.Log("Received initial data from user: " + response.sender + " ok size: " + response.data.Length);

        if (response.data != "FAILED") {
            Debug.Log("Start to convert data");
            UnityMainThreadDispatcher.Instance().Enqueue(() => PersistanceManager.SetProjectData(response.data, refObject, prefab));
            //PersistanceManager.SetProjectData(response.data, refObject, prefab);
            Debug.Log("Set data to project");
        } else {
            Debug.Log("Requested master is offline, either connect to another or load in master mode");
        }
    }

    IEnumerator CheckOnStart(WebSocket soket) {
        yield return null;
        if (GlobalParams.Map.ContainsKey("mode") && (AppMode) GlobalParams.Map["mode"] == AppMode.CONNECT) {
            soket.Send(JsonUtility.ToJson(
                new ProjectDataDTO(
                    GlobalParams.Map["username"] as string, 
                    GlobalParams.Map["masterUsername"] as string, 
                    "GET_INITIAL")));
            Debug.Log("Sent to receiver with name: " + GlobalParams.Map["masterUsername"] as string);
        }
    }
}

/*
void ConnectCheck() {
    if (GlobalParams.Map.ContainsKey("token")) {
        wsCheck = new WebSocket($"{GlobalParams.Map["projectBaseURL"] as string}/check");
        wsCheck.SetCookie(new Cookie("Bearer", GlobalParams.Map["token"] as string));
        wsCheck.OnOpen += (sender, e) => Debug.Log($"Connection {"check"} opened");
        wsCheck.OnMessage += HandleCheckMessage;
        wsCheck.Connect();
    } else Debug.Log("Auth token is absent, try to login again");
}
*/

/*
void HandleMessage(object sender, MessageEventArgs e) {
    ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
    Debug.Log("Received a message");
    if (response.type == ProjectDataMsgType.CHECK) {
        if (response.data == "OK") {
            Debug.Log("Requested master is online");
        } else {
            Debug.Log("Requested master id offline, either connect to another or load in master mode");
            SceneManager.LoadScene("Menu");
            SceneManager.UnloadSceneAsync("MainScene");
        }
    } 

    if (response.type == ProjectDataMsgType.INITIAL) {
        Debug.Log("Received initial request");
        if (response.data.Equals("GET_INITIAL")) {
            Debug.Log("Start stending project data");
            UnityMainThreadDispatcher.Instance().Enqueue(SendInitialData(response, refObject));
        } else {
            Debug.Log("Received initial data from user: " + response.sender + " ok size: " + response.data.Length);
            PersistanceManager.SetProjectData(response.data, refObject, prefab);
            Debug.Log("Set data to projext");
        }
    }
}
*/