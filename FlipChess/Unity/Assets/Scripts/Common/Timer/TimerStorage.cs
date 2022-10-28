using System;
using UnityEngine;

namespace GameCommon
{
    [GenerateAutoClass]
    public class TimerStorage : ScriptableObject
    {
        #region GenerateId
        private ushort m_value;
        private long m_lastIdTime;

        public long GenerateId()
        {
            long time = TimeSince2020;
            if (time > this.m_lastIdTime)
            {
                this.m_lastIdTime = time;
                this.m_value = 0;
            }
            else
            {
                ++this.m_value;
                if (m_value > ushort.MaxValue - 1)
                {
                    this.m_value = 0;
                    ++this.m_lastIdTime;
                }
            }
            return ToLong((ulong)time, m_value);
        }
        private long ToLong(ulong time, ulong value)
        {
            ulong result = 0;
            result |= value;
            result |= time << 34;
            return (long)result;
        }
        #endregion

        //ºÁÃë×ª»»
        public readonly static long MilliSecondTick = 10000;
        public readonly static long SecondTick = 1000 * MilliSecondTick;
        public readonly static long MinuteTick = SecondTick * 60;
        public readonly static long HourTick = MinuteTick * 60;
        public readonly static long DayTick = HourTick * 24;
        public readonly static long WeekTick = DayTick * 7;

        private readonly DateTime m_gmtDataTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        private readonly long m_gmt2020Time = (new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local).Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).Ticks) / 10000;

        public long OffsetTicks;
        public long Ticks
        {
            get
            {
                return DateTime.UtcNow.ToLocalTime().Ticks - m_gmtDataTime.Ticks + OffsetTicks;
            }
        }
        public long TimeSince2020
        {
            get
            {
                long a = (MilliSecond - m_gmt2020Time) / 1000;
                return a;
            }
        }
        public long MilliSecond
        {
            get
            {
                return Ticks / 10000;
            }
        }
        public long Second
        {
            get
            {
                return MilliSecond / 1000;
            }
        }
        public int Year
        {
            get
            {
                DateTime dataTime = m_gmtDataTime.AddTicks(Ticks);
                return dataTime.Year;
            }
        }
        public int Month
        {
            get
            {
                DateTime dataTime = m_gmtDataTime.AddTicks(Ticks);
                return dataTime.Month;
            }
        }
        public int Day
        {
            get
            {
                DateTime _dataTime = m_gmtDataTime.AddTicks(Ticks);
                return _dataTime.Day;
            }
        }
        public DayOfWeek DayOfWeek
        {
            get
            {
                DateTime dataTime = m_gmtDataTime.AddTicks(Ticks);
                return dataTime.DayOfWeek;
            }
        }
        public bool IsSameDay(long tick1, long tick2)
        {
            if (Math.Abs(tick1 - tick2) > DayTick)
            {
                return false;
            }
            DateTime dataTime1 = m_gmtDataTime.AddTicks(tick1);
            DateTime dataTime2 = m_gmtDataTime.AddTicks(tick2);
            return dataTime1.Day == dataTime2.Day;
        }
    }
}
