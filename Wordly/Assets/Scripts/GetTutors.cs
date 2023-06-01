using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    private void OnEnable()
    {
        for (var i = tutorTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(tutorTransform.GetChild(i).gameObject);
        }
        StartCoroutine(GetAllTutors());
    }

    void Start()
    {
        Requester = GameObject.Find("App").GetComponent<Requester>();
    }

    public IEnumerator GetAllTutors()
    {
        if (!Requester)
        {
            Requester = GameObject.Find("App").GetComponent<Requester>();
        }

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));
        body.Add("name", PlayerPrefs.GetString("CurrentLanguage"));

        OperationResult<List<TutorInfo>> operation = Requester.PostOperation<List<TutorInfo>>($"http://127.0.0.1:8000/api/search/tutors/language", body, header);

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
            newTutor.cost.text = tutor.cost;
            newTutor.cost.text = "$10000";
            string[] daysTutor = {"Lunes", "Martes", "Miércoles"};
            //newTutor.rating.text = tutor.rating.ToString();
            if (tutor.days != null)
            {
                SetTutorWorkingDays(newTutor, daysTutor);
            }
            newTutor.GetComponent<Button>().onClick.AddListener(() => SearchTutorView.SetActive(false));
            newTutor.GetComponent<Button>().onClick.AddListener(() => TutorInfoView.SetActive(true));
            newTutor.GetComponent<Button>().onClick.AddListener(() => tutorInfoController.SetTutorInfo(tutor));
        }
    }

    public void SetTutorWorkingDays(TutorPrefab tutor, string[] days)
    {
        if (days.Contains("Lunes"))
        {
            tutor.monday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Martes"))
        {
            tutor.tuesday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Miercoles"))
        {
            tutor.wednesday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Jueves"))
        {
            tutor.thursday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Viernes"))
        {
            tutor.friday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Sábado"))
        {
            tutor.saturday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
        if (days.Contains("Domingo"))
        {
            tutor.sunday.GetComponent<TextMeshProUGUI>().color = activeDayColor;
        }
    }
}
