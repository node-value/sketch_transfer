using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuAuthController : MonoBehaviour {
    public GameObject authPanel, afterAuthPanel, signInPanel, signUpPanel;
    public Button signIn, signUp, toSignIn, toSignUp;
    public TMP_InputField inUsername, inPassword, upUsername, upPassword;

    void Start() {
        toSignIn.onClick.AddListener(ToSignInOnClick);
        toSignUp.onClick.AddListener(ToSignUpOnClick);

        signIn.onClick.AddListener(SignInOnClick);
        signUp.onClick.AddListener(SignUpOnClick);
    }

    void ToSignInOnClick() {
        signUpPanel.SetActive(false);
        signInPanel.SetActive(true);
    }
    void ToSignUpOnClick() {
        signInPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    void SignInOnClick() {
        if (inUsername.text != "" && inPassword.text != "") {
            StartCoroutine(HttpAuth.Login(inUsername.text, inPassword.text, authPanel, afterAuthPanel));
        } else {
            Debug.Log("Username and password must not be empty!");
        }
    }
    void SignUpOnClick() {
        if (upUsername.text != "" && upPassword.text != "") {
            StartCoroutine(HttpAuth.Login(upUsername.text, upPassword.text, authPanel, afterAuthPanel));
        } else {
            Debug.Log("Username and password must not be empty!");
        }
    }


}
