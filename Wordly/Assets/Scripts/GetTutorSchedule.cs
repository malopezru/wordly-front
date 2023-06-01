using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetTutorSchedule : MonoBehaviour
{
    [SerializeField] GameObject tutorSchedulePrefab;
    [SerializeField] Transform sessionsContent;
    [SerializeField] MessagePopUp messagePopUp;
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

        StartCoroutine(GetTutorSessions());
    }

    public IEnumerator GetTutorSessions()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<ClassSession>> operation = Requester.GetOperation<List<ClassSession>>($"http://127.0.0.1:8000/api/my-sessions/tutors", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            foreach(ClassSession session in operation.Data)
            {
                GameObject newSession = Instantiate(tutorSchedulePrefab, sessionsContent);
                newSession.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = session.date.Substring(0, 10);
                newSession.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = session.start_time.ToString();
                newSession.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = session.end_time.ToString();
                if (session.status == 0)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Pendiente";
                    newSession.transform.GetChild(5).GetComponent<Button>().interactable = true;
                    newSession.transform.GetChild(6).GetComponent<Button>().interactable = true;
                }
                else if (session.status == 1)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Aprobado";
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = approvedColor;
                    newSession.transform.GetChild(5).GetComponent<Button>().interactable = false;
                    newSession.transform.GetChild(6).GetComponent<Button>().interactable = false;
                }
                else if (session.status == 2)
                {
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Rechazado";
                    newSession.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = rejectedColor;
                    newSession.transform.GetChild(5).GetComponent<Button>().interactable = false;
                    newSession.transform.GetChild(6).GetComponent<Button>().interactable = false;
                }
                newSession.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => ApproveSession(session));
                newSession.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => RejectSession(session));
            }
        }
    }

    public void ApproveSession(ClassSession session)
    {
        StartCoroutine(ApproveSessionRequest(session));
    }

    public IEnumerator ApproveSessionRequest(ClassSession session)
    {

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("date", session.date);
        body.Add("start_time", session.start_time);
        body.Add("end_time", session.end_time);
        body.Add("description", session.description);
        body.Add("tutor", session.tutor.ToString());
        body.Add("student", session.student.ToString());
        body.Add("status", "1");

        OperationResult<ClassSession> operation = Requester.PostOperation<ClassSession>($"http://127.0.0.1:8000/api/sessions/session/{session.id}/update", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            messagePopUp.SetPopUpMessage("Sesión aprobada", false);
            StartCoroutine(GetTutorSessions());
        }
        else
        {
            messagePopUp.SetPopUpMessage(operation.ErrorMessage, true);
        }
    }

    public void RejectSession(ClassSession session)
    {
        StartCoroutine(RejectSessionRequest(session));
    }

    public IEnumerator RejectSessionRequest(ClassSession session)
    {

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("date", session.date);
        body.Add("start_time", session.start_time);
        body.Add("end_time", session.end_time);
        body.Add("description", session.description);
        body.Add("tutor", session.tutor.ToString());
        body.Add("student", session.student.ToString());
        body.Add("status", "2");

        OperationResult<List<ClassSession>> operation = Requester.PostOperation<List<ClassSession>>($"http://127.0.0.1:8000/api/sessions/session/{session.id}/update", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            messagePopUp.SetPopUpMessage("Sesión rechazada", false);
            StartCoroutine(GetTutorSessions());
        }
        else
        {
            messagePopUp.SetPopUpMessage(operation.ErrorMessage, true);
        }
    }
}
