using UnityEngine;
using System.Collections;
using TMPro;

public class LocationManager : MonoBehaviour
{
    [SerializeReference]
    public APIHelper apiHelperScript;

    public IEnumerator Start()
    {
        // Wait until the editor and unity remote are connected before starting a location service
        yield return new WaitForSeconds(5);
      
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            print("Location not enabled");
            apiHelperScript.StartRequest(); // Begin API request with default lat and lon
            yield break;
        }
        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            float lat = Input.location.lastData.latitude;
            float lon = Input.location.lastData.longitude;

            APIData.lat = lat.ToString();
            APIData.lon = lon.ToString();
            apiHelperScript.StartRequest();
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
}