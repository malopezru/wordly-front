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

    [Header("Job Experience")]
    [SerializeField] TMP_InputField EmpresaInput;
    [SerializeField] TMP_InputField PosicionInput;
    [SerializeField] TMP_InputField DuracionInput;
    [SerializeField] GameObject jobExperiencePrefab;
    [SerializeField] Transform jobExperienceContent;

    public void SaveDecriptionInfo()
    {
        string description = this.AboutMeInput.text;
        requestBody["description"] = description;
        StartCoroutine(PostTutorDescription(requestBody));
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

        OperationResult<Language> operation = Requester.PostOperation<Language>($"http://localhost:8000/api/profile/update_data_tutor", body, header);

        while (!operation.IsReady)
        {
            yield return null;
        }

        if (!operation.HasError)
        {
            popUp.SetPopUpMessage("Información Guardada Exitosamente", false);
        }
    }
}
