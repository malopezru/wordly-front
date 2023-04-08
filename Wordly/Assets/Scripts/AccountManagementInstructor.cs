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

    [Header("Job Experience")]
    [SerializeField] TMP_InputField EmpresaInput;
    [SerializeField] TMP_InputField PosicionInput;
    [SerializeField] TMP_InputField DuracionInput;

    public void SaveDecriptionInfo()
    {
        string descript = this.AboutMeInput.text;

        Debug.Log(descript);
    }

    public void SaveExpInfo()
    {

        string company = this.EmpresaInput.text;
        string position = this.PosicionInput.text;
        string duration = this.DuracionInput.text;

        if (string.IsNullOrEmpty(company) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(duration))
        {
            popUp.SetPopUpMessage("Por favor llene todos los campos", true);
        }
        else
        {
            Debug.Log(company + " : " + position + " : " + duration);
        }
    }
}
