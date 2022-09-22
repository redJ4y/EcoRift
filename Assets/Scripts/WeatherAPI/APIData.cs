
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIData
{
    // Brisbane: lat: -27.469770, lon: 153.025131
    // Auckland: lat: -36.852095, lon: 174.7631803

    static public string lat = "-27.469770";
    static public string lon = "153.025131";
    static private string key = "REMOVED API KEY";
    static public string APIurl = "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&units=metric&appid=" + key;
}
