using Falcon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Falcon
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {

        public event Action<GameSettings> OnGameDataLoadedListeners;

        public event Action<bool> OnGameEndedListeners;
        public event Action<LevelSettings> OnGameStartedListeners;

        public List<SpawnerController> spawnPoints;
        public List<PlatformController> platforms;

        private bool gameIsLoaded;

        public int SuccesfulLandings { get; private set; }
        public int Crashes { get; private set; }

        public override void OnAwake()
        {
            base.OnAwake();

            var crashesKey = SettingsUtils.CrashesKey;
            Crashes = PersistentDataController.GetInt(crashesKey);

            var landingsKey = SettingsUtils.LandingKey;
            SuccesfulLandings = PersistentDataController.GetInt(landingsKey);

            Time.timeScale = 0f;
        }

        private void Start()
        {
            LoadGame();
        }

        private void LoadGame()
        {
            SettingsController.Instance.LoadGameSettings(OnGameLoaded);
        }

        private void OnGameLoaded(GameSettings settings)
        {
            SetGravity(settings);

            OnGameDataLoadedListeners?.Invoke(settings);
            gameIsLoaded = true;
            StartGame();
        }

        private void SetGravity(GameSettings data)
        {
            Physics2D.gravity = new Vector2(0, -data.gravity);
        }

        private void StartGame()
        {
            if (gameIsLoaded)
            {
                Time.timeScale = 1f;

                BindListeners();

                var settings = PrepareLevelSettings();

                OnGameStartedListeners?.Invoke(settings);
            }
        }

        private void BindListeners()
        {
            DestroyerController.Instance.OnPlayerTriggeredListeners += OnPlayerTriggeredEdge;

            foreach (var x in platforms)
            {
                x.OnPlayerLandingListeners += OnPlayerLanding;
            }

            CharacterMovementController.Instance.OnFuelEndedListeners += OnFuelEnded;
        }

        private void OnPlayerLanding(bool obj)
        {
            if (obj)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }

        private void Win()
        {
            AddSuccesfulLanding();
            EndGame(true);
        }

        private void OnFuelEnded()
        {
            Lose();
        }

        private LevelSettings PrepareLevelSettings()
        {
            var rand = new System.Random();
            var spawnPoint = spawnPoints.ElementAt(rand.Next(spawnPoints.Count()));

            var startPosition = spawnPoint.GetRandomPosition();

            LevelSettings newSettings = new LevelSettings();
            newSettings.StartPosition = startPosition;

            return newSettings;
        }

        private void OnPlayerTriggeredEdge()
        {
            Lose();
        }

        private void Lose()
        {
            AddCrash();
            EndGame(false);
        }

        private void AddSuccesfulLanding(int value = 1)
        {
            var succesfulLandingKey = SettingsUtils.LandingKey;
            var landings = PersistentDataController.GetInt(succesfulLandingKey);
            landings += value;

            PersistentDataController.SaveInt(succesfulLandingKey, landings);
            SuccesfulLandings = landings;
        }

        private void AddCrash(int value = 1)
        {
            var crashesKey = SettingsUtils.CrashesKey;
            var crashes = PersistentDataController.GetInt(crashesKey);
            crashes += value;

            PersistentDataController.SaveInt(crashesKey, crashes);
            Crashes = crashes;
        }

        private void EndGame(bool won)
        {
            UnbindListeners();

            Time.timeScale = 0f;

            OnGameEndedListeners?.Invoke(won);

            StartCoroutine(StartGameAfterDelay());
        }

        private IEnumerator StartGameAfterDelay()
        {
            yield return new WaitForSecondsRealtime(2f);

            StartGame();
        }

        private void UnbindListeners()
        {
            CharacterMovementController.Instance.OnFuelEndedListeners -= OnFuelEnded;

            DestroyerController.Instance.OnPlayerTriggeredListeners -= OnPlayerTriggeredEdge;

            foreach (var x in platforms)
            {
                x.OnPlayerLandingListeners -= OnPlayerLanding;
            }
        }
    }
}
