using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class APITest
{
    [SetUp]
    public void APITestInitialize()
    {
        APIHelper apiHelper = new GameObject("APIHelperTest").AddComponent<APIHelper>();
        GetWeather weather = new GameObject("WeatherTest").AddComponent<GetWeather>();
        apiHelper.getWeatherScript = weather;
        LocationManager locationManager = new GameObject("LocationManagerTest").AddComponent<LocationManager>();
        locationManager.apiHelperScript = apiHelper;
        locationManager.StartCoroutine(locationManager.Start());
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator APITestWithEnumeratorPasses()
    {
        GetWeather weather = GameObject.Find("WeatherTest").GetComponent<GetWeather>();
        yield return new WaitForSeconds(8);
        string currentWeather = weather.getWeatherType();
        Assert.IsNotNull(currentWeather);
        switch (currentWeather)
        {
            case "Rain":
            case "Snow":
            case "Clear":
            case "Clouds":
                Assert.Pass();
                break;
            default:
                Assert.Fail();
                break;
        }
    }
}
