using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine.Networking;

public class APIHelper : MonoBehaviour
{
    public GetWeather getWeatherScript;

    /*
    public static string GetCurrentWeather()
    {
        // Brisbane: lat: -27.469770, lon: 153.025131
        // Auckland: lat: -36.852095, lon: 174.7631803

        string lat = "-27.469770";
        string lon = "153.025131";
        string key = "REMOVED API KEY";
        string APIurl = "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&units=metric&appid=" + key;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIurl);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        return json;
    }


    public static Weather[] GetWeatherList()
    {
        Weather[] weather = JsonHelper.FromJson<Weather>(GetCurrentWeather());

        return weather;
    }
    */

    private void Start()
    {
        startRequest();
    }

    public void startRequest()
    {
        StartCoroutine(MakeRequests());
    }

    private IEnumerator MakeRequests()
    {
        // GET
        // Brisbane: lat: -27.469770, lon: 153.025131
        // Auckland: lat: -36.852095, lon: 174.7631803

        string lat = "-27.469770";
        string lon = "153.025131";
        string key = "REMOVED API KEY";
        string APIurl = "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&units=metric&appid=" + key;

        UnityWebRequest getRequest = CreateRequest(APIurl);
        yield return getRequest.SendWebRequest();
        //var deserializedGetData = JsonUtility.FromJson<Todo>(getRequest.downloadHandler.text);
        Weather[] deserialisedGetData = JsonHelper.FromJson<Weather>(getRequest.downloadHandler.text);
        getWeatherScript.fetchAPIData(deserialisedGetData);
        // Trigger continuation of game flow
    }

    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        UnityWebRequest request = new UnityWebRequest(path, type.ToString());

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }
}

public enum RequestType
{
    GET = 0,
    POST = 1,
    PUT = 2
}