using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour  where T : class {

    public static T Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if(Instance != this as T)
        {
            Debug.LogWarning($"Destroying {gameObject.name}. Another instance of object has been found");
            Destroy(gameObject);
        }
        OnAwake();
    }

    public virtual void OnAwake()
    {

    }
}
