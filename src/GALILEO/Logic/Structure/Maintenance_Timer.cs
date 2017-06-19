using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRepublic.Magic.Extensions;

namespace CRepublic.Magic.Logic.Structure
{
    internal class Maintenance_Timer
    {
        internal int Seconds;
        internal DateTime StartTime;
        internal int EndTime;

        internal Maintenance_Timer()
        {
            this.StartTime = new DateTime(1970, 1, 1);
            this.EndTime = 0;
            this.Seconds = 0;
        }

        internal void FastForward(int seconds)
        {
            this.Seconds -= seconds;
            this.EndTime -= seconds;
        }

        internal void IncreaseTime(int seconds)
        {
            this.Seconds += seconds;
            this.EndTime += seconds;
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
