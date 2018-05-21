using Falcon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviourSingleton<SettingsController> {

    public GameSettings gameSettings { get; private set; }

    private string settingsFileName = "settings.json";

    public void LoadGameSettings(System.Action<GameSettings> OnGameLoaded)
    {
        var data = StartCoroutine(PersistentDataController.LoadFromJson<GameSettings>(settingsFileName, OnGameLoaded));
    }

    private void OnGameLoaded(GameSettings data)
    {
        if (data != null)
        {
            gameSettings = data;
        }
    }
}
