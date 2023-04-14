using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using System.Diagnostics;
using UnityEngine.Rendering.Universal;

public class ToolBoxS : MonoBehaviour {
    public Button addButton;

    public Transform referenceObject;

    public GameObject projectorPrefab;

    private Texture2D texture;

    bool isAddSelected = false;

    void Start() {
        addButton.onClick.AddListener(AddTattooOnClick);
    }

    void Update() {
        RayCastTatto();
    }

    void RayCastTatto() {
        if (isAddSelected && texture != null) {
            if (Input.GetMouseButtonDown(0)) {
                int layerMask = 1 << LayerMask.NameToLayer("Model");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) {
                    projectorPrefab.GetComponent<DecalProjector>().material.SetTexture("Base_Map", texture);
                    Instantiate(projectorPrefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal, Vector3.up), referenceObject);
                } isAddSelected = false;
            }
        }
    }

    void AddTattooOnClick() {
        isAddSelected = true;
        string path = EditorUtility.OpenFilePanel("Open Image", "", "png,jpg,jpeg");
        if (path != null) {
            texture = new(2, 2);
            texture.LoadImage(File.ReadAllBytes(path));
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            texture.anisoLevel = 1;
            texture.Apply(true);
        }
    }
}