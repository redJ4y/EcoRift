using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class HealthTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void HealthTestSimplePasses()
    {
        // Test TakeDamage:
        Health health = new GameObject("HealthTest").AddComponent<Health>();
        health.PrepareForTest();
        float startingHP = health.GetHp();
        health.TakeDamage(5.0f);
        Assert.AreEqual(startingHP - 5.0f, health.GetHp());
        // Test RaiseHP:
        health.TakeDamage(10.0f);
        health.raiseHP();
        Assert.AreEqual(startingHP - 15.0f + 10.0f, health.GetHp());
        health.raiseHP();
        Assert.AreEqual(startingHP, health.GetHp());
        // Test BuffHP:
        health.buffHp(2.0f);
        health.raiseHP();
        Assert.Greater(health.GetHp(), startingHP);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator HealthTestWithEnumeratorPasses()
    {
        // Use yield to skip a frame.
        Health health = new GameObject("HealthTest").AddComponent<Health>();
        health.PrepareForTest();
        float startingHP = health.GetHp();
        health.TakeDamage(20.0f);
        DateTime startTime = DateTime.UtcNow;
        yield return new WaitForSeconds(8);
        Assert.Greater(health.GetHp(), startingHP - 20.0f);
        startingHP = health.GetHp(); // Update startingHP for next test...
        startTime = DateTime.UtcNow;
        yield return new WaitForSeconds(8);
        Assert.AreEqual(health.GetHp(), startingHP);
    }
}
