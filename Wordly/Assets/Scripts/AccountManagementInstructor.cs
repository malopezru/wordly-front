using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] TMP_InputField CostInput;

    [Header("Job Experience")]
    [SerializeField] TMP_InputField EmpresaInput;
    [SerializeField] TMP_InputField PosicionInput;
    [SerializeField] TMP_InputField DuracionInput;
    [SerializeField] GameObject jobExperiencePrefab;
    [SerializeField] Transform jobExperienceContent;

    [Header("Availability")]
    [SerializeField] DayHoursPrefab hourButtonsPrefab;
    [SerializeField] Transform dayAvailabilityContent;

    public void SaveDecriptionInfo()
    {
        string description = this.AboutMeInput.text;
        string cost = this.CostInput.text;
        requestBody["description"] = description;
        requestBody["cost"] = cost;
        StartCoroutine(PostTutorDescription(requestBody));
    }

    public void GenerateAvailabilityView()
    {
        for (var i = dayAvailabilityContent.childCount - 1; i >= 1; i--)
        {
            Destroy(dayAvailabilityContent.GetChild(i).gameObject);
        }

        for (var i = 0; i < 7;  i++)
        {
            DayHoursPrefab dayButtons = Instantiate(hourButtonsPrefab, dayAvailabilityContent);
            dayButtons.day = (i + 1).ToString();
        }
    }

    public void SendSelectedAvailability()
    {
        List<Dictionary<string, string>> availabilityBody = new List<Dictionary<string, string>>();
        for (var i = 1; i < dayAvailabilityContent.childCount; i++)
        {
            Debug.Log("DAY: " + dayAvailabilityContent.GetChild(i).GetComponent<DayHoursPrefab>().day);
            for (var j = 1; j < dayAvailabilityContent.GetChild(i).childCount; j++)
            {
                if (dayAvailabilityContent.GetChild(i).transform.GetChild(j).GetComponent<DayHoursButtonPrefab>().isSelected)
                {
                    Dictionary<string, string> currentSchedule = new Dictionary<string, string>();
                    currentSchedule.Add("day_of_week", i.ToString());
                    int start = Int32.Parse(dayAvailabilityContent.GetChild(i).transform.GetChild(j).name);

                    if (start < 10)
                    {
                        currentSchedule.Add("start_time", "0" + dayAvailabilityContent.GetChild(i).transform.GetChild(j).name + ":00");
                    }
                    else
                    {
                        currentSchedule.Add("start_time", dayAvailabilityContent.GetChild(i).transform.GetChild(j).name + ":00");
                    }

                    int end = start + 1;
                    if (end < 10)
                    {
                        currentSchedule.Add("end_time", "0" + end.ToString() + ":00");
                    }
                    else
                    {
                        currentSchedule.Add("end_time", end.ToString() + ":00");
                    }
                    availabilityBody.Add(currentSchedule);
                }
            }
        }
        StartCoroutine(PostTutorSchedule(availabilityBody));
    }

    public IEnumerator PostTutorSchedule(List<Dictionary<string, string>> body)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        foreach(Dictionary<string, string> schedule in body)
        {
            OperationResult<AvailabilityModel> operation = Requester.PostOperation<AvailabilityModel> ($"http://localhost:8000/api/time-av/create", schedule, header);

            while (!operation.IsReady)
            {
                yield return null;
            }

            if (!operation.HasError)
            {
                popUp.SetPopUpMessage("Información Guardada Correctamente", false);
            }
        }
    }

    public void ShowExperience()
    {
        for (var i = jobExperienceContent.childCount - 1; i >= 0; i--)
        {
            Destroy(jobExperienceContent.GetChild(i).gameObject);
        }
        StartCoroutine(GetExperience());
    }

    public IEnumerator GetExperience()
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<List<JobExperience>> operation = Requester.GetOperation<List<JobExperience>>($"http://localhost:8000/api/my-workexp", header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            foreach (JobExperience job in operation.Data)
            {
                GameObject newJob = Instantiate(jobExperiencePrefab, jobExperienceContent);
                newJob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = job.company;
                newJob.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = job.position;
                newJob.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = job.lenght + " años";
                newJob.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => deleteJobExperience(job));
            }
        }
    }

    public void deleteJobExperience(JobExperience job)
    {
        StartCoroutine(deleteSpecificExperience(job));
    }

    public IEnumerator deleteSpecificExperience(JobExperience job)
    {

        Dictionary<string, string> header = new Dictionary<string, string>();
        Dictionary<string, string> body = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<Message> operation = Requester.PostOperation<Message>($"http://localhost:8000/api/workexp/work_exp/{job.id}/delete", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información eliminada", false);
            ShowExperience();
        }
    }

    public void SaveExpInfo()
    {

        string company = this.EmpresaInput.text;
        string position = this.PosicionInput.text;
        string duration = this.DuracionInput.text;

        Dictionary<string, string> body = new Dictionary<string, string>();
        body.Add("company", company);
        body.Add("position", position);
        body.Add("lenght", duration);

        if (string.IsNullOrEmpty(company) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(duration))
        {
            popUp.SetPopUpMessage("Por favor llene todos los campos", true);
        }
        else
        {
            StartCoroutine(PostExpInfo(body));
        }
    }

    public IEnumerator PostExpInfo(Dictionary<string, string> body)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<JobExperience> operation = Requester.PostOperation<JobExperience>($"http://localhost:8000/api/workexp/create-workexp", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
            ShowExperience();
        }
        else
        {
            popUp.SetPopUpMessage(operation.ErrorMessage, true);
        }
    }

    IEnumerator PostTutorDescription(Dictionary<string, string> body)
    {
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Authorization", PlayerPrefs.GetString("Authorization"));

        OperationResult<User> operation = Requester.PostOperation<User>($"http://localhost:8000/api/profile/update_data_tutor", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
        else
        {
            popUp.SetPopUpMessage(operation.ErrorMessage, true);
        }
    }
}
