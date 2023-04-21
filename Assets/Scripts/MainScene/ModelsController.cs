using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsController : MonoBehaviour {

    public GameObject aModel, bModel;

    void Start() {
        if (GlobalParams.Map.ContainsKey("bodyType")) {
            switchPanels((BodyType)GlobalParams.Map["bodyType"] == BodyType.MALE);
        }
    }

    private void switchPanels(bool on) {
        aModel.SetActive(on); bModel.SetActive(!on);
    }
}
