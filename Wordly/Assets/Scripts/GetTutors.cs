using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTutors : MonoBehaviour
{
    public TutorPrefab tutorPrefab;
    public Transform tutorTransform;
    public Color activeDayColor;
    public GameObject SearchTutorView;
    public GameObject TutorInfoView;
    public TutorInfoController tutorInfoController;
    List<TutorInfo> tutorsList = new List<TutorInfo>();
    Requester requester;

    public Requester Requester { get => requester; set => requester = value; }

    void Start()
    {
        Requester = GameObject.Find("App").GetComponent<Requester>();
    }

    public void ShowTutors()
    {
        StartCoroutine(GetAllTutors());
    }

    public IEnumerator GetAllTutors()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<TutorInfo>> operation = Requester.GetOperation<List<TutorInfo>>($"http://127.0.0.1:8000/api/search/tutors/all", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            tutorsList = operation.Data;
            CreateTutors(tutorsList);
        }
    }

    public void CreateTutors(List<TutorInfo> tutorsList)
    {
        foreach (TutorInfo tutor in tutorsList)
        {
            TutorPrefab newTutor = Instantiate(tutorPrefab, tutorTransform);
            newTutor.tutorName.text = $"{tutor.name} {tutor.last_name}";
            //newTutor.cost.text = tutor.cost;
            newTutor.cost.text = "$10000";
            //newTutor.rating.text = tutor.rating.ToString();
            SetTutorWorkingDays(newTutor, tutor.days);
            newTutor.GetComponent<Button>().onClick.AddListener(() => SearchTutorView.SetActive(false));
            newTutor.GetComponent<Button>().onClick.AddListener(() => TutorInfoView.SetActive(true));
            newTutor.GetComponent<Button>().onClick.AddListener(() => tutorInfoController.SetTutorInfo(tutor));
        }
    }

    public void SetTutorWorkingDays(TutorPrefab tutor, List<string> days)
    {
        if (days.Contains("Lunes"))
        {
            tutor.monday.color = activeDayColor;
        }
        if (days.Contains("Martes"))
        {
            tutor.tuesday.color = activeDayColor;
        }
        if (days.Contains("Miercoles"))
        {
            tutor.wednesday.color = activeDayColor;
        }
        if (days.Contains("Jueves"))
        {
            tutor.thursday.color = activeDayColor;
        }
        if (days.Contains("Viernes"))
        {
            tutor.friday.color = activeDayColor;
        }
        if (days.Contains("Sábado"))
        {
            tutor.saturday.color = activeDayColor;
        }
        if (days.Contains("Domingo"))
        {
            tutor.sunday.color = activeDayColor;
        }
    }
}
