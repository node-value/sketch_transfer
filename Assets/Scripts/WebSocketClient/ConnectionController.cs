using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ConnectionController : MonoBehaviour {

    private WebSocket ws;
    public Button send;

    //private string token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1c2VyMCIsImlhdCI6MTY4MjA2NjU4MywiZXhwIjoxNjgyMDY5NDYzfQ.bUPsbgDUUInWsK0Kq0OBYEv731BglV3PIs2_tCh-XFU";

    void Start() {
        if (GlobalParams.Map.ContainsKey("token")) {
            ws = new WebSocket((string)GlobalParams.Map["chatURL"]);
            ws.SetCookie(new Cookie("Bearer", (string)GlobalParams.Map["token"]));
            ws.OnOpen += (sender, e) => Debug.Log("Connection opened");
            ws.OnMessage += (sender, e) => Debug.Log(e.Data);
            send.onClick.AddListener(SendOnClick);
            ws.Connect();
        }
    }

    void SendOnClick() {
        ws.Send(JsonUtility.ToJson(new MessageDTO(GlobalParams.Map["projectName"] as string, "test_project", "hello!")));
    }
}
