using UnityEngine;
using System.Net;
using System.IO;

public static class APIHelper
{
    public static string GetCurrentWeather()
    {
        // Brisbane: lat: -27.469770, lon: 153.025131
        // Auckland: lat: -36.852095, lon: 174.7631803

        string lat = "-27.469770";
        string lon = "153.025131";
        string key = "REMOVED API KEY";
        string APIurl = "https://api.openweathermap.org/data/2.5/weather?lat="+lat+"&lon="+lon+"&units=metric&appid="+key;

        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(APIurl);
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        return json;
    }

    public static Weather[] GetWeatherList()
    {
        Weather[] weather = JsonHelper.FromJson<Weather>(GetCurrentWeather());

        return weather;
    }
}
