using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class AccountManagementController : MonoBehaviour
{
    [Header("Personal Info")]
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField lastNameInput;
    [SerializeField] TMP_InputField birthdayInput;
    [SerializeField] Button savePersonalInfo;

    [Header("Language Selection")]
    [SerializeField] Button frenchButton;
    [SerializeField] Button englishButton;
    [SerializeField] Button spanishButton;
    [SerializeField] Button japaneseButton;
    [SerializeField] Button germanButton;
    [SerializeField] Color buttonNormal;
    [SerializeField] Color buttonPressed;
    [SerializeField] LanguageLevel languageLevelPrefab;
    [SerializeField] Transform languageLevelContent;

    [Header("Payment Method")]
    [SerializeField] TMP_InputField cardNumberInput;
    [SerializeField] TMP_InputField expirationDateInput;
    [SerializeField] TMP_InputField cvvInput;
    [SerializeField] GameObject cardInfoPrefab;
    [SerializeField] Transform cardInfoContent;

    [Header("Management Selection")]
    [SerializeField] Button personalInfoButton;
    [SerializeField] Button languagesInfoButton;
    [SerializeField] Button paymentMethodsButton;
    [SerializeField] GameObject SelectedButtonBackground;

    [Header("Other")]
    [SerializeField] public MessagePopUp popUp;

    private string sex = "M";
    private string user_type = null;
    private string selectedLevel = null;
    private List<Language> languageList = new List<Language>();
    public List<string> selectedLanguages = null;
    public Dictionary<string, string> requestBody = new Dictionary<string, string>();
    Requester requester;

    public TMP_InputField Name { get => nameInput; set => nameInput = value; }
    public TMP_InputField LastName { get => lastNameInput; set => lastNameInput = value; }
    public TMP_InputField Birthday { get => birthdayInput; set => birthdayInput = value; }
    public TMP_InputField CardNumber { get => cardNumberInput; set => cardNumberInput = value; }
    public TMP_InputField ExpirationDate { get => expirationDateInput; set => expirationDateInput = value; }
    public TMP_InputField CVV { get => cvvInput; set => cvvInput = value; }
    public Requester Requester { get => requester; set => requester = value; }

    private void Awake()
    {
        frenchButton.onClick.AddListener(() => SetPressedButton(frenchButton, StringInArray(selectedLanguages, "french"), "french"));
        englishButton.onClick.AddListener(() => SetPressedButton(englishButton, StringInArray(selectedLanguages, "english"), "english"));
        spanishButton.onClick.AddListener(() => SetPressedButton(spanishButton, StringInArray(selectedLanguages, "spanish"), "spanish"));
        japaneseButton.onClick.AddListener(() => SetPressedButton(japaneseButton, StringInArray(selectedLanguages, "japanese"), "japanese"));
        germanButton.onClick.AddListener(() => SetPressedButton(germanButton, StringInArray(selectedLanguages, "german"), "german"));
    }

    private void Start()
    {
        Requester = GameObject.Find("App").GetComponent<Requester>();
        StartCoroutine(GetUserData());
    }

    public IEnumerator GetUserData()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        OperationResult<User> operation = Requester.GetOperation<User>("http://localhost:8000/api/profile/user_data", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            FillBodyFields(operation.Data);
        }
    }

    public void FillBodyFields(User userData)
    {
        requestBody.Add("name", userData.name);
        requestBody.Add("email", userData.email);
        requestBody.Add("last_name", userData.last_name);
        requestBody.Add("gender", userData.gender);
        user_type = userData.user_type;
    }

    public void SetPressedButton(Button button, bool isPressed, string languagePressed)
    {
        if (isPressed)
        {
            button.image.color = buttonNormal;
            var itemToRemove = selectedLanguages.Single(item => item == languagePressed);
            selectedLanguages.Remove(itemToRemove);
        }
        else
        {
            button.image.color = buttonPressed;
            selectedLanguages.Add(languagePressed);
        }
    }

    public bool StringInArray(List<string> strings, string word)
    {
        if (strings.Count != 0)
        {
            foreach (string s in strings)
            {
                if (s == word)
                {
                    return true;
                }
            }
        }
        return false;

    }

    public void MoveButtonBackground(Button button)
    {
        SelectedButtonBackground.SetActive(true);
        SelectedButtonBackground.GetComponent<RectTransform>().anchorMin = button.GetComponent<RectTransform>().anchorMin;
        SelectedButtonBackground.GetComponent<RectTransform>().anchorMax = button.GetComponent<RectTransform>().anchorMax;
    }

    public void HandleSex(int value)
    {
        if (value == 0)
        {
            sex = "M";
        }
        else if (value == 1)
        {
            sex = "F";
        }
        else if (value == 2) 
        {
            sex = "O";
        }
    }

    public bool IsDateTime(string value)
    {
        DateTime dateTime;

        if (DateTime.TryParse(value, out dateTime) && value[4] == '-' && value[7] == '-')
        {
            return true;
        }
        return false;
    }

    public void SavePersonalInfo()
    {
        string name = this.Name.text;
        string lastName = this.LastName.text;
        string birthday = this.Birthday.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(birthday))
        {
            popUp.SetPopUpMessage("Por favor llene todos los campos", true);
        }
        else if (!IsDateTime(birthday))
        {
            popUp.SetPopUpMessage("Fecha no válida", true);
        }
        else
        {
            StartCoroutine(PostPersonalInfo(name, lastName, birthday, sex));
        }
    }


    public IEnumerator PostPersonalInfo(string name, string lastName, string birthday, string gender)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        string endpoint = "update_data_student";

        if (user_type != "Student")
        {
            endpoint = "update_data_tutor";
        }

        requestBody["name"] = name;
        requestBody["last_name"] = lastName;
        requestBody["birthday"] = birthday;
        requestBody["gender"] = gender;
        requestBody["user_type"] = user_type;

        OperationResult<User> operation = Requester.PostOperation<User>($"http://localhost:8000/api/profile/{endpoint}", requestBody, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
    }

    public void SaveLanguagesInfo()
    {
        if (selectedLanguages.Count > 0)
        {
            foreach (string language in selectedLanguages)
            {
                Dictionary<string, string> languagesBody = new Dictionary<string, string>(){
                    { "name", language },
                    { "level", "a1" }
                };
                StartCoroutine(PostLanguagesInfo(languagesBody));
            }
        }
        else
        {
            popUp.SetPopUpMessage("Seleccione al menos un idioma", false);
        }
    }

    IEnumerator PostLanguagesInfo(Dictionary<string, string> body)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<Language> operation = Requester.PostOperation<Language>($"http://localhost:8000/api/languages/create-language", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
    }

    public void ShowSelectedLanguages()
    {
        StartCoroutine(GetSelectedLanguages());
    }

    public IEnumerator GetSelectedLanguages()
    {

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<Language>> operation = Requester.GetOperation<List<Language>>($"http://localhost:8000/api/my-languages/", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            foreach (Language language in operation.Data)
            {
                LanguageLevel newLanguagePrefab = Instantiate(languageLevelPrefab, languageLevelContent);
                if (language.name == "english")
                {
                    newLanguagePrefab.languageFlag.sprite = englishButton.transform.GetChild(1).GetComponent<Image>().sprite;
                }
                else if (language.name == "french")
                {
                    newLanguagePrefab.languageFlag.sprite = frenchButton.transform.GetChild(1).GetComponent<Image>().sprite;
                }
                else if (language.name == "german")
                {
                    newLanguagePrefab.languageFlag.sprite = germanButton.transform.GetChild(1).GetComponent<Image>().sprite;
                }
                else if (language.name == "japanese")
                {
                    newLanguagePrefab.languageFlag.sprite = japaneseButton.transform.GetChild(1).GetComponent<Image>().sprite;
                }
                else if (language.name == "spanish")
                {
                    newLanguagePrefab.languageFlag.sprite = spanishButton.transform.GetChild(1).GetComponent<Image>().sprite;
                }
                newLanguagePrefab.currentLanguageId = language.id;
                newLanguagePrefab.currentLanguageName = language.name;
            }
        }
    }

    public void SaveLanguagesLevels()
    {
        for (var i = languageLevelContent.transform.childCount - 1; i >= 0; i--)
        {
            Dictionary<string, string> newLanguage = new Dictionary<string, string>();
            newLanguage.Add("level", languageLevelContent.transform.GetChild(i).GetComponent<LanguageLevel>().GetCurrentLanguageLevel());
            newLanguage.Add("name", languageLevelContent.transform.GetChild(i).GetComponent<LanguageLevel>().currentLanguageName);
            int languageId = languageLevelContent.transform.GetChild(i).GetComponent<LanguageLevel>().currentLanguageId;

            StartCoroutine(PutLanguageLevel(newLanguage, languageId));
        }
    }

    public IEnumerator PutLanguageLevel(Dictionary<string, string> body, int id)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<Language> operation = Requester.PostOperation<Language>($"http://127.0.0.1:8000/api/languages/language/{id}/update", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
    }


    public void CleanLanguageLevelObjects()
    {
        for (var i = languageLevelContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(languageLevelContent.transform.GetChild(i).gameObject);
        }
    }

    public void ShowUserPaymentMethods()
    {
        for (var i = cardInfoContent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardInfoContent.GetChild(i).gameObject);
        }
        StartCoroutine(GetUserPaymentMethods());
    }

    public IEnumerator GetUserPaymentMethods()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<PaymentMethod>> operation = Requester.GetOperation<List<PaymentMethod>>($"http://127.0.0.1:8000/api/create-payment", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
            foreach (PaymentMethod payment in operation.Data)
            {
                GameObject newJob = Instantiate(cardInfoPrefab, cardInfoContent);
                newJob.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "XXXXXXXXXXXX" + payment.number.ToString().Substring(payment.number.ToString().Length - 4);
                newJob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = payment.expire_date;
                newJob.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => deleteCardInfo(payment));
            }
        }
    }

    public void deleteCardInfo(PaymentMethod payment)
    {
        StartCoroutine(DeleteSpecificCard(payment));
    }

    public IEnumerator DeleteSpecificCard(PaymentMethod payment)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<Message> operation = Requester.PostOperation<Message>($"http://localhost:8000/api/payment/{payment.id}/delete", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información eliminada", false);
            ShowUserPaymentMethods();
        }
    }

    public void SaveCardInfo()
    {
        string cardNumber = this.CardNumber.text;
        string expirationDate = this.ExpirationDate.text;
        string cvv = this.CVV.text;

        Dictionary<string, string> body = new Dictionary<string, string>();
        body.Add("number", cardNumber);
        body.Add("expire_date", expirationDate + "-01");
        body.Add("ccv", cvv);

        if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(expirationDate) || string.IsNullOrEmpty(cvv))
        {
            popUp.SetPopUpMessage("Por favor llene todos los campos", true);
        }
        else if(!IsCreditCardInfoValid(cardNumber, expirationDate, cvv))
        {
            popUp.SetPopUpMessage("La información de la tarjeta no es válida", true);
        }
        else
        {
            StartCoroutine(PostCardInfo(body));
        }
    }

    public IEnumerator PostCardInfo(Dictionary<string, string> body)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<PaymentMethod> operation = Requester.PostOperation<PaymentMethod>($"http://127.0.0.1:8000/api/create-payment", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
        else
        {
            popUp.SetPopUpMessage(operation.ErrorMessage, true);
        }
    }

    public static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
    {
        var cardCheck = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$");
        var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
        var yearCheck = new Regex(@"^20[0-9]{2}$");
        var cvvCheck = new Regex(@"^\d{3}$");

        if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
            return false;
        if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
            return false;

        var dateParts = expiryDate.Split('-'); //expiry date in from MM-yyyy            
        if (!monthCheck.IsMatch(dateParts[1]) || !yearCheck.IsMatch(dateParts[0])) // <3 - 6>
            return false; // ^ check date format is valid as "MM/yyyy"

        var year = int.Parse(dateParts[0]);
        var month = int.Parse(dateParts[1]);
        var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
        var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

        //check expiry greater than today & within next 6 years <7, 8>>
        return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
    }
}
