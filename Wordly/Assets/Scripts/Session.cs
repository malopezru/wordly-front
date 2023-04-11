using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Session : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField passwordInput;
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

        requestBody.Add("username", caseSensitiveUser);
        requestBody.Add("password", password);

        OperationResult<UserLogin> operation = requester.PostOperation<UserLogin>("http://127.0.0.1:8000/api/user/login/", requestBody);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            PlayerPrefs.SetString("Authorization", "Bearer " + operation.Data.token.access);
        }
        else
        {

        }
    }
}
