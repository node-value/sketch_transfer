using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowChatHeader : MonoBehaviour {

    public GameObject chat;
    public Button     button;
    public TMP_Text   text;

    private string show = "Show Chat", hide = "Hide Chat";
    
    private bool isShowing = false;

    void Start() {
        button.onClick.AddListener(ShowChatOnClick);
    }

    public void ShowChatOnClick() {
        chat.SetActive(isShowing = !isShowing);
        text.text = isShowing ? hide : show;
        float height = chat.GetComponent<RectTransform>().rect.height; //- button.GetComponent<RectTransform>().rect.height/2;
        button.GetComponent<RectTransform>().localPosition += 
            new Vector3(0, isShowing ? height : -height, 0);

    }
}
