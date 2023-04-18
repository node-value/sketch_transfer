using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneStart : MonoBehaviour {

    public GameObject prefab;
    public Transform  refObj;

    void Start() {
        if (GlobalParams.Map.ContainsKey("mode") && ((string) GlobalParams.Map["mode"]) == "load")
            DataController.Load(refObj, prefab,(string) GlobalParams.Map["projectName"]);
    }

}
