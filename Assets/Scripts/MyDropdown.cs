using UnityEngine;
using UnityEngine.UI;

public class MyDropdown : MonoBehaviour {

    public GameObject dropPanel;
    public Button arrowButton;


    public void Start() {
        dropPanel.SetActive(false);
        arrowButton.onClick.AddListener(HandleClick);
    }

    public void HandleClick() {
        arrowButton.image.rectTransform.Rotate(0 ,0, 180);
        dropPanel.SetActive(!dropPanel.activeSelf);
    }
}
