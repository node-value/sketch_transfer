using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FitPanelToText : MonoBehaviour {

    public RectTransform panel;
    public TMP_Text text;

    public void SetMessage(string message) {
        text.text = message;
        float preferredHeight = LayoutUtility.GetPreferredHeight(text.rectTransform);
        panel.sizeDelta = new(panel.sizeDelta.x, Mathf.Max(preferredHeight, panel.sizeDelta.y));
    }
    public void Clear() {
        text.text = "";
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, 0);
    }
}
