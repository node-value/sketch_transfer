using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ProjectConnectionController : MonoBehaviour {

    private WebSocket wsCheck, wsInitial, wsDelete, wsMove, wsAdd;

    public Transform refObject;
    public GameObject prefab;
    public GameObject toolBox;


    void Start() {
        wsCheck   = ConnectToWebSocket(wsCheck,   HandleCheckMessage,   "check");
        wsInitial = ConnectToWebSocket(wsInitial, HandleInitialMessage, "initial");
        wsDelete  = ConnectToWebSocket(wsDelete,  HandleDeleteMessage,  "delete");
        wsAdd     = ConnectToWebSocket(wsAdd,     HandleAddMessage,     "add");
        wsMove    = ConnectToWebSocket(wsMove,    HandleMoveMessage,    "move");

        if (GlobalParams.Map.ContainsKey("mode") && (AppMode)GlobalParams.Map["mode"] == AppMode.CONNECT)
            SendCheckData();
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
            if (!GlobalParams.Map.ContainsKey("masterUsername")) GlobalParams.Map["masterUsername"] = response.sender;
            Debug.Log("Sending initial data");
            SendInitialData(response, refObject);
        } else {
            Debug.Log("Requested master id offline, either connect to another or load in master mode");
            //SceneManager.LoadScene("Menu");
            //SceneManager.UnloadSceneAsync("MainScene");
        }
    }

    void HandleInitialMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received initial data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            UnityMainThreadDispatcher.Instance().Enqueue(() => PersistanceManager.SetProjectData(response.data, refObject, prefab));
        } else {
            Debug.Log("Requested master is offline, either connect to another or load in master mode");
        }
    }

    void HandleDeleteMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received delete data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            Destroy(refObject.GetChild(int.Parse(response.data)).gameObject);
            toolBox.GetComponent<ToolBoxController>().Reset();
        } else {
            Debug.Log("Connection failed");
        }
    }

    void HandleAddMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received Add data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            UnityMainThreadDispatcher.Instance().Enqueue(() => CreateProjector(JsonUtility.FromJson<SketchData>(response.data), refObject, prefab));
        } else {
            Debug.Log("Connection failed");
        }
    }

    void HandleMoveMessage(object _, MessageEventArgs e) {
        ProjectDataDTO response = JsonUtility.FromJson<ProjectDataDTO>(e.Data);
        Debug.Log("Received Add data from user: " + response.sender + " ok size: " + response.data.Length);
        if (response.data != "FAILED") {
            ProjectorDataDTO moveData = JsonUtility.FromJson<ProjectorDataDTO>(response.data);
            Transform child = refObject.GetChild(moveData.Index);
            child.SetPositionAndRotation(moveData.Position, moveData.Rotation);
            child.localScale = moveData.Scale;
            child.gameObject.GetComponent<DecalProjector>().size = moveData.Scale;
            
        } else {
            Debug.Log("Connection failed");
        }
    }

    IEnumerator SendData(WebSocket socket, string sender, string receiver, string data) {
        yield return null;
        socket.Send(JsonUtility.ToJson(new ProjectDataDTO(sender, receiver, data)));
    }
    private IEnumerator SendDataInit(WebSocket socket, string sender, string receiver, Transform refer) {
        yield return null;
        socket.Send(JsonUtility.ToJson(new ProjectDataDTO(sender, receiver, PersistanceManager.GetProjectDataRaw(refer))));
    }

    private void SendCheckData() {
        UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(
                wsCheck, (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], "GET_INITIAL"));
    }

    private void SendInitialData(ProjectDataDTO response, Transform referObject) {
        UnityMainThreadDispatcher.Instance().Enqueue(
            SendDataInit(wsInitial, response.receiver, response.sender, referObject));
    }

    public void SendDeleteData(int childN) {
        UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(
                wsDelete, (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], childN+""));
    }

    public void SendAddData(SketchData data) {
        UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(wsAdd, (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], JsonUtility.ToJson(data)));
    }

    public void SendMoveData(ProjectorDataDTO data) {
        UnityMainThreadDispatcher.Instance().Enqueue(
            SendData(wsMove, (string)GlobalParams.Map["username"], (string)GlobalParams.Map["masterUsername"], JsonUtility.ToJson(data)));
    }



    private Texture2D CreateTexture(byte[] data) {
        Texture2D texture = new(2, 2); texture.LoadImage(data);
        return texture;
    }

    private void CreateProjector(SketchData data, Transform refObj, GameObject prefab) {
        DecalProjector projector = UnityEngine.Object.Instantiate(prefab, data.Position, data.Rotation, refObj).GetComponent<DecalProjector>();
        projector.material = Material.Instantiate(projector.material);
        projector.material.SetTexture("Base_Map", CreateTexture(data.Texture));
        projector.size = data.Scale;
    }


    /*
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
    

    IEnumerator SendDeleteData(int childN) {
        yield return null;
        wsDelete.Send(JsonUtility.ToJson(
            new ProjectDataDTO(
                    GlobalParams.Map["username"] as string,
                    GlobalParams.Map["masterUsername"] as string,
                    childN + ""
        )));
        Debug.Log("Sending delete data");
    }

    IEnumerator CheckOnStart(WebSocket soket) {
        yield return null;
        soket.Send(JsonUtility.ToJson(
            new ProjectDataDTO(
                GlobalParams.Map["username"] as string,
                GlobalParams.Map["masterUsername"] as string,
                "GET_INITIAL"
        )));
        Debug.Log("Sent to receiver with name: " + GlobalParams.Map["masterUsername"] as string);
    }
    */
}