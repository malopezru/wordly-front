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

    void Start()
    {
        //get function
        List<TutorInfo> tutorsList = new List<TutorInfo>();

        TutorInfo tutor1 = new TutorInfo();
        TutorInfo tutor2 = new TutorInfo();

        tutor1.name = "Juan";
        tutor1.last_name = "Ortega";
        tutor1.days = new List<string>();
        tutor1.days.Add("Lunes");
        tutor1.days.Add("Viernes");
        tutor1.cost = "10000";
        tutor1.rating = 5;

        tutor2.name = "Mateo";
        tutor2.last_name = "López";
        tutor2.days = new List<string>();
        tutor2.days.Add("Lunes");
        tutor2.days.Add("Jueves");
        tutor2.days.Add("Sábado");
        tutor2.cost = "15000";
        tutor2.rating = 4;

        tutorsList.Add(tutor1);
        tutorsList.Add(tutor2);

        CreateTutors(tutorsList);
    }

    public void CreateTutors(List<TutorInfo> tutorsList)
    {
        foreach (TutorInfo tutor in tutorsList)
        {
            TutorPrefab newTutor = Instantiate(tutorPrefab, tutorTransform);
            newTutor.tutorName.text = $"{tutor.name} {tutor.last_name}";
            newTutor.cost.text = tutor.cost;
            //newTutor.rating.text = tutor.rating.ToString();
            SetTutorWorkingDays(newTutor, tutor.days);
            newTutor.GetComponent<Button>().onClick.AddListener(() => Debug.Log("clicked"));
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
