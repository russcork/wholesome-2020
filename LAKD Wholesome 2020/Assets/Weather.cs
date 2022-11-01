using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public float _timeOfDay = 0;
    public float _dayDuration = 0;
    public static Weather _instance;

    private float _minutesPerSecond;

    public int _daysElapsed = 0;
    
    public enum WeatherType
    {
        Clear = 0,
        Rain = 1
    }

    public enum TimeOfDayDescriptive
    {
        Dawn = 0,
        Day = 1,
        Dusk = 2,
        Night = 3
    }

    public TimeOfDayDescriptive _timeOfDayDescriptive;

    public WeatherType _weatherType;

    void Awake()
    {
        _instance = this;
        _minutesPerSecond = (24 / _dayDuration) * 60f;
    }

    public string GetFriendlyTime()
    {
        var time = TimeSpan.FromMinutes(_minutesPerSecond * _timeOfDay) + TimeSpan.FromHours(9); // start at 9am
        if (time.Days >= 1) time -= TimeSpan.FromDays(1);
        
        var totalMinutes = (int)(time + new TimeSpan(0, 15/2, 0)).TotalMinutes;

        // thanks stackoverflow
        time = new TimeSpan(0, totalMinutes - totalMinutes % 15, 0);
        
        return string.Format($"{time.Hours:00}:{time.Minutes:00}");
    }
    
    void Update()
    {
        _timeOfDay += Time.deltaTime;
        
        if (_timeOfDay < 20)
        {   
            _timeOfDayDescriptive = TimeOfDayDescriptive.Day;
        }
        else if (_timeOfDay >= 20 && _timeOfDay < 40)
        {
            _timeOfDayDescriptive = TimeOfDayDescriptive.Dusk;
        }
        else if (_timeOfDay >= 40 && _timeOfDay < 60)
        {
            _timeOfDayDescriptive = TimeOfDayDescriptive.Night;
        }
        else
        {
            _timeOfDayDescriptive = TimeOfDayDescriptive.Dawn;
        }

        if (_timeOfDay > _dayDuration)
        {
            _timeOfDay = 0;
            _daysElapsed ++;
        }
        
        ChangeWeatherType();
    }

    void ChangeWeatherType()
    {

    }

    float GetTimeOfDay()
    {
        return _timeOfDay;
    }

    WeatherType GetWeatherType()
    {
        return _weatherType;
    }
}
