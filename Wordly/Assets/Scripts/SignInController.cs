using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SignInController : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmPasswordInput;
    [SerializeField] Button signInButton;
    [SerializeField] GameObject studentManagementView;
    [SerializeField] GameObject instructorManagementView;

    private string role = "Estudiante";

    public TMP_InputField User { get => userNameInput; set => userNameInput = value; }
    public TMP_InputField Password { get => passwordInput; set => passwordInput = value; }
    public TMP_InputField ConfirmPassword { get => confirmPasswordInput; set => confirmPasswordInput = value; }

    private void Start()
    {
        Password.contentType = TMP_InputField.ContentType.Password;
        ConfirmPassword.contentType = TMP_InputField.ContentType.Password;
    }

    public void HandleRole(int value)
    {
        if (value == 0)
        {
            role = "Estudiante";
        }
        else if (value == 1)
        {
            role = "Instructor";
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

    public void SignIn()
    {
        string user = this.User.text;
        string password = this.Password.text;
        string confirmPassword = this.ConfirmPassword.text;
        bool isEmailValid = CheckEmailFormat(user);

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.Log("Please Fill all fields");
        }
        else if (!isEmailValid) 
        {
            Debug.Log("Error in email format : " + user);
        }
        else if (password != confirmPassword)
        {
            Debug.Log("Passwords don't match");
        }
        else
        {
            Debug.Log(user + " : " + password + " : " + confirmPassword + " : " + role);
            if (role == "Estudiante")
            {
                this.gameObject.SetActive(false);
                studentManagementView.SetActive(true);
            }
            else if (role == "Instructor")
            {
                this.gameObject.SetActive(false);
                instructorManagementView.SetActive(true);
            }
        }
    }

}
