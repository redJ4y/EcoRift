using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GetWeather : MonoBehaviour
{
    [SerializeReference] private InfoScript infoScript; 
    [SerializeField] private string currentWeather;   // String representation of current weather
    [SerializeField] private GameObject tileGrid;     // The game's tile grid to be altered with weather
    [SerializeField] private Sprite[] snowSprites;    // Snow tile sprites for snowy conditions
    [SerializeField] private Sprite[] rainSprites;    // Rain tile sprites for rainy conditions
    [SerializeField] private Sprite[] sunSprites;     // Rain tile sprites for rainy conditions
    [SerializeField] private Sprite[] normalSprites;  // Sunny/cloudy tile sprites for 'normal' conditions

    private Sprite[] activeSprites;                   // Currently used tile sprites
    private GameObject activeTileMap;                 // Currently used tile map
    private List<GameObject> weatherBackgrounds;      // Array of background gameobjects for each weather

    void Start()
    {
        if (tileGrid != null)
        {
            activeTileMap = tileGrid.transform.Find("Ground").gameObject;
            activeSprites = normalSprites;

            // populate weather backgrounds
            GameObject[] bgs = {
            transform.Find("Rainy").gameObject,
            transform.Find("Cloudy").gameObject,
            transform.Find("Snowy").gameObject,
            transform.Find("Sunny").gameObject,
            transform.Find("Rain Particles").gameObject,
            transform.Find("Snow Particles").gameObject,
            transform.Find("Cloud Particles").gameObject,
            transform.Find("Default").gameObject
            };
            weatherBackgrounds = new List<GameObject>(bgs);
        }
    }

    private void replaceCurrentTiles(Sprite[] newSprites)
    {
        if (newSprites != activeSprites)
        {
            Debug.Log("Changing tiles...");
            activeSprites = newSprites;
            Tilemap activeTM = activeTileMap.GetComponent<Tilemap>();

            foreach (var position in activeTM.cellBounds.allPositionsWithin)
            {
                if (!activeTM.HasTile(position))
                {
                    continue;
                }
                else
                {
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    Sprite currentTile = activeTM.GetSprite(position);
                    newTile.sprite = getSpriteFromName(currentTile.name, newSprites);
                    activeTM.SetTile(position, newTile);
                }
            }
        }
    }

    private Sprite getSpriteFromName(string name, Sprite[] array)
    {
        foreach (Sprite s in array)
        {
            if (s.name == name)
            {
                return s;
            }
        }
        return null;
    }

    public void fetchAPIData(Weather[] weatherList)
    {
        // json object is a list for some reason so parse through that
        foreach (Weather w in weatherList)
        {
            currentWeather = w.main;
        }

        updateWeather();
    }

    private void updateWeather()
    {
        infoScript.Alert("Current weather: " + currentWeather);

        // Buff enemies after weather is updated:
        foreach (FlyingEnemyBrain brain in FindObjectsOfType<FlyingEnemyBrain>())
        {
            brain.UpdateBuff(currentWeather);
        }
        foreach (GroundEnemyBrain brain in FindObjectsOfType<GroundEnemyBrain>())
        {
            brain.UpdateBuff(currentWeather);
        }

        if (weatherBackgrounds != null)
        {
            // Ensure no backgrounds are currently set
            disableBackgrounds();

            if (currentWeather == "Clouds")
            {
                weatherBackgrounds.Find(obj => obj.name == "Cloudy").SetActive(true);
                weatherBackgrounds.Find(obj => obj.name == "Cloud Particles").SetActive(true);
                //replaceCurrentTiles(normalSprites);
            }
            else if (currentWeather == "Clear")
            {
                weatherBackgrounds.Find(obj => obj.name == "Sunny").SetActive(true);
                //replaceCurrentTiles(sunSprites);
            }
            else if (currentWeather == "Rain")
            {
                weatherBackgrounds.Find(obj => obj.name == "Rainy").SetActive(true);
                weatherBackgrounds.Find(obj => obj.name == "Rain Particles").SetActive(true);
                //replaceCurrentTiles(rainSprites);
            }
            else if (currentWeather == "Snow")
            {
                weatherBackgrounds.Find(obj => obj.name == "Snowy").SetActive(true);
                weatherBackgrounds.Find(obj => obj.name == "Snow Particles").SetActive(true);
                //replaceCurrentTiles(snowSprites);
            }
            else
            {
                Debug.Log("Weather not found! " + currentWeather);
            }
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
                currentWeather = "Rain";//rain
                break;
            case 2:
                currentWeather = "Snow";//snow
                break;
            case 3:
                currentWeather = "Clear";//sun
                break;
            case 4:
                currentWeather = "Clouds";//storm
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
