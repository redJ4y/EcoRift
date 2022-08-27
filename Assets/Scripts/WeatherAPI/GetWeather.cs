using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeather : MonoBehaviour
{
    private string currentWeather;

    void Start()
    {
        Debug.Log("Running weather API");

        Weather[] weatherList = APIHelper.GetWeatherList();

        foreach (Weather w in weatherList)
        {
            currentWeather = w.main;
            Debug.Log(w.main);
            if (w.main == "Clouds")
            {
                Camera.main.backgroundColor = Color.grey;
            }
            else if (w.main == "Clear")
            {
                Camera.main.backgroundColor = Color.grey;
            }
            else if (w.main == "Rain")
            {
                Camera.main.backgroundColor = Color.grey;
            }
        }
    }

    public string getWeatherType()
    {
        return currentWeather;
    }
}
