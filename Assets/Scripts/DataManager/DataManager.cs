using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    [SerializeReference] private GameObject textObj;

    public void SunProgTest()
    {
        IncreaseSunProg();
    }


    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void Alert()
    {
        TMP_Text text = textObj.GetComponent<TMP_Text>();
        text.text = GetSunProg().ToString();
        Debug.Log(GetSunProg().ToString());
    }

        public bool IncreaseSunProg()
    {
        int current = PlayerPrefs.GetInt("sun", 0);

        if (current >= 3)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("sun", current+1);
            PlayerPrefs.Save();
            return true;
        }
    }

    public bool IncreaseSnowProg()
    {
        int current = PlayerPrefs.GetInt("snow", 0);

        if (current >= 3)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("snow", current + 1);
            PlayerPrefs.Save();
            return true;
        }

    }
    public bool IncreaseStormProg()
    {
        int current = PlayerPrefs.GetInt("storm", 0);

        if (current >= 3)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("storm", current + 1);
            PlayerPrefs.Save();
            return true;
        }
    }
    public bool IncreaseRainProg()
    {
        int current = PlayerPrefs.GetInt("rain", 0);

        if (current >= 3)
        {
            return false;
        }
        else
        {
            PlayerPrefs.SetInt("rain", current + 1);
            PlayerPrefs.Save();
            return true;
        }

    }

    public int GetSunProg()
    {
        return PlayerPrefs.GetInt("sun", 0);    
    }

    public int GetSnowProg()
    {
        return PlayerPrefs.GetInt("snow", 0);
    }

    public int GetStormProg()
    {
        return PlayerPrefs.GetInt("storm", 0);
    }
    public int GetRainProg()
    {
        return PlayerPrefs.GetInt("rain", 0);
    }
}
