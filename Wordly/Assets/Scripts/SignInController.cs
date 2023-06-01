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
    Requester requester;

    private string role = "Estudiante";
    private string endpoint = "api/auth/register_student";

    public TMP_InputField User { get => userNameInput; set => userNameInput = value; }
    public TMP_InputField Password { get => passwordInput; set => passwordInput = value; }
    public TMP_InputField ConfirmPassword { get => confirmPasswordInput; set => confirmPasswordInput = value; }
    public Requester Requester { get => requester; set => requester = value; }

    private void Start()
    {
        Requester = GameObject.Find("App").GetComponent<Requester>();
        Password.contentType = TMP_InputField.ContentType.Password;
        ConfirmPassword.contentType = TMP_InputField.ContentType.Password;
    }

    public void HandleRole(int value)
    {
        if (value == 0)
        {
            role = "Estudiante";
            endpoint = "api/auth/register_student";
        }
        else if (value == 1)
        {
            role = "Instructor";
            endpoint = "api/auth/register_tutor";
        }
    }

    public bool CheckEmailFormat(string value)
    {
        if (value.Contains("@hotmail.com") || value.Contains("@gmail.com") || value.Contains("@unal.edu.co"))
        {
            return true;
        }
        return false;
    }

    public void SignIn()
    {
        string email = this.User.text;
        string password = this.Password.text;
        string confirmPassword = this.ConfirmPassword.text;
        bool isEmailValid = CheckEmailFormat(email);

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.Log("Please Fill all fields");
        }
        else if (!isEmailValid) 
        {
            Debug.Log("Error in email format : " + email);
        }
        else if (password != confirmPassword)
        {
            Debug.Log("Passwords don't match");
        }
        else
        {
            StartCoroutine(RegisterUser(email, password));
        }
    }


    public IEnumerator RegisterUser(string email, string password)
    {
        Dictionary<string, string> requestBody = new Dictionary<string, string>();

        int index = email.IndexOf("@");
        string user = email.Substring(0, index);
        string endpoint = "";

        requestBody.Add("username", user);
        requestBody.Add("email", email);
        requestBody.Add("name", "usuario");
        requestBody.Add("last_name", "usuario");
        requestBody.Add("gender", "O");
        requestBody.Add("birthday", "2001-01-01");
        requestBody.Add("password", password);
        requestBody.Add("payment", "0000000000000000");
        requestBody.Add("ccv", "123");
        requestBody.Add("expire_date", "2000-01-01");

        if (role == "Instructor")
        {
            requestBody.Add("description", "a");
            endpoint = "api/auth/register_tutor";
        }
        else
        {
            endpoint = "api/auth/register_student";
        }

        OperationResult<UserLogin> operation = requester.PostOperation<UserLogin>($"https://wordly-zgzi.onrender.com/{endpoint}", requestBody);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            PlayerPrefs.SetString("Authorization", "Token " + operation.Data.token);

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
        else
        {
            Debug.LogError(operation.ErrorMessage);
        }
    }
}
