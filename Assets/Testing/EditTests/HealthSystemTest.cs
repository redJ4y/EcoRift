using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthSystemTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void HealthSystemTestSimplePasses()
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
}
