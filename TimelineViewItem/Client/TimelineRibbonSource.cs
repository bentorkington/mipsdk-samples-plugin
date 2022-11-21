using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using VideoOS.Platform.Client;
using VideoOS.Platform.Util;

namespace TimelineViewItem.Client
{
    public class TimelineRibbonSource : TimelineSequenceSource
    {
        private Guid _sourceId;

        public TimelineRibbonSource()
        {
            _sourceId = Guid.NewGuid();
        }

        public override Guid Id
        {
            get
            {
                return _sourceId;
            }
        }

        public override TimelineSequenceSourceType SourceType
        {
            get
            {
                return TimelineSequenceSourceType.Ribbon;
            }
        }

        public override string Title
        {
            get
            {
                return "Sample data";
            }
        }

        public override Color RibbonContentColor
        {
            get
            {
                return Color.Blue;
            }
        }

        private void GetSomeData(object argument)
        {
            var intervals = argument as IEnumerable<TimeInterval>;
            var results = new List<TimelineSourceQueryResult>();
            foreach(var interval in intervals)
            {
               // adding some fake data - in a real scenario data should come from a real source
                results.Add(new TimelineSourceQueryResult(interval) { Sequences = GetOneResult(interval) });
            }
            OnSequencesRetrieved(results);
        }

        private IEnumerable<TimelineDataArea> GetOneResult(TimeInterval interval)
        {
            var result = new List<TimelineDataArea>();
            var nextBeginTime = new DateTime(interval.StartTime.Year, interval.StartTime.Month, interval.StartTime.Day, interval.StartTime.Hour, interval.StartTime.Minute, 0, DateTimeKind.Utc);
            if (nextBeginTime < interval.StartTime)
                nextBeginTime = nextBeginTime.AddMinutes(1);
            var nextEndTime = nextBeginTime.AddMinutes(1);

            // Simulate a process that makes 10 minute batches of events available on completion of that 10 minute period.

            var minTicks = Math.Min(DateTime.UtcNow.AddMinutes(-10).Ticks, interval.EndTime.Ticks);
            var endTime = new DateTime(minTicks, DateTimeKind.Utc);

            while (nextEndTime <= endTime)
            {
                result.Add(new TimelineDataArea(new TimeInterval(nextBeginTime, nextEndTime)));
                nextBeginTime = nextBeginTime.AddMinutes(1);
                nextEndTime = nextBeginTime.AddMinutes(1);
            }
            return result;
        }

        public override void StartGetSequences(IEnumerable<TimeInterval> intervals)
        {
            var t = new Thread(GetSomeData);
            t.Name = "Timeline sample data thread";
            t.Start(intervals);
        }
    }
}
