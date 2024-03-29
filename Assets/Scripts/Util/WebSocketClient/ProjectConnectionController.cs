using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ProjectConnectionController : MonoBehaviour {

    private WebSocket wsCheck, wsInitial, wsDelete, wsMove, wsAdd, wsChat;

    public Transform refObject;
    public GameObject prefab;
    public GameObject toolBox;
    public GameObject loadingAnim;
    public GameObject chat;

    private bool isConnected;

    void Start() {
        isConnected = GlobalParams.Map.ContainsKey("mode") && (AppMode)GlobalParams.Map["mode"] == AppMode.CONNECT;
        loadingAnim.SetActive(isConnected && !GlobalParams.Map.ContainsKey("bodyType"));
        wsCheck = ConnectToWebSocket(wsCheck, HandleCheckMessage, "check");
        if (isConnected) {
            StartConnections();
            SendCheckData();
        }
    }

    void StartConnections() {
        isConnected = true;
        wsInitial = ConnectToWebSocket(wsInitial, HandleInitialMessage, "initial");
        wsDelete  = ConnectToWebSocket(wsDelete,  HandleDeleteMessage,  "delete");
        wsAdd     = ConnectToWebSocket(wsAdd,     HandleAddMessage,     "add");
        wsMove    = ConnectToWebSocket(wsMove,    HandleMoveMessage,    "move");
        wsChat    = ConnectToWebSocket(wsChat,    HandleChatMessage,    "chat");
        
    }

    WebSocket ConnectToWebSocket(WebSocket socket, EventHandler<MessageEventArgs> handler, string endPoint) {
        if (GlobalParams.Map.ContainsKey("token")) {
            socket = new WebSocket($"{GlobalParams.Map["projectBaseURL"] as string}/{endPoint}");
            socket.SetCookie(new Cookie("Bearer", GlobalParams.Map["token"] as string));
            socket.OnOpen += (sender, e) => Debug.Log($"Connection {endPoint} opened");
            socket.OnMessage += handler;
            socket.OnMessage += (_, e) => { if (e.IsPing) Debug.Log("received a ping"); };
            socket.Connect();
            return socket;
        } Debug.Log("Auth token is absent, try to login again");
        isConnected = false;
        return null;
    }

    void HandleChatMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received chat message " + response.data);
        if (response.data != "FAILED") {
            UnityMainThreadDispatcher.Instance().Enqueue(() => chat.GetComponent<ChatController>().CreateIncomeMessage(response.data));
        } else {
            isConnected = false;
            Debug.Log("Connection failed");
        }
    }

    void HandleCheckMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received a message: " + e.Data);
        if (response.data != "FAILED") {
            StartConnections();
            GlobalParams.Map["masterUsername"] = response.sender; 
            Debug.Log("Sending initial data");
            SendInitialData(response);
            SendAllChildren(refObject);
        } else {
            isConnected = false;
            Debug.Log("Requested master id offline, either connect to another or load in master mode");
            SceneManager.UnloadSceneAsync("MainScene");
            SceneManager.LoadScene("Menu");
        }
    }

    void SendAllChildren(Transform refObj) {
        for (int i = 0; i < refObj.childCount; i++) {
            var child = refObj.GetChild(i);
            SendAddData(new SketchData(
                child.position, child.rotation, child.GetComponent<DecalProjector>().size, 
                child.GetComponent<DecalProjector>().material.GetTexture("Base_Map")));
        }
    }

    void HandleInitialMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received initial data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            loadingAnim.SetActive(false);
            InitialDataDTO data = JsonUtility.FromJson<InitialDataDTO>(response.data);
            GlobalParams.Map["bodyType"]    = data.BodyType;
            GlobalParams.Map["projectName"] = data.Name;
        } else {
            isConnected = false;
            Debug.Log("Requested master is offline, either connect to another or load in master mode");
        }
    }

    void HandleDeleteMessage(object _, MessageEventArgs e) {
        toolBox.GetComponent<ToolBoxController>().Reset();
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received delete data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            Destroy(refObject.GetChild(int.Parse(response.data)).gameObject);
            toolBox.GetComponent<ToolBoxController>().Reset();
        } else {
            isConnected = false;
            Debug.Log("Connection failed");
        }
    }

    void HandleAddMessage(object _, MessageEventArgs e) {
        toolBox.GetComponent<ToolBoxController>().Reset();
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received Add data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            UnityMainThreadDispatcher.Instance().Enqueue(() => CreateProjector(JsonUtility.FromJson<SketchData>(response.data), refObject, prefab));
        } else {
            isConnected = false;
            Debug.Log("Connection failed");
        }
    }

    void HandleMoveMessage(object _, MessageEventArgs e) {
        toolBox.GetComponent<ToolBoxController>().Reset();
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received Add data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            ProjectorDataDTO moveData = JsonUtility.FromJson<ProjectorDataDTO>(response.data);
            Transform child = refObject.GetChild(moveData.Index);
            child.SetPositionAndRotation(moveData.Position, moveData.Rotation);
            child.localScale = moveData.Scale;
            child.gameObject.GetComponent<DecalProjector>().size = moveData.Scale;
        } else {
            isConnected = false;
            Debug.Log("Connection failed");
        }

    }

    IEnumerator SendData(WebSocket socket, string sender, string receiver, string data) {
        yield return null;
        socket.Send(JsonUtility.ToJson(new ProjectDataDTO(sender, receiver, data)));
    }

    private void SendCheckData() {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData( wsCheck, 
                (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], "GET_INITIAL"));
    }

    private void SendInitialData(ProjectDataDTO response) {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData( wsInitial, response.receiver, response.sender,
                JsonUtility.ToJson(
                    new InitialDataDTO((string)GlobalParams.Map["projectName"], (BodyType)GlobalParams.Map["bodyType"]))));
    }

    public void SendDeleteData(int childN) {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData( wsDelete, 
                (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], childN+""));
    }

    public void SendMessageData(string message) {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(wsChat,
                (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], message));
    }

    public void SendAddData(SketchData data) {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(wsAdd, 
                (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], JsonUtility.ToJson(data)));
    }

    public void SendMoveData(ProjectorDataDTO data) {
        if (isConnected) UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(wsMove, 
                (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], JsonUtility.ToJson(data)));
    }


    private Texture2D CreateTexture(byte[] data) {
        Texture2D texture = new(2, 2); texture.LoadImage(data);
        return texture;
    }

    private void CreateProjector(SketchData data, Transform refObj, GameObject prefab) {
        DecalProjector projector = UnityEngine.Object.Instantiate(prefab, data.Position, data.Rotation, refObj).GetComponent<DecalProjector>();
        projector.size = data.Scale;
        projector.transform.localScale = data.Scale;
        projector.material = Material.Instantiate(projector.material);
        Texture2D texture = CreateTexture(data.Texture);
        projector.material.SetTexture("Base_Map", texture);
        projector.gameObject.GetComponent<TextureHolder>().texture = ResizeTexture(texture);
        
    }

    private Texture2D ResizeTexture(Texture2D texture) {
        Texture2D result = new(texture.width / 4, texture.height / 4);
        Graphics.ConvertTexture(texture, result);
        return result;
    }
}