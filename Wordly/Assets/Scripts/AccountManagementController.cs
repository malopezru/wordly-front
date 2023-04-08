using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountManagementController : MonoBehaviour
{
    [Header("Personal Info")]
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField lastNameInput;
    [SerializeField] TMP_InputField birthdayInput;
    [SerializeField] TextMeshProUGUI noSelectedOptionText;
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

    private string sex = "Masculino";
    private List<string> selectedLanguages = null;

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
        noSelectedOptionText.gameObject.SetActive(false);
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
            Debug.Log("Please Fill all fields");
        }
        else if (!IsDateTime(birthday))
        {
            Debug.Log("Error in birthday format : " + birthday);
        }
        else
        {
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
            Debug.Log("Please Fill all fields");
        }
        else
        {
            Debug.Log(cardNumber + " : " + expirationDate + " : " + cvv);
        }
    }
}
