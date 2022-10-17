using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DataManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void DataManagerTestLevelProgress()
    {
        DataManager dataManager = new GameObject("DataManagerTest").AddComponent<DataManager>();
        dataManager.ResetData();
        
        dataManager.SunProgTest();
        int expectedSunResult = 1;
        int sunResult = PlayerPrefs.GetInt("sun");
        Assert.AreEqual(sunResult, expectedSunResult);

        dataManager.SnowProgTest();
        int expectedSnowResult = 1;
        int snowResult = PlayerPrefs.GetInt("snow");
        Assert.AreEqual(snowResult, expectedSnowResult);

        dataManager.StormProgTest();
        int expectedStormResult = 1;
        int stormResult = PlayerPrefs.GetInt("storm");
        Assert.AreEqual(stormResult, expectedStormResult);

        dataManager.RainProgTest();
        int expectedRainResult = 1;
        int rainResult = PlayerPrefs.GetInt("rain");
        Assert.AreEqual(rainResult, expectedRainResult);
    }

    [Test]
    public void DataManagerTestEmblemArray()
    {
        DataManager dataManager = new GameObject("DataManagerTest").AddComponent<DataManager>();
        PlayerPrefs.DeleteAll();

        bool[] results = dataManager.GetRainLevelEmblems();

        Assert.AreEqual(false, results[0]);
        Assert.AreEqual(false, results[1]);
        Assert.AreEqual(false, results[2]);
        Assert.AreEqual(false, results[3]);
    }
}
