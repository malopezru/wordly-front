using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class BookClassController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalCost;
    [SerializeField] TMP_InputField classDay;
    [SerializeField] TMP_InputField beginHour;
    [SerializeField] TMP_InputField endHour;
    [SerializeField] GameObject successfulPopUp;

    int begin = -1;
    int end = -1;
    public int tutorCost;

    public TMP_InputField ClassDay { get => classDay; set => classDay = value; }
    public TMP_InputField BeginHour { get => beginHour; set => beginHour = value; }
    public TMP_InputField EndHour { get => endHour; set => endHour = value; }

    public void SetTutorCost(string cost)
    {
        tutorCost = Int32.Parse(cost);
    }

    private void Update()
    {
        Debug.Log(this.BeginHour.text);
        if (!string.IsNullOrEmpty(this.BeginHour.text))
        {
            begin = Int32.Parse(this.BeginHour.text);
        }
        else
        {
            begin = -1;
        }

        if (!string.IsNullOrEmpty(this.EndHour.text))
        {
            end = Int32.Parse(this.EndHour.text);
        }
        else
        {
            end = -1;
        }

        if (end > begin && end > 0 && end <= 24 && begin >= 0 && begin < 24)
        {
            int cost = (end - begin) * tutorCost;
            totalCost.text = "$" + cost.ToString();
        }
        else
        {
            totalCost.text = "$";
        }


    }

    public void SendClassSessionRequest()
    {
        string sessionDay = this.ClassDay.text;
        if (end > begin && end > 0 && end <= 24 && begin >= 0 && begin < 24 && !string.IsNullOrEmpty(sessionDay))
        {
            successfulPopUp.SetActive(true);
        }
        //send request to server
    }
}
