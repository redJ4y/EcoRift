using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionEmblems : MonoBehaviour
{
  private bool[] snowEmblems = new bool[4];
  private bool[] stormEmblems = new bool[4];
  private bool[] rainEmblems = new bool[4];
  private bool[] sunEmblems = new bool[4];



  [SerializeField] public Image snowSunEmblem;
  [SerializeField] public Image snowRainEmblem;
  [SerializeField] public Image snowSnowEmblem;
  [SerializeField] public Image snowStormEmblem;

  [SerializeField] public Image stormSunEmblem;
  [SerializeField] public Image stormRainEmblem;
  [SerializeField] public Image stormSnowEmblem;
  [SerializeField] public Image stormStormEmblem;

  [SerializeField] public Image rainSunEmblem;
  [SerializeField] public Image rainRainEmblem;
  [SerializeField] public Image rainSnowEmblem;
  [SerializeField] public Image rainStormEmblem;

  [SerializeField] public Image sunSunEmblem;
  [SerializeField] public Image sunRainEmblem;
  [SerializeField] public Image sunSnowEmblem;
  [SerializeField] public Image sunStormEmblem;

  void Start()
  {
      testEmblems();
  }

  void Update()
  {
    if(snowEmblems[0]==true)
    {
      brightenEmblem(snowSunEmblem);
    }
    else if(snowEmblems[0]==false)
    {
      darkenEmblem(snowSunEmblem);
    }

    if(snowEmblems[1]==true)
    {
      brightenEmblem(snowRainEmblem);
    }
    else if(snowEmblems[1]==false)
    {
      darkenEmblem(snowRainEmblem);
    }

    if(snowEmblems[2]==true)
    {
      brightenEmblem(snowSnowEmblem);
    }
    else if(snowEmblems[2]==false)
    {
      darkenEmblem(snowSnowEmblem);
    }

    if(snowEmblems[3]==true)
    {
      brightenEmblem(snowStormEmblem);
    }
    else if(snowEmblems[3]==false)
    {
      darkenEmblem(snowStormEmblem);
    }

    if(stormEmblems[0]==true)
    {
      brightenEmblem(stormSunEmblem);
    }
    else if(stormEmblems[0]==false)
    {
      darkenEmblem(stormSunEmblem);
    }

    if(stormEmblems[1]==true)
    {
      brightenEmblem(stormRainEmblem);
    }
    else if(stormEmblems[1]==false)
    {
      darkenEmblem(stormRainEmblem);
    }

    if(stormEmblems[2]==true)
    {
      brightenEmblem(stormSnowEmblem);
    }
    else if(stormEmblems[2]==false)
    {
      darkenEmblem(stormSnowEmblem);
    }

    if(stormEmblems[3]==true)
    {
      brightenEmblem(stormStormEmblem);
    }
    else if(stormEmblems[3]==false)
    {
      darkenEmblem(stormStormEmblem);
    }

    if(rainEmblems[0]==true)
    {
      brightenEmblem(rainSunEmblem);
    }
    else if(rainEmblems[0]==false)
    {
      darkenEmblem(rainSunEmblem);
    }

    if(rainEmblems[1]==true)
    {
      brightenEmblem(rainRainEmblem);
    }
    else if(rainEmblems[1]==false)
    {
      darkenEmblem(rainRainEmblem);
    }

    if(rainEmblems[2]==true)
    {
      brightenEmblem(rainSnowEmblem);
    }
    else if(rainEmblems[2]==false)
    {
      darkenEmblem(rainSnowEmblem);
    }

    if(rainEmblems[3]==true)
    {
      brightenEmblem(rainStormEmblem);
    }
    else if(rainEmblems[3]==false)
    {
      darkenEmblem(rainStormEmblem);
    }

    if(sunEmblems[0]==true)
    {
      brightenEmblem(sunSunEmblem);
    }
    else if(sunEmblems[0]==false)
    {
      darkenEmblem(sunSunEmblem);
    }

    if(sunEmblems[1]==true)
    {
      brightenEmblem(sunRainEmblem);
    }
    else if(sunEmblems[1]==false)
    {
      darkenEmblem(sunRainEmblem);
    }

    if(sunEmblems[2]==true)
    {
      brightenEmblem(sunSnowEmblem);
    }
    else if(sunEmblems[2]==false)
    {
      darkenEmblem(sunSnowEmblem);
    }

    if(sunEmblems[3]==true)
    {
      brightenEmblem(sunStormEmblem);
    }
    else if(sunEmblems[3]==false)
    {
      darkenEmblem(sunStormEmblem);
    }
  }

  public void brightenEmblem(Image emblem)
  {
      emblem.color = new Color (1, 1, 1, 1.0f);
  }

  public void darkenEmblem(Image emblem)
  {
      emblem.color = new Color (1, 1, 1, 0.2f);
  }

  public void testEmblems()
  {
    //debug
    snowEmblems[0] = true;
    stormEmblems[1] = true;
    rainEmblems[2] = true;
    rainEmblems[3] = true;
  }
}
