using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PersistentDataController {

    public static IEnumerator LoadFromJson<T>(string fileName, System.Action<T> OnLoaded = null)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        bool openingCondition = false;
#if UNITY_IOS || UNITY_EDITOR
        openingCondition = File.Exists(filePath);
#endif
#if UNITY_ANDROID
        openingCondition = true;
#endif
        if (openingCondition)
        {
            string dataAsJson;
#if UNITY_IOS || UNITY_EDITOR
            dataAsJson = File.ReadAllText(filePath);
#endif
#if UNITY_ANDROID
            WWW www = new WWW(filePath);
            while (!www.isDone)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
            dataAsJson = www.text;
#endif

            T data = JsonUtility.FromJson<T>(dataAsJson);
            yield return null;
            OnLoaded?.Invoke(data);
        }
        else
        {
            throw new System.Exception("Cannot load data, file is missing or is broken!");
        }
    }

    public static void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            PlayerPrefs.SetInt(key, defaultValue);
        }

        return defaultValue;
    }
}
