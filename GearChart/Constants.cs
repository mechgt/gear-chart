using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GearChart
{
    static class Constants
    {
        public static readonly int GearTrackSmoothing = 10;

        public static readonly UInt16 SecondsPerMinute = 60;
        public static readonly UInt16 MinutesPerHour = 60;
        public static readonly UInt16 SecondsPerHour = (UInt16)(MinutesPerHour * SecondsPerMinute);

        public static readonly string GearColumns = "GearColumns";
        public static readonly string GearCharts = "GearCharts";
        public static readonly string BigGears = "BigGears";
        public static readonly string SmallGears = "SmallGears";
        public static readonly string WheelCircumference = "WheelCircumference";
        public static readonly string BikeList = "BikeList";
        public static readonly string Bike = "Bike";
        public static readonly string Id = "Id";
        public static readonly string Item = "Item";
        public static readonly string Gear = "gear";

        public static readonly byte DefaultColumnWidth = 75;
    }
}
