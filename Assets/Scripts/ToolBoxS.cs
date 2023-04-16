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

    private DecalProjector currProjector;

    private ToolBoxState state = ToolBoxState.NONE;
    void Start() {
        addButton.   onClick.AddListener(AddTattooOnClick);
        pickButton.  onClick.AddListener(() => state = ToolBoxState.PICK);
        viewButton.  onClick.AddListener(() => state = ToolBoxState.VIEW);
        removeButton.onClick.AddListener(() => state = ToolBoxState.REMOVE);
    }

    void Update() {
        RelocateTatto();
        PickTattoo();
        EditTattoo();
        DeleteTattoo();
    }

    void RelocateTatto() {
        if (( state == ToolBoxState.ADD || state == ToolBoxState.VIEW )  && currProjector != null) {
            RelocateCast(state);
            if (Input.GetMouseButtonDown(0)) state = ToolBoxState.PICK;  
        }
    }

    void EditTattoo() {
        if (state == ToolBoxState.PICK && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = currProjector;
            if (!scetchEditor.GetComponentInParent<MyDropdown>().dropPanel.activeSelf)
                 scetchEditor.GetComponentInParent<MyDropdown>().HandleClick();
            currProjector = null;
            state = ToolBoxState.NONE;
        }
    }

    void DeleteTattoo() {
        if (state == ToolBoxState.REMOVE && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = null;
            Destroy(currProjector.gameObject);
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

    void RelocateCast(ToolBoxState state) {
        int layerMask = 1 << LayerMask.NameToLayer("Model");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            currProjector.transform.SetPositionAndRotation(
                hit.point + hit.normal * 0.01f, 
                Quaternion.LookRotation(
                    hit.normal, 
                    state == ToolBoxState.ADD ? 
                        Vector3.up : currProjector.transform.up));
    }

    void PickCast() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) 
            currProjector = hit.collider.GetComponent<DecalProjector>();
    }

    void AddTattooOnClick() {
        string path = EditorUtility.OpenFilePanel("Open Image", "", "png,jpg,jpeg");
        if (path != null) CreateProjector(CreateTexture2D(path)); 
        state = ToolBoxState.ADD;
    }

    void CreateProjector(Texture2D texture) { 
        currProjector = Instantiate(projectorPrefab, new(0,0,0), new(), referenceObject).GetComponent<DecalProjector>();
        currProjector.material = Material.Instantiate(currProjector.material);
        currProjector.material.SetTexture("Base_Map", texture); 
    }

    Texture2D CreateTexture2D(string path) {
        Texture2D texture = new(2, 2);
        texture.LoadImage(File.ReadAllBytes(path));
        return texture;
    } 
}