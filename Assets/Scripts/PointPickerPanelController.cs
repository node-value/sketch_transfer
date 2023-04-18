using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickerPanelController : MonoBehaviour {
    
    public GameObject aIcon, bIcon;

    void Start() {
        if (GlobalParams.Map.ContainsKey("bodyType")) {
            switchPanels((BodyType) GlobalParams.Map["bodyType"] == BodyType.MALE);
        }
    }

    private void switchPanels(bool on) {
        aIcon.SetActive(on); bIcon.SetActive(!on);
    }
}
