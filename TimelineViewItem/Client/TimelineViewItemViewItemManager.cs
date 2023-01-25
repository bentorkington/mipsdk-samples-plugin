using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// The ViewItemManager contains the configuration for the ViewItem.
    /// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
    /// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.
    /// This class is only relevant when executing in the Smart Client.
    /// </summary>
    public class TimelineViewItemViewItemManager : ViewItemManager
    {
        private List<TimelineSequenceSource> _timelineSequenceSources = new List<TimelineSequenceSource> { new TimelineRibbonSource(10), new TimelineMarkerSource() };

        private double _ribbonLengthInSeconds = 10;

        public double RibbonLengthInSeconds { set
            {
                _ribbonLengthInSeconds = value;
                // recreate the _timelineSequenceSources list with a freshly initialized sources with the new parameters
                _timelineSequenceSources = new List<TimelineSequenceSource>
                {
                    new TimelineRibbonSource(_ribbonLengthInSeconds),
                    new TimelineMarkerSource(),
                };
                EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.RefreshTimelineRequest, null));
                Debug.WriteLine($"set ribbon length to {value} seconds and sent RefreshTimelineRequest");
            }
        }

        public TimelineViewItemViewItemManager() : base("TimelineViewItemViewItemManager")
        {
        }

        #region Methods overridden 

        /// <summary>
        /// Generate the UserControl containing the Actual ViewItem Content
        /// </summary>
        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new TimelineViewItemViewItemWpfUserControl(this);
        }

        public override IEnumerable<TimelineSequenceSource> TimelineSequenceSources
        {
            get
            {
                Debug.WriteLine("SmartClient requested list of TimelineSequenceSources");
                return _timelineSequenceSources;
            }
        }

        public override string DisplayName
        {
            get
            {
                // This will be shown as a header for the individual timeline so provide a name that describes the source
                return "Timeline sample";
            }
        }

        #endregion

    }
}
