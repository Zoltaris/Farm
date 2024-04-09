using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour, ISingleton
{
    public static DayManager Instance;
    public UnityEvent NewDay;
    private short hours;
    private short minutes;
    public Clock clock;
    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    public Day day;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public Season season;
    public short year;
    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        year = 0;
        season = Season.Winter;
        day = Day.Sunday;
        BeginNewDay();
    }

    // Update is called once per frame
    void Update()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        if (_time >= 15)
        {
            minutes += 1;
            _time = 0;
        }
        else
        {
            _time += Time.deltaTime;
        }
        if (minutes >= 6)
        {
            hours += 1;
            minutes = 0;
        }
        if (hours >= 24)
        {
            hours = 0;
        }
        if (hours is >= 2 and < 3)
        {
            ForceNewDay();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ForceNewDay();
        }
        clock.UpdateTime(hours, minutes);
            
    }

    private void BeginNewDay()
    {
        hours = 6;
        minutes = 0;
        _time = 0;
        int dayNum = (int)day;
        dayNum++;
        if (dayNum > 6)
        {
            day = Day.Monday;
            UpdateSeason();
        }
        else
        {
            day = (Day)dayNum;
        }
        clock.UpdateDay((int)day+1);
        NewDay.Invoke();   
    }

    private void UpdateSeason()
    {
        int seasonNum = (int)season;
        seasonNum++;
        if (seasonNum > 3)
        {
            BeginNewYear();
            season = Season.Spring;
        }
        else
        {
            season = (Season)seasonNum;
        }
        clock.UpdateSeason((int)season+1);
    }

    private void BeginNewYear()
    {
        year++;
    }

    private void ForceNewDay()
    {
        Debug.Log("Uhoh stinky man didn't snoozle");
        BeginNewDay();
    }

    public void Sleep()
    {
        BeginNewDay();
    }

}
