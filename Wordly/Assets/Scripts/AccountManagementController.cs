using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
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

    [Header("Payment Method")]
    [SerializeField] TMP_InputField cardNumberInput;
    [SerializeField] TMP_InputField expirationDateInput;
    [SerializeField] TMP_InputField cvvInput;

    [Header("Management Selection")]
    [SerializeField] Button personalInfoButton;
    [SerializeField] Button languagesInfoButton;
    [SerializeField] Button paymentMethodsButton;
    [SerializeField] GameObject SelectedButtonBackground;

    [Header("Other")]
    [SerializeField] public MessagePopUp popUp;

    private string sex = "Masculino";
    public List<string> selectedLanguages = null;

    public TMP_InputField Name { get => nameInput; set => nameInput = value; }
    public TMP_InputField LastName { get => lastNameInput; set => lastNameInput = value; }
    public TMP_InputField Birthday { get => birthdayInput; set => birthdayInput = value; }
    public TMP_InputField CardNumber { get => cardNumberInput; set => cardNumberInput = value; }
    public TMP_InputField ExpirationDate { get => expirationDateInput; set => expirationDateInput = value; }
    public TMP_InputField CVV { get => cvvInput; set => cvvInput = value; }

    private void Awake()
    {
        frenchButton.onClick.AddListener(() => SetPressedButton(frenchButton, StringInArray(selectedLanguages, "French"), "French"));
        englishButton.onClick.AddListener(() => SetPressedButton(englishButton, StringInArray(selectedLanguages, "English"), "English"));
        spanishButton.onClick.AddListener(() => SetPressedButton(spanishButton, StringInArray(selectedLanguages, "Spanish"), "Spanish"));
        japaneseButton.onClick.AddListener(() => SetPressedButton(japaneseButton, StringInArray(selectedLanguages, "Japanese"), "Japanese"));
        germanButton.onClick.AddListener(() => SetPressedButton(germanButton, StringInArray(selectedLanguages, "German"), "German"));
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
        foreach (var x in selectedLanguages)
        {
            Debug.Log(x.ToString());
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
            sex = "Masculino";
        }
        else if (value == 1)
        {
            sex = "Femenino";
        }
        else if (value == 2) 
        {
            sex = "Otro";
        }
    }

    public bool IsDateTime(string value)
    {
        DateTime dateTime;
        bool isDateTime = false;

        isDateTime = DateTime.TryParse(value, out dateTime);

        return isDateTime;
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
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
            Debug.Log(name + " : " + lastName + " : " + birthday + " : " + sex);
        }
    }

    public void SaveCardInfo()
    {

        string cardNumber = this.CardNumber.text;
        string expirationDate = this.ExpirationDate.text;
        string cvv = this.CVV.text;

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
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
            Debug.Log(cardNumber + " : " + expirationDate + " : " + cvv);
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

        var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
        if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
            return false; // ^ check date format is valid as "MM/yyyy"

        var year = int.Parse(dateParts[1]);
        var month = int.Parse(dateParts[0]);
        var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
        var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

        //check expiry greater than today & within next 6 years <7, 8>>
        return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
    }
}
