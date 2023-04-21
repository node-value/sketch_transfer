using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ProjectConnectionController : MonoBehaviour {

    private WebSocket ws;
    public Button send;

    void Start() {
        Connect();
    }

    void Connect() {
        if (GlobalParams.Map.ContainsKey("token")) {
            ws = new WebSocket(GlobalParams.Map["projectURL"] as string);
            ws.SetCookie(new Cookie("Bearer", GlobalParams.Map["token"] as string));
            ws.OnOpen    += (sender, e) => Debug.Log("Connection opened");
            ws.OnMessage += (sender, e) => Debug.Log(e.Data);
            send.onClick.AddListener(SendOnClick);
            ws.Connect();
        }
    }

    void SendOnClick() {
        ws.Send(JsonUtility.ToJson(new MessageDTO("user1", "hello!")));
    }
}
