using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Session : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] GameObject studentHomeView;
    [SerializeField] GameObject tutorHomeView;
    [SerializeField] public MessagePopUp popUp;
    Requester requester;
    public Button loginButton;

    public TMP_InputField User { get => userNameInput; set => userNameInput = value; }
    public TMP_InputField Password { get => passwordInput; set => passwordInput = value; }
    public Requester Requester { get => requester; set => requester = value; }

    private void Start()
    {
        Requester = GameObject.Find("App").GetComponent<Requester>();
        Password.contentType = TMP_InputField.ContentType.Password;
    }

    public void UserLogIn()
    {
        string user = this.User.text;
        string password = this.Password.text;
        
        StartCoroutine(LoginUser(user, password));
        Debug.Log(user +  "   " + password );

    }

    public IEnumerator LoginUser(string user, string password)
    {

        Dictionary<string, string> requestBody = new Dictionary<string, string>();
        
        string caseSensitiveUser = "";
        if (!string.IsNullOrEmpty(user))
        {
            caseSensitiveUser = $"{user[0].ToString().ToLower()}{user.Substring(1)}";
        }

        requestBody.Add("email", caseSensitiveUser);
        requestBody.Add("password", password);

        OperationResult<UserLogin> operation = requester.PostOperation<UserLogin>("http://localhost:8000/api/auth/login", requestBody);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            PlayerPrefs.SetString("Authorization", "Token " + operation.Data.token);
            if (operation.Data.user.user_type == "Student")
            {
                this.gameObject.SetActive(false);
                studentHomeView.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
                tutorHomeView.gameObject.SetActive(true);
            }
        }
        else
        {
            popUp.SetPopUpMessage("Contraseña o Usuario incorrectos", true);
        }
    }
}
