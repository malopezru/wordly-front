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
    private Requester requester;

    int begin = -1;
    int end = -1;
    public int tutorCost;
    private int currentTutorId;

    public TMP_InputField ClassDay { get => classDay; set => classDay = value; }
    public TMP_InputField BeginHour { get => beginHour; set => beginHour = value; }
    public TMP_InputField EndHour { get => endHour; set => endHour = value; }
    public Requester Requester { get => requester; set => requester = value; }

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

    public void SetTutorId(int id)
    {
        currentTutorId = id;
    }

    public void SendClassSessionRequest()
    {
        StartCoroutine(PostClassSession());
    }

    public IEnumerator PostClassSession()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        string sessionDay = this.ClassDay.text;
        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("date", sessionDay);
        body.Add("start_time", begin.ToString());
        body.Add("end_time", end.ToString());
        body.Add("description", "Sesión de clase");
        body.Add("tutor", currentTutorId.ToString());

        OperationResult<ClassSession> operation = Requester.PostOperation<ClassSession>($"http://127.0.0.1:8000/api/sessions/create-session", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            if (end > begin && end > 0 && end <= 24 && begin >= 0 && begin < 24 && !string.IsNullOrEmpty(sessionDay))
            {
                successfulPopUp.SetActive(true);
            }
        }
        else
        {
            Debug.Log(operation.ErrorMessage);
        }
    }
}
