using Falcon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Falcon
{
    public class UIStatistics : MonoBehaviourSingleton<UIStatistics>
    {

        [SerializeField]
        private Text FuelText;

        [SerializeField]
        private Text SuccesfulLandingsText;

        [SerializeField]
        private Text CrashesText;

        private bool showUI = false;

        public override void OnAwake()
        {
            base.OnAwake();

            OnGameEnded();

            GameManager.Instance.OnGameStartedListeners += OnGameStarted;
            GameManager.Instance.OnGameEndedListeners += OnGameEnded;
        }

        private void OnGameStarted(LevelSettings obj)
        {
            showUI = true;

            SetCrashesText();
            SetLandingsText();

            ActivateTextObjects(showUI);
        }

        private void SetLandingsText()
        {
            var value = GetLandingsTest();

            SuccesfulLandingsText.text = value;
        }

        //Fuel text does not need to be update every frame. 
        //It can be update i.e. every 0.1 s.
        private void Update()
        {
            FuelText.text = GetFuelText();
        }

        private void SetCrashesText()
        {
            var value = GetCrashesText();

            CrashesText.text = value;
        }

        private string GetLandingsTest()
        {
            var value = GameManager.Instance.SuccesfulLandings;
            var title = UIStatisticsUtils.SuccesfulLandingTitle;

            return UIStatisticsUtils.FormatUIStatistic(title, value);
        }

        private string GetCrashesText()
        {
            var value = GameManager.Instance.Crashes;
            var title = UIStatisticsUtils.CrashesTitle;

            return UIStatisticsUtils.FormatUIStatistic(title, value);
        }

        private string GetFuelText()
        {
            var value = CharacterMovementController.Instance.GetFuelPercentage();
            var title = UIStatisticsUtils.FuelTitle;

            return UIStatisticsUtils.FormatPercentageUIStatistic(title, value);
        }

        private void OnGameEnded(bool success = false)
        {
            showUI = false;
            ActivateTextObjects(showUI);
        }

        private void ActivateTextObjects(bool enabled)
        {
            FuelText.gameObject.SetActive(enabled);
            SuccesfulLandingsText.gameObject.SetActive(enabled);
            CrashesText.gameObject.SetActive(enabled);
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStartedListeners -= OnGameStarted;
            GameManager.Instance.OnGameEndedListeners -= OnGameEnded;
        }
    }
}
