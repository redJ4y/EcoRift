using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
* Emblem return:
*    0      1       2      3
*   sun    snow   storm   rain
*  [false, false, false, false]
*
* Weather type to emblem type:
* "Clear" = sun
* "Snow" = snow
* "Clouds" = storm
* "Rain" = rain
*/
public class DataManager : MonoBehaviour
{
    [SerializeReference] private GameObject textObj; // For testing purposes
    [SerializeReference] private GetWeather weatherState;
    private string[] weatherNames;

    public void SetWeatherNames(string[] weatherNames) // Must be called before using GetUnlockedTiers
    {
        this.weatherNames = weatherNames;
    }

    public Dictionary<string, Dictionary<int, bool>> GetUnlockedTiers()
    {
        Dictionary<string, Dictionary<int, bool>> unlockedTiers = new();
        foreach (string weatherName in weatherNames)
        {
            Dictionary<int, bool> newDict = new();
            for (int i = 1; i <= 3; i++)
            {
                newDict.Add(i, false);
            }
            for (int i = 0; i <= GetProg(weatherName); i++)
            {
                newDict.Remove(i + 1);
                newDict.Add(i + 1, true);
            }
            unlockedTiers.Add(weatherName, newDict);
        }
        return unlockedTiers;
    }

    private int GetProg(string weatherName)
    {
        switch (weatherName)
        {
            case "Water":
                return GetRainProg();
            case "Snow":
                return GetSnowProg();
            case "Lightning":
                return GetStormProg();
            case "Sun":
                return GetSunProg();
        }
        Debug.Log("WEATHER NAMES DO NOT MATCH (DataManager.cs/49)");
        return 0;
    }

    private bool IncreaseSunProg()
    {
        int current = PlayerPrefs.GetInt("sun", 0);

        if (current >= 2)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("sun", current + 1);
            return true;
        }
    }

    private bool IncreaseSnowProg()
    {
        int current = PlayerPrefs.GetInt("snow", 0);

        if (current >= 2)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("snow", current + 1);
            return true;
        }

    }
    private bool IncreaseStormProg()
    {
        int current = PlayerPrefs.GetInt("storm", 0);

        if (current >= 2)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("storm", current + 1);
            Debug.Log(PlayerPrefs.GetInt("storm"));
            return true;
        }
    }
    private bool IncreaseRainProg()
    {
        int current = PlayerPrefs.GetInt("rain", 0);

        if (current >= 2)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("rain", current + 1);
            return true;
        }

    }

    public bool[] GetRainLevelEmblems()
    {
        bool[] emblems = new bool[4];
        emblems[0] = PlayerPrefs.GetInt("rainClear", 0) == 1;
        emblems[1] = PlayerPrefs.GetInt("rainSnow", 0) == 1;
        emblems[2] = PlayerPrefs.GetInt("rainClouds", 0) == 1;
        emblems[3] = PlayerPrefs.GetInt("rainRain", 0) == 1;

        return emblems;
    }

    public bool[] GetStormLevelEmblems()
    {
        bool[] emblems = new bool[4];
        emblems[0] = PlayerPrefs.GetInt("stormClear", 0) == 1;
        emblems[1] = PlayerPrefs.GetInt("stormSnow", 0) == 1;
        emblems[2] = PlayerPrefs.GetInt("stormClouds", 0) == 1;
        emblems[3] = PlayerPrefs.GetInt("stormRain", 0) == 1;

        return emblems;
    }

    public bool[] GetSnowLevelEmblems()
    {
        bool[] emblems = new bool[4];
        emblems[0] = PlayerPrefs.GetInt("snowClear", 0) == 1;
        emblems[1] = PlayerPrefs.GetInt("snowSnow", 0) == 1;
        emblems[2] = PlayerPrefs.GetInt("snowClouds", 0) == 1;
        emblems[3] = PlayerPrefs.GetInt("snowRain", 0) == 1;

        return emblems;
    }

    public bool[] GetSunLevelEmblems()
    {
        bool[] emblems = new bool[4];
        emblems[0] = PlayerPrefs.GetInt("sunClear", 0) == 1;
        emblems[1] = PlayerPrefs.GetInt("sunSnow", 0) == 1;
        emblems[2] = PlayerPrefs.GetInt("sunClouds", 0) == 1;
        emblems[3] = PlayerPrefs.GetInt("sunRain", 0) == 1;

        return emblems;
    }

    public void SunLevelComplete()
    {
        PlayerPrefs.SetInt("sun" + weatherState.getWeatherType(), 1);
        IncreaseSunProg();
        PlayerPrefs.Save();
    }

    public void SnowLevelComplete()
    {
        PlayerPrefs.SetInt("snow" + weatherState.getWeatherType(), 1);
        IncreaseSnowProg();
        PlayerPrefs.Save();
    }

    public void StormLevelComplete()
    {
        PlayerPrefs.SetInt("storm" + weatherState.getWeatherType(), 1);
        IncreaseStormProg();
        PlayerPrefs.Save();
    }

    public void RainLevelComplete()
    {
        PlayerPrefs.SetInt("rain" + weatherState.getWeatherType(), 1);
        IncreaseRainProg();
        PlayerPrefs.Save();
    }

    public int GetSunProg()
    {
        return PlayerPrefs.GetInt("sun", 0);
    }

    public int GetSnowProg()
    {
        return PlayerPrefs.GetInt("snow", 0);
    }

    public int GetStormProg()
    {
        return PlayerPrefs.GetInt("storm", 0);
    }
    public int GetRainProg()
    {
        return PlayerPrefs.GetInt("rain", 0);
    }
    public void SunProgTest()
    {
        IncreaseSunProg();
    }

    public void SnowProgTest()
    {
        IncreaseSnowProg();
    }

    public void StormProgTest()
    {
        IncreaseStormProg();
    }

    public void RainProgTest()
    {
        IncreaseRainProg();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Used for updating on screen debugging text:
    public void Alert()
    {
        TMP_Text text = textObj.GetComponent<TMP_Text>();
        text.text = GetSunProg().ToString();
        Debug.Log(GetSunProg().ToString());
    }
}
