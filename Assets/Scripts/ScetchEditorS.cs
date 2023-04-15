using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScetchEditorS : MonoBehaviour {
    public Slider scaleSlider, rotateSlider, depthSlider;

    public GameObject emptyPanel, editPanel, scetch;
    public DecalProjector Projector { 
        get { return _projector; }
        set { _projector = value; OnProjectorChanged(value); } 
    } private DecalProjector _projector;

    // Start is called before the first frame update
    void Start() {
        switchEditPanel(false);
        scaleSlider.onValueChanged.AddListener(OnScaleChanged);
        rotateSlider.onValueChanged.AddListener(OnRotateChanged);
        depthSlider.onValueChanged.AddListener(OnDepthChanged);
    }


    // Update is called once per frame
    void Update() {
        if (Projector != null) {
            
        } 
    }

    void OnProjectorChanged(DecalProjector value) {
        if (value == null) {
            scetch.GetComponent<Image>().sprite = null;
        } else {
            Texture t = Projector.material.GetTexture("Base_Map");
            scetch.GetComponent<Image>().sprite = Sprite.Create((Texture2D)t, new(0, 0, t.width, t.height), Vector2.one * 0.5f);   
        } switchEditPanel(value != null);
    }

    void OnScaleChanged(float value) {
        Projector.transform.localScale *= value;
    }
    void OnRotateChanged(float value) { }
    void OnDepthChanged(float value) { }


    void switchEditPanel(bool on) {
        editPanel.SetActive(on); emptyPanel.SetActive(!on); 
    }


}
