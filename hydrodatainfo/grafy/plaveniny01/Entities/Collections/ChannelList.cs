using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.BO
{
    public class ChannelList: List<Channel>
    {
        public ChannelList() 
        { }

        /// <summary>
        /// Represents the period where all
        /// channels have some available data
        /// </summary>
        public TimeInterval CommonDataPeriod
        {
            get 
            {
                if (Count == 0) return TimeInterval.Missing;
                
                DateTime maxStart = this[0].StartOfOperation;
                DateTime minEnd = this[0].EndOfOperation;
                foreach (Channel ch in this)
                {
                    if (ch.StartOfOperation > maxStart)
                    {
                        maxStart = ch.StartOfOperation;
                    }

                    if (ch.EndOfOperation < minEnd)
                    {
                        minEnd = ch.EndOfOperation;
                    }
                }

                if (minEnd < maxStart)
                {
                    return TimeInterval.Missing;
                }
                else
                {
                    return new TimeInterval(maxStart, minEnd);
                }
            }
        }

        /// <summary>
        /// Represents the period between first
        /// available observation (any channel)
        /// and last available observation (any channel)
        /// </summary>
        public TimeInterval LongestDataPeriod
        {
            get
            {
                if (Count == 0) return TimeInterval.Missing;
                
                DateTime minStart = this[0].StartOfOperation;
                DateTime maxEnd = this[0].EndOfOperation;
                foreach (Channel ch in this)
                {
                    if (ch.StartOfOperation < minStart)
                    {
                        minStart = ch.StartOfOperation;
                    }

                    if (ch.EndOfOperation > maxEnd)
                    {
                        maxEnd = ch.EndOfOperation;
                    }
                }
                return new TimeInterval(minStart, maxEnd);
            }
        }
    }
}
