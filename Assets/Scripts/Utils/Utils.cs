using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Utils
{
    public static class Utils
    {
        public static string FormatTime(float timeInSeconds)
        {
            int days = (int)(timeInSeconds / 86400); // 86400 seconds in a day
            int hours = (int)((timeInSeconds % 86400) / 3600);
            int minutes = (int)((timeInSeconds % 3600) / 60);
            int seconds = (int)(timeInSeconds % 60);

            return $"{days}d {hours}h {minutes}m {seconds}s";
        }
    }
}