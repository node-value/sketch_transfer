using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ToolBoxS : MonoBehaviour {

    public Button     addButton, removeButton, pickButton, viewButton, zoomInButton, zoomOutButton, saveButton;
    public Transform  referenceObject;
    public GameObject projectorPrefab;
    public GameObject scetchEditor;
    public GameObject pointer;

    private DecalProjector currProjector;
    private float moveTime = 0.0f;
    private ToolBoxState state = ToolBoxState.NONE;
    void Start() {
        pointer.SetActive(false);

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
        DisplayPointer();
        //Reset();
    }
    void DisplayPointer() {
        pointer.SetActive(state != ToolBoxState.NONE);
        if (state != ToolBoxState.NONE) {
            if (currProjector != null) {
                pointer.transform.SetPositionAndRotation(
                    currProjector.transform.position + currProjector.transform.forward * 0.25f,
                    Quaternion.LookRotation(currProjector.transform.forward));

                pointer.transform.RotateAround(pointer.transform.position, pointer.transform.forward, Time.time * 100f);
                pointer.transform.position = pointer.transform.position + 0.07f * Mathf.Sin(moveTime += Time.deltaTime * 3.0f) * pointer.transform.forward;
            }   
        }
    }

    void RelocateTatto() {
        if (( state == ToolBoxState.ADD || state == ToolBoxState.VIEW )  && currProjector != null) {
            Cursor.visible = false;
            RelocateCast(state);
            if (Input.GetMouseButtonDown(0)) { Cursor.visible = true; state = ToolBoxState.PICK;}  
        }
    }

    void EditTattoo() {
        if (state == ToolBoxState.PICK && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = currProjector;
            if (!scetchEditor.GetComponentInParent<MyDropdown>().dropPanel.activeSelf)
                scetchEditor.GetComponentInParent<MyDropdown>().HandleClick();
            //currProjector = null;
            //state = ToolBoxState.NONE;
        }
    }

/*
    void Reset() {
        if (DetectMouseOnDefault()) {
            currProjector = null;
            state = ToolBoxState.NONE;
            pointer.SetActive(false);
        }
    }
*/

    void DeleteTattoo() {
        if (state == ToolBoxState.REMOVE && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = null;
            Destroy(currProjector.gameObject);
            currProjector = null;
            state = ToolBoxState.NONE;
            pointer.SetActive(false);
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

/*
    bool DetectMouseOnDefault() {
        return Input.GetMouseButtonDown(0) && 
            Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition),
                out RaycastHit _, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Model")));
    }
*/

    void PickCast() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Sketches"))) 
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