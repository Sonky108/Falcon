using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Falcon
{
    public static class UIStatisticsUtils
    {
        public const string FuelTitle = "Fuel";
        public const string CrashesTitle = "Crashes";
        public const string SuccesfulLandingTitle = "Succesful landings";

        public static string FormatUIStatistic<T>(string title, T value)
        {
            return string.Format("{0}: {1}", title, value.ToString());
        }

        public static string FormatPercentageUIStatistic(string title, float value)
        {
            var valuePercentage = value * 100;
            var valueString = string.Format("{0:0.00}%", valuePercentage);
            return FormatUIStatistic(title, valueString);
        }

        public static string FormatPercentageUIStatistic(string title, int value)
        {
            return FormatPercentageUIStatistic(title, (float)value);
        }
    }
}
