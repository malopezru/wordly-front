using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayHoursPrefab : MonoBehaviour
{
    [SerializeField] DayHoursButtonPrefab dayHoursButtonPrefab;
    private List<DayHoursButtonPrefab> dayHoursButtons;
    public string day;

    private void Start()
    {
        for (var i = 1; i <= 24; i++)
        {
            DayHoursButtonPrefab button = Instantiate(dayHoursButtonPrefab, this.transform);
            button.name = (i - 1).ToString();
        }
    }

    public List<DayHoursButtonPrefab> GetDayButtons()
    {
        return dayHoursButtons;
    }
}
