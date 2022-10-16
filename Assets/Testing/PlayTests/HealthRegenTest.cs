using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthRegenTest
{
    // A UnityTest behaves like a coroutine in Play Mode.
    [UnityTest]
    public IEnumerator HealthRegenTestWithEnumeratorPasses()
    {
        Health health = new GameObject("HealthTest").AddComponent<Health>();
        health.PrepareForTest();
        float startingHP = health.GetHp();
        health.TakeDamage(20.0f);

        yield return new WaitForSeconds(8);
        Assert.Greater(health.GetHp(), startingHP - 20.0f);

        startingHP = health.GetHp(); // Update startingHP for next test...
        yield return new WaitForSeconds(8);
        Assert.Greater(health.GetHp(), startingHP);
    }
}
