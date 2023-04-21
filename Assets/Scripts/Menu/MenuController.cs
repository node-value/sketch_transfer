using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour {

    public Button
        newProject,   openExisting, confirmBody,  cancelBody,
        confirmOpen,  cancelOpen,   maleBody,     femaleBody,
        master,       client,       connect;

    public TMP_InputField projectName, masterUsername;

    public GameObject 
        roleChoosePanel, connectPanel, 
        mainPanel,       bodyChoosePanel, 
        projectChoosePanel;

    public Button buttonPrefab;
    public Transform  scrollPanel;

    private BodyType bodyType = BodyType.NONE;
    private string   selectedFile = null;

    void Start() {
        newProject  .onClick.AddListener(NewProjectOnClick);
        openExisting.onClick.AddListener(OpenExistingOnClick); 
        confirmBody .onClick.AddListener(ConfirmBodyOnClick);
        cancelBody  .onClick.AddListener(CancelBodyOnClick);
        cancelOpen  .onClick.AddListener(CancelOpenOnClick);
        confirmOpen .onClick.AddListener(ConfirmOpenOnClick); 
        maleBody    .onClick.AddListener(MaleBodyOnClick);    
        femaleBody  .onClick.AddListener(FemaleBodyOnClick);
        master      .onClick.AddListener(MasterOnClick);
        client      .onClick.AddListener(ClientOnClick);
        connect     .onClick.AddListener(ConnectOnClick);
    }
    void MasterOnClick() {
        roleChoosePanel.SetActive(false);
        mainPanel      .SetActive(true);
    }

    void ClientOnClick() {
        GlobalParams.Map.Add("mode", AppMode.CONNECT);
        roleChoosePanel.SetActive(false);
        connectPanel   .SetActive(true);
    }

    void ConnectOnClick() {
        if (masterUsername.text.Length != 0) {
            GlobalParams.Map.Add("masterUsername", masterUsername.text);
            connectPanel.SetActive(false);
            SceneManager.LoadScene("MainScene");
        } else Debug.Log("Username must not be empty");
    } 

    void NewProjectOnClick() { 
        mainPanel      .SetActive(false);
        bodyChoosePanel.SetActive(true);
    }
    void OpenExistingOnClick() {
        mainPanel         .SetActive(false);
        projectChoosePanel.SetActive(true);
        foreach (string name in PersistanceManager.GetFileNames()) {
            if (Path.GetExtension(name) == PersistanceManager.ext) {
                Button curr = Instantiate(buttonPrefab, scrollPanel);
                curr.GetComponentInChildren<TextMeshProUGUI>().text =
                       Path.GetFileNameWithoutExtension(name);
                curr.onClick.AddListener(() => selectedFile = curr.GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }

    void ConfirmBodyOnClick() {
        if (bodyType != BodyType.NONE && projectName.text.Length != 0) {
            GlobalParams.Map.Add("bodyType", bodyType);
            GlobalParams.Map.Add("projectName", projectName.text);

            SceneManager.LoadScene("MainScene");
        } else Debug.Log("Pick a body type and type a project name");
    }
    void CancelBodyOnClick() {
        bodyChoosePanel.SetActive(false);
        mainPanel      .SetActive(true);
    }
    void ConfirmOpenOnClick() {
        if (selectedFile != null) {
            GlobalParams.Map.Add("mode", AppMode.LOAD);
            GlobalParams.Map.Add("projectName", selectedFile);
            SceneManager.LoadScene("MainScene");
        } else Debug.Log("Pick existing project");
    }
    void CancelOpenOnClick() {
        foreach (Transform child in scrollPanel) Destroy(child.gameObject);
        projectChoosePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    void MaleBodyOnClick()   { bodyType = BodyType.MALE; }
    void FemaleBodyOnClick() { bodyType = BodyType.FEMALE; }

}
