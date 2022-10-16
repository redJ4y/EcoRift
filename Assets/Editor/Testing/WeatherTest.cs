using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class APITest
{
    // A Test behaves as an ordinary method
    [Test]
    public void APITestSimplePasses()
    {
        // Use the Assert class to test conditions
        APIHelper apiHelper = new GameObject("APIHelperTest").AddComponent<APIHelper>();
        IEnumerator makeRequest = apiHelper.MakeRequests();
        makeRequest.MoveNext();
    }
}
