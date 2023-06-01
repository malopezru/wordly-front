using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetScheduleStudent : MonoBehaviour
{
    [SerializeField] GameObject schedulePrefab;
    [SerializeField] Transform sessionsContent;
    [SerializeField] Color approvedColor;
    [SerializeField] Color rejectedColor;
    private Requester requester;

    public Requester Requester { get => requester; set => requester = value; }

    private void OnEnable()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        for (var i = sessionsContent.childCount - 1; i >= 0; i--)
        {
            Destroy(sessionsContent.GetChild(i).gameObject);
        }

        StartCoroutine(GetStudentSessions());
    }
    
    public IEnumerator GetStudentSessions()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<ClassSession>> operation = Requester.GetOperation<List<ClassSession>>($"http://127.0.0.1:8000/api/my-sessions/students", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            foreach (ClassSession session in operation.Data)
            {
                GameObject newSession = Instantiate(schedulePrefab, sessionsContent);
                newSession.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = session.date.Substring(0, 10);
                newSession.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = session.start_time.ToString();
                newSession.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = session.end_time.ToString();
                if (session.status == 0)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Pendiente";
                }
                else if (session.status == 1)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Aprobado";
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = approvedColor;
                }
                else if (session.status == 2)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Rechazado";
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = rejectedColor;
                }
            }
        }
    }
}
