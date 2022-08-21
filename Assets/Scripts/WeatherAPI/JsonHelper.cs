using UnityEngine;
using System.Net;
using System.IO;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.weather;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] weather;
    }
}