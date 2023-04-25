using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {

    public Button         send;
    public Transform      parent;
    public GameObject     content;
    public GameObject     userPrefab;
    public GameObject     incomePrefab;
    public TMP_InputField input;
    public GameObject     connection;

    void Start() {
        send.onClick.AddListener(OnClick);
    }

    void OnClick() {
        if (!string.IsNullOrEmpty(input.text)) {
            connection.GetComponent<ProjectConnectionController>().SendMessageData(input.text);
            CreateMessagePanel(input.text, userPrefab);
            input.text = "";
        }
    }

    public void CreateIncomeMessage(string message) {
        CreateMessagePanel(message, incomePrefab);
    }

    private void CreateMessagePanel(string message, GameObject prefab) {    
        GameObject newMessage = Instantiate(prefab, parent);
        newMessage.GetComponent<FitPanelToText>().SetMessage(message);
        var size = content.GetComponent<RectTransform>().sizeDelta;
        content.GetComponent<RectTransform>().sizeDelta = new(
            size.x,
            size.y + newMessage.GetComponent<RectTransform>().sizeDelta.y);
        
    }


}
