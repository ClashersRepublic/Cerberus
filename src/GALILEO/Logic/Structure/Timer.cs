using BL.Servers.CoC.Extensions;

namespace BL.Servers.CoC.Logic.Structure
{
    using System;
    internal class Timer
    {
        internal int Seconds;
        internal DateTime StartTime;
        internal int EndTime;

        internal Timer()
        {
            this.StartTime = new DateTime(1970, 1, 1);
            this.EndTime = 0;
            this.Seconds = 0;
        }
        internal void FastForward(int seconds) => this.Seconds -= seconds;

        internal void IncreaseTime(int seconds)
        {
            this.Seconds += seconds;
            this.EndTime += seconds;
        }

        internal int GetRemainingSeconds(DateTime time, bool boost, DateTime boostEndTime = default(DateTime), float multiplier = 0f)
        {
            int result;
            if (!boost)
            {
                result = this.Seconds - (int)time.Subtract(this.StartTime).TotalSeconds;
            }
            else
            {
                if (boostEndTime >= time)
                    result = this.Seconds - (int)(time.Subtract(this.StartTime).TotalSeconds * multiplier);
                else
                {
                    var boostedTime = (float)time.Subtract(this.StartTime).TotalSeconds - (float)(time - boostEndTime).TotalSeconds;
                    var notBoostedTime = (float)time.Subtract(this.StartTime).TotalSeconds - boostedTime;

                    result = this.Seconds - (int)(boostedTime * multiplier + notBoostedTime);
                }
            }
            if (result <= 0)
                result = 0;
            return result;
        }

        internal int GetRemainingSeconds(DateTime time)
        {
            int result = this.Seconds - (int)time.Subtract(this.StartTime).TotalSeconds;
            if (result <= 0)
            {
                result = 0;
            }
            return result;
        }

        internal DateTime GetStartTime => this.StartTime;
        internal DateTime GetEndTime => TimeUtils.FromUnixTimestamp(EndTime);

        internal void StartTimer(DateTime time, int durationSeconds)
        {
            this.StartTime = time;
            this.EndTime = (int)TimeUtils.ToUnixTimestamp(time) + durationSeconds;
            this.Seconds = durationSeconds;
        }
    }
}
