using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsController : MonoBehaviour {

    public GameObject aModel, bModel;

    private bool isUpdating;

    void Start() {
        isUpdating = GlobalParams.Map.ContainsKey("mode") && (AppMode)GlobalParams.Map["mode"] == AppMode.CONNECT;
        if (GlobalParams.Map.ContainsKey("bodyType")) {
            SwitchPanels((BodyType)GlobalParams.Map["bodyType"] == BodyType.MALE);
        }
    }

    void Update() {
        if (isUpdating && GlobalParams.Map.ContainsKey("bodyType")) {
            SwitchPanels((BodyType)GlobalParams.Map["bodyType"] == BodyType.MALE);
            isUpdating = false;
        }
     }

    private void SwitchPanels(bool on) {
        aModel.SetActive(on); bModel.SetActive(!on);
    }
}
