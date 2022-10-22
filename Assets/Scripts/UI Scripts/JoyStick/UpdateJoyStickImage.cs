using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateJoyStickImage : MonoBehaviour
{

    [SerializeReference]
    private Sprite[] joySticks;

    [SerializeField]
    private GetWeather weatherState;
    private Image currentImage;


    // Start is called before the first frame update
    void Start()
    {
        currentImage = GetComponent<Image>();
    }
    //Clouds
    //Clear
    //Rain
    //Snow

    public void ChangeImage(string weatherState)
    { 
        switch(weatherState)
        {
            case "Clear":
                currentImage.sprite = joySticks[0];
                break;
            case "Snow":
                currentImage.sprite = joySticks[1];
                break;
            case "Clouds":
                currentImage.sprite = joySticks[2];
                break;
            case "Rain":
                currentImage.sprite = joySticks[3];
                break;
            default:
                currentImage.sprite = joySticks[4];
                break;
        }
    }
}
