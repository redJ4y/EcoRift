using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionEmblems : MonoBehaviour
{
    [SerializeField] private Image[] snowEmblemImages;
    [SerializeField] private Image[] stormEmblemImages;
    [SerializeField] private Image[] rainEmblemImages;
    [SerializeField] private Image[] sunEmblemImages;

    void Start()
    {
        updateEmblems();
    }

    public void updateEmblems()
    {
        DataManager dataManager = DataManager.Instance;
        int index = 0;
        foreach (bool complete in dataManager.GetRainLevelEmblems())
        {
            if (complete)
            {
                brightenEmblem(rainEmblemImages[index++]);
            }
            else
            {
                darkenEmblem(rainEmblemImages[index++]);
            }
        }
        index = 0;
        foreach (bool complete in dataManager.GetSunLevelEmblems())
        {
            if (complete)
            {
                brightenEmblem(sunEmblemImages[index++]);
            }
            else
            {
                darkenEmblem(sunEmblemImages[index++]);
            }
        }
        index = 0;
        foreach (bool complete in dataManager.GetSnowLevelEmblems())
        {
            if (complete)
            {
                brightenEmblem(snowEmblemImages[index++]);
            }
            else
            {
                darkenEmblem(snowEmblemImages[index++]);
            }
        }
        index = 0;
        foreach (bool complete in dataManager.GetStormLevelEmblems())
        {
            if (complete)
            {
                brightenEmblem(stormEmblemImages[index++]);
            }
            else
            {
                darkenEmblem(stormEmblemImages[index++]);
            }
        }
    }

    public void brightenEmblem(Image emblem)
    {
        emblem.color = new Color(1, 1, 1, 1.0f);
    }

    public void darkenEmblem(Image emblem)
    {
        emblem.color = new Color(1, 1, 1, 0.2f);
    }
}
