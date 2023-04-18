using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public Button
        newProject,  openExisting, confirmBody, cancelBody,
        confirmOpen, cancelOpen,   maleBody,    femaleBody;

    public TMP_InputField projectName;

    public GameObject mainPanel, bodyChoosePanel, projectChoosePanel; 

    private BodyType bodyType = BodyType.MALE;

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
        GlobalParams.Map.Add("projectName", "Test1");
        SceneManager.LoadScene("MainScene");
    }
    void CancelOpenOnClick() {
        projectChoosePanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    void MaleBodyOnClick()   { bodyType = BodyType.MALE; }
    void FemaleBodyOnClick() { bodyType = BodyType.FEMALE; }

}
