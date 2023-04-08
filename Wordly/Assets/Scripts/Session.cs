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
        bool isEmailValid = CheckEmailFormat(user);
        
        if (!isEmailValid) 
        {
            Debug.Log("Error in email format");
        }
        else
        {
            StartCoroutine(GetUser("https://gorest.co.in/public/v2/users/464"));
            Debug.Log(user +  "   " + password );
        }

    }

    public bool CheckEmailFormat(string value)
    {
        if (value.Contains("@hotmail.") || value.Contains("@gmail."))
        {
            return true;
        }
        return false;
    }

    public IEnumerator GetUser(string url)
    {

        Dictionary<string, string> headers = new Dictionary<string, string>();
        OperationResult<User> operation = requester.GetOperation<User>(url, headers);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            Debug.Log("hola");
        }
    }
}