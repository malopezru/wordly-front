using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Session : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField passwordInput;
    public Button loginButton;

    public TMP_InputField User { get => userNameInput; set => userNameInput = value; }
    public TMP_InputField Password { get => passwordInput; set => passwordInput = value; }

    private void Start()
    {
        Password.contentType = TMP_InputField.ContentType.Password;
    }

    public void UserLogIn()
    {
        string user = this.User.text;
        string password = this.Password.text;

        if (!user.Contains("@hotmail") || !user.Contains("@gmail")) 
        {
            Debug.Log("Error in email format");
        }
        else
        {
            Debug.Log(user +  "   " + password );
        }

    }
}
