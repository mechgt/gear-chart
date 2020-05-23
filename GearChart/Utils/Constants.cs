﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GearChart.Utils
{
    static class Constants
    {
        public static readonly UInt16 SecondsPerMinute = 60;
        public static readonly UInt16 MinutesPerHour = 60;
        public static readonly UInt16 SecondsPerHour = (UInt16)(MinutesPerHour * SecondsPerMinute);
        public static readonly Color[] LineChartTypesColors = new Color[]
                    { Color.FromArgb(78, 154, 6),               // Cadence = 0
                      Color.FromArgb(143, 89, 2),               // Elevation = 1
                      Color.FromArgb(204, 0, 0),                // HeartRateBPM = 2
                      Color.FromArgb(204, 0, 0),                // HeartRatePercentMax = 3
                      Color.FromArgb(92, 53, 102),              // Power
                      Color.FromArgb(193, 125, 17),             // Grade = 5
                      Color.FromArgb(32, 74, 135) };            // Speed = 6
    }
}
