using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetWeather : MonoBehaviour
{
    [SerializeField] private string currentWeather;
    private List<GameObject> weatherBackgrounds;

    void Start()
    {
        // populate weather backgrounds
        GameObject[] bgs = {
            transform.Find("Rainy").gameObject,
            transform.Find("Cloudy").gameObject,
            transform.Find("Snowy").gameObject,
            transform.Find("Sunny").gameObject,
            transform.Find("Rain Particles").gameObject
        };
        weatherBackgrounds = new List<GameObject>(bgs);

        fetchAPIData();
    }

    public void fetchAPIData()
    {
        Weather[] weatherList = APIHelper.GetWeatherList();
        // json object is a list for some reason so parse through that
        foreach (Weather w in weatherList)
        {
            currentWeather = w.main;
            Debug.Log("Current weather: " + w.main);
        }

        updateWeather();
    }

    private void updateWeather()
    {
        // Ensure no backgrounds are currently set
        disableBackgrounds();

        if (currentWeather == "Clouds")
        {
            weatherBackgrounds.Find(obj => obj.name == "Cloudy").SetActive(true);
        }
        else if (currentWeather == "Clear")
        {
            weatherBackgrounds.Find(obj => obj.name == "Sunny").SetActive(true);
        }
        else if (currentWeather == "Rain")
        {
            weatherBackgrounds.Find(obj => obj.name == "Rainy").SetActive(true);
            weatherBackgrounds.Find(obj => obj.name == "Rain Particles").SetActive(true);
        }
        else if (currentWeather == "Snow")
        {
            weatherBackgrounds.Find(obj => obj.name == "Snowy").SetActive(true);
        }
        else
        {
            Debug.Log("Weather not found!");
        }
    }

    private void disableBackgrounds()
    {
        foreach (GameObject bgObj in weatherBackgrounds)
        {
            bgObj.SetActive(false);
        }
    }

    public void dropDownSelect(TMP_Dropdown dropDown)
    {
        switch (dropDown.value)
        {
            case 1:
                currentWeather = "Rain";
                break;
            case 2:
                currentWeather = "Snow";
                break;
            case 3:
                currentWeather = "Clear";
                break;
            case 4:
                currentWeather = "Clouds";
                break;
        }

        if (dropDown.value != 0)
            updateWeather();
    }

    public string getWeatherType()
    {
        return currentWeather;
    }
}
