using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using SimpleFileBrowser;

public class ToolBoxController : MonoBehaviour {

    public Button     
        addButton,  removeButton, pickButton, 
        viewButton, zoomInButton, zoomOutButton, 
        saveButton, hideMenuButton;
    
    public GameObject projectorPrefab, scetchEditor, pointer;
    public Transform  referenceObject;
    public TMPro.TextMeshProUGUI projectName;

    private DecalProjector currProjector;

    private float        moveTime     = 0.0f;
    private ToolBoxState state        = ToolBoxState.NONE;
    private bool         isRelocating = false;

    void Start() {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".png");

        pointer.SetActive(false);
        if (GlobalParams.Map.ContainsKey("projectName"))
            projectName.text = GlobalParams.Map["projectName"] as string;

        addButton    .onClick.AddListener(AddTattooOnClick);
        pickButton   .onClick.AddListener(() => state = ToolBoxState.PICK);
        viewButton   .onClick.AddListener(() => state = ToolBoxState.VIEW);
        removeButton .onClick.AddListener(() => state = ToolBoxState.REMOVE);
        saveButton   .onClick.AddListener(() => PersistanceManager.Save(referenceObject));
        zoomInButton .onClick.AddListener(() => Camera.main.GetComponent<CameraController>().Zoom(-1f));
        zoomOutButton.onClick.AddListener(() => Camera.main.GetComponent<CameraController>().Zoom(1f));
    }

    void Update() {
        RelocateTatto ();
        PickTattoo    ();
        EditTattoo    ();
        DeleteTattoo  ();
        DisplayPointer();
    }
    void DisplayPointer() {
        pointer.SetActive(state != ToolBoxState.NONE);
        if (state != ToolBoxState.NONE && currProjector != null) {
           // pointer.GetComponent<FadeController>().SetState(FadeState.IN);
            pointer.transform.SetPositionAndRotation(
                currProjector.transform.position + currProjector.transform.forward * 0.25f,
                Quaternion.LookRotation(currProjector.transform.forward));

            pointer.transform.RotateAround(pointer.transform.position, pointer.transform.forward, Time.time * 100f);
            pointer.transform.position = pointer.transform.position + 0.07f * Mathf.Sin(moveTime += Time.deltaTime * 3.0f) * pointer.transform.forward;
              
        }
    }

    void RelocateTatto() {
        if (( state == ToolBoxState.ADD || state == ToolBoxState.VIEW )  && currProjector != null) {
            isRelocating   = true;
            Cursor.visible = false;
            RelocateCast(state);
            if (Input.GetMouseButtonDown(0)) {
                Cursor.visible = true; 
                state = ToolBoxState.PICK;
            }  
        }
    }

    void EditTattoo() {
        if (state == ToolBoxState.PICK && currProjector != null) {
            scetchEditor.GetComponent<ScetchEditorS>().Projector = currProjector;
            if (!scetchEditor.GetComponentInParent<MyDropdown>().dropPanel.activeSelf)
                scetchEditor.GetComponentInParent<MyDropdown>().HandleClick();
            isRelocating = false;
        }
    }

    void DeleteTattoo() {
        if (state == ToolBoxState.REMOVE && currProjector != null) {
            Destroy(currProjector.gameObject);
            Reset();
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Sketches"))) {
            DecalProjector hitProj = hit.collider.GetComponent<DecalProjector>();
            if (hitProj.Equals(currProjector) && !isRelocating) Reset();
            else currProjector = hitProj;
        }     
    }
    void Reset() {
        if (scetchEditor.GetComponentInParent<MyDropdown>().dropPanel.activeSelf)
            scetchEditor.GetComponentInParent<MyDropdown>().HandleClick();
        scetchEditor.GetComponent<ScetchEditorS>().Projector = null;
        currProjector = null;
        state = ToolBoxState.NONE;
        //pointer.GetComponent<FadeController>().SetState(FadeState.OUT);
        pointer.transform.position = Vector3.zero;
    }

    void AddTattooOnClick() {
        FileBrowser.ShowLoadDialog(
            (paths) => CreateProjector(CreateTexture2D(paths[0])), null, FileBrowser.PickMode.Files);
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