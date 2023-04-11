using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointPicker : MonoBehaviour {

    public GameObject dropPanel;
    public Button arrowButton;
    //bool isDroped = false;

    public void Start() {
        dropPanel.SetActive(false);
        arrowButton.onClick.AddListener(HandleClick);
    }

    public void HandleClick() {
        //isDroped = !isDroped;
        arrowButton.image.rectTransform.Rotate(0 ,0, 180);
        dropPanel.SetActive(!dropPanel.activeSelf);
    }

    // Update is called once per frame
    void Update() { }
}
