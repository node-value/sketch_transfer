using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering.Universal;
                
public class ToolBoxS : MonoBehaviour {

    public Button     addButton, removeButton, pickButton, viewButton, zoomInButton, zoomOutButton, saveButton;
    public Transform  referenceObject;
    public GameObject projectorPrefab;
    public GameObject scetchEditor;

    private Texture2D      texture;
    private DecalProjector currProjector;

    private ToolBoxState state = ToolBoxState.NONE;
    void Start() {
        addButton.onClick.AddListener(AddTattooOnClick);
        pickButton.onClick.AddListener(PickTattooOnClick);
        viewButton.onClick.AddListener(ViewTattoOnClick);
    }

    void Update() {
        RelocateTatto();
        PickTattoo();
        EditTattoo();
    }

    void RelocateTatto() {
        if (( state == ToolBoxState.ADD || state == ToolBoxState.VIEW )  && currProjector != null) {
            RelocateCast();
            if (Input.GetMouseButtonDown(0)) state = ToolBoxState.PICK;  
        }
    }

    void EditTattoo() {
        if (state == ToolBoxState.PICK && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = currProjector;
            currProjector = null;
            state = ToolBoxState.NONE;
        }
    }

    void PickTattoo() {
        if (( state == ToolBoxState.PICK || 
              state == ToolBoxState.VIEW || 
              state == ToolBoxState.REMOVE ) && Input.GetMouseButtonDown(0)) {
            PickCast();
        }
    }

    void RelocateCast() {
        int layerMask = 1 << LayerMask.NameToLayer("Model");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            currProjector.transform.SetPositionAndRotation(hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal, Vector3.up));
    }

    void PickCast() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) 
            currProjector = hit.collider.GetComponent<DecalProjector>();
    }


    void AddTattooOnClick() {
        state = ToolBoxState.ADD;
        string path = EditorUtility.OpenFilePanel("Open Image", "", "png,jpg,jpeg");
        if (path != null) {
            texture = CreateTexture2D(path);
            CreateProjector(texture);
        }
    }
    void PickTattooOnClick() => state = ToolBoxState.PICK;
    void ViewTattoOnClick()  => state = ToolBoxState.VIEW;

    void CreateProjector(Texture2D texture) { 
        currProjector = Instantiate(projectorPrefab, new Vector3(0, 0, 0), new Quaternion(), referenceObject).GetComponent<DecalProjector>();
        currProjector.material = Material.Instantiate(currProjector.material);
        currProjector.material.SetTexture("Base_Map", texture);
        currProjector.GetComponent<ProjectorS>().State = ScetchState.EDIT;
        
    }

    Texture2D CreateTexture2D(string path) {
        Texture2D texture = new(2, 2);
        texture.LoadImage(File.ReadAllBytes(path));
        return texture;
    } 
}