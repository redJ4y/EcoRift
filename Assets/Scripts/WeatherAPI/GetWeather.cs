using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GetWeather : MonoBehaviour
{
    [SerializeField] private string currentWeather;
    [SerializeField] private GameObject tileGrid;
    [SerializeField] private Sprite[] snowSprites;
    [SerializeField] private Sprite[] rainSprites;
    [SerializeField] private Sprite[] normalSprites;
    private Sprite[] activeSprites;
    private GameObject activeTileMap;
    private List<GameObject> weatherBackgrounds;

    void Start()
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
            transform.Find("Snow Particles").gameObject
        };
        weatherBackgrounds = new List<GameObject>(bgs);

        fetchAPIData();
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
            replaceCurrentTiles(normalSprites);
        }
        else if (currentWeather == "Clear")
        {
            weatherBackgrounds.Find(obj => obj.name == "Sunny").SetActive(true);
            replaceCurrentTiles(normalSprites);
        }
        else if (currentWeather == "Rain")
        {
            weatherBackgrounds.Find(obj => obj.name == "Rainy").SetActive(true);
            weatherBackgrounds.Find(obj => obj.name == "Rain Particles").SetActive(true);
            replaceCurrentTiles(rainSprites);
        }
        else if (currentWeather == "Snow")
        {
            weatherBackgrounds.Find(obj => obj.name == "Snowy").SetActive(true);
            weatherBackgrounds.Find(obj => obj.name == "Snow Particles").SetActive(true);
            replaceCurrentTiles(snowSprites);
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
