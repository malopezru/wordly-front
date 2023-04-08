using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountManagementInstructor: AccountManagementController
{
    [SerializeField] Button AboutMeButton;
    [SerializeField] Button JobExperienceButton;
    [SerializeField] Button ScheduleButton;

    [Header ("About Me")] 
    [SerializeField] Button ContinueButton;
    [SerializeField] TMP_InputField AboutMeInput;

    [Header("Experiencia laboral")]
    [SerializeField] TMP_InputField EmpresaInput;
    [SerializeField] TMP_InputField PosicionInput;
    [SerializeField] TMP_InputField DuracionInput;
    


    void Start()
    {

        //ContinueButton.onClick.AddListener(() => SetPressedButton(ContinueButton, StringInArray(selectedLanguages, "French"), "French"));
        
    }

    public void SaveDecriptionInfo()
    {
        string descript = this.AboutMeInput.text;

        Debug.Log(descript);
    }

    public void SaveExpInfo()
    {

        string empresa = this.EmpresaInput.text;
        string posicion = this.PosicionInput.text;
        string duracion = this.DuracionInput.text;

        if (string.IsNullOrEmpty(empresa) || string.IsNullOrEmpty(posicion) || string.IsNullOrEmpty(duracion))
        {
            Debug.Log("Please Fill all fields");
        }
        else
        {
            Debug.Log(empresa + " : " + posicion + " : " + duracion);
        }
    }
}
