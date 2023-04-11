using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointPicker : MonoBehaviour {
    public Camera MainCamera;
    public Button HeadButton, ChestButton, StomachButton, ShoulderRButton,
        ShoulderLButton, ArmRButton, ArmLButton, HipButton, ThighLButton,
        ThighRButton, CalfLButton, CalfRButton;



    void Start() {
        HeadButton     .onClick.AddListener(() => ClickHandler(HeadButton.GetComponent<HeadButtonS>().Head)); 
        ChestButton    .onClick.AddListener(() => ClickHandler(ChestButton.GetComponent<ChestButtonS>().Chest)); 
        StomachButton  .onClick.AddListener(() => ClickHandler(StomachButton.GetComponent<StomachButtonS>().Stomach)); 
        ShoulderRButton.onClick.AddListener(() => ClickHandler(ShoulderRButton.GetComponent<ShoulderRButtonS>().ShoulderR));
        ShoulderLButton.onClick.AddListener(() => ClickHandler(ShoulderLButton.GetComponent<ShoulderLButtonS>().ShoulderL)); 
        ArmRButton     .onClick.AddListener(() => ClickHandler(ArmRButton.GetComponent<ArmRButtonS>().ArmR)); 
        ArmLButton     .onClick.AddListener(() => ClickHandler(ArmLButton.GetComponent<ArmLButtonS>().ArmL)); 
        HipButton      .onClick.AddListener(() => ClickHandler(HipButton.GetComponent<HipButtonS>().Hip)); 
        ThighLButton   .onClick.AddListener(() => ClickHandler(ThighLButton.GetComponent<ThighLButtonS>().ThighL));
        ThighRButton   .onClick.AddListener(() => ClickHandler(ThighRButton.GetComponent<ThighRButtonS>().ThighR)); 
        CalfLButton    .onClick.AddListener(() => ClickHandler(CalfLButton.GetComponent<CalfLButtonR>().CalfL)); 
        CalfRButton    .onClick.AddListener(() => ClickHandler(CalfRButton.GetComponent<CalfRButtonS>().CalfR));
    }

    void ClickHandler(GameObject obj) {
        MainCamera.GetComponent<CameraController>().SetCameraPivot(obj.GetComponent<Transform>());
        Debug.Log($"Child button clicked!");
    }

    void Update() { }
}
