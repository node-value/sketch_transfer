using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ProjectConnectionController : MonoBehaviour {

    private WebSocket ws;
    public Button send;

    void Start() {
        Connect();
        SendOnStart();
    }

    void Connect() {
        if (GlobalParams.Map.ContainsKey("token")) {
            ws = new WebSocket(GlobalParams.Map["projectURL"] as string);
            ws.SetCookie(new Cookie("Bearer", GlobalParams.Map["token"] as string));
            ws.OnOpen    += (sender, e) => Debug.Log("Connection opened");
            ws.OnMessage += HandleMessage;
            
            ws.Connect();
        }
    }

    void HandleMessage(object sender, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        if (response.type == ProjectDataMsgType.CHECK) {
            if (response.data == "OK") {
                Debug.Log("Requested master is online");
            } else {
                Debug.Log("Requested master id offline, either connect to another or load in master mode");
                SceneManager.LoadScene("Menu");
                SceneManager.UnloadSceneAsync("MainScene");
            }
        }
    }

    void SendOnStart() {
        if (GlobalParams.Map.ContainsKey("mode") && (AppMode) GlobalParams.Map["mode"] == AppMode.CONNECT) {
            ws.Send(JsonUtility.ToJson(new ProjectDataDTO(ProjectDataMsgType.CHECK, GlobalParams.Map["masterUsername"] as string, "")));
            Debug.Log("Sent to receiver with name: " + GlobalParams.Map["masterUsername"] as string);
        } else {
            Debug.Log("SendOnStart failed");
        }
    }

    void SendOnClick() {
        //ws.Send(JsonUtility.ToJson(new ProjectDataDTO("user1", "hello!")));
    }
}
