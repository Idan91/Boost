﻿using System;

namespace Boost.Model.DataClasses
{
    // Created for the Best Times heatmap
    public struct DayAndHour
    {
        #region Data Members

        public const int k_NumOfWeekDays = 7;
        public const int k_NumOfHours = 24;

        #endregion

        #region Properties

        public DayOfWeek DayOfWeek { get; private set; }

        public TimeSpan HourOfDay { get; private set; }

        #endregion

        #region Ctor

        public DayAndHour(DayOfWeek i_DayOfWeek, TimeSpan i_HourOfDay)
        {
            if(i_HourOfDay.Minutes > 30)
            {
                TimeSpan hourToAddParser = new TimeSpan(0, 1, 0, 0);
                TimeSpan timeSpanWithOneHour = i_HourOfDay.Add(hourToAddParser);
                HourOfDay = timeSpanWithOneHour;
            }
            else
            {
                HourOfDay = i_HourOfDay;
            }

            DayOfWeek = i_DayOfWeek;
        }

        #endregion
    }
}
