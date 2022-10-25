using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine.Networking;

public class APIHelper : MonoBehaviour
{
    public GetWeather getWeatherScript;

    public void StartRequest()
    {
        StartCoroutine(MakeRequests());
    }

    private IEnumerator MakeRequests()
    {
        // GET
        UnityWebRequest getRequest = CreateRequest(APIData.APIurl);
        yield return getRequest.SendWebRequest();
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