using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour {

    public Button
        newProject,  openExisting, confirmBody, cancelBody,
        confirmOpen, cancelOpen,   maleBody,    femaleBody;

    public TMP_InputField projectName;

    public GameObject mainPanel, bodyChoosePanel, projectChoosePanel;

    public Button buttonPrefab;
    public Transform  scrollPanel;

    private BodyType bodyType = BodyType.MALE;
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
        GlobalParams.Map.Add("bodyType",    bodyType);
        GlobalParams.Map.Add("projectName", projectName.text);

        SceneManager.LoadScene("MainScene");
    }
    void CancelBodyOnClick() {
        bodyChoosePanel.SetActive(false);
        mainPanel      .SetActive(true);
    }
    void ConfirmOpenOnClick() {
        GlobalParams.Map.Add("mode", "load");
        GlobalParams.Map.Add("projectName", selectedFile);
        SceneManager.LoadScene("MainScene");
    }
    void CancelOpenOnClick() {
        foreach (Transform child in scrollPanel) Destroy(child.gameObject);
        projectChoosePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    void MaleBodyOnClick()   { bodyType = BodyType.MALE; }
    void FemaleBodyOnClick() { bodyType = BodyType.FEMALE; }

}
