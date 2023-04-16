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

    void Start() {
        scaleSlider .onValueChanged.AddListener(OnScaleChanged);
        rotateSlider.onValueChanged.AddListener(OnRotateChanged);
        depthSlider .onValueChanged.AddListener(OnDepthChanged);
    }

    void OnProjectorChanged(DecalProjector value) {
        if (value == null) {
            scetch.GetComponent<Image>().sprite = null;
        } else {
            scaleSlider.value = Projector.size.x;
            rotateSlider.value = Projector.transform.rotation.eulerAngles.z;
            depthSlider.value = Projector.size.z;
            Texture t = Projector.material.GetTexture("Base_Map");
            scetch.GetComponent<Image>().sprite = Sprite.Create((Texture2D)t, new(0, 0, t.width, t.height), Vector2.one * 0.5f); 
        }
        SwitchEditPanel(value != null);
    }

    void OnScaleChanged(float scale) {
        Projector.size = new(scale, scale, depthSlider.value);
        Projector.transform.localScale = new(scale,scale, depthSlider.value);
    }

    void OnRotateChanged(float value) {
        Projector.transform.rotation = Quaternion.LookRotation(Projector.transform.forward) * Quaternion.Euler(0,0,value);
    }

    void OnDepthChanged(float depth) {
        Projector.size = new(scaleSlider.value, scaleSlider.value, depth);
        Projector.transform.localScale = new(scaleSlider.value, scaleSlider.value, depth);
    }

    void SwitchEditPanel(bool on) {
        editPanel.SetActive(on); emptyPanel.SetActive(!on); 
    }

}
