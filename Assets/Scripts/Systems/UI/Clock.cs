using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI Time;
    public TextMeshProUGUI Days;
    public TextMeshProUGUI Season;

    public void UpdateTime(int hours, int minutes)
    {
        Time.text = $"{hours:0#}:{minutes}0";
    }

    public void UpdateDay(int day)
    {
        Days.text = day.ToString();
    }

    public void UpdateSeason(int season)
    {
        Season.text = season.ToString();
    }
}
