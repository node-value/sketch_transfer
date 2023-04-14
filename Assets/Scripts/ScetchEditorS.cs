using UnityEngine;

public class ScetchEditorS : MonoBehaviour {
    public GameObject scaleSlider, rotateSlider, depthSlider;

    public GameObject emptyPanel, editPanel;
    private GameObject Projector { get;  set; }

    // Start is called before the first frame update
    void Start() {
        emptyPanel.SetActive(true);
        editPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }
}
