using System;
using System.Collections.Generic;
using System.Text;
using jk.plaveninycz.BO;

namespace jk.plaveninycz.search
{
    /// <summary>
    /// Represents a results of "find channels by criteria"
    /// search query. The [Channels] collection is null if nothing
    /// was found. The [Stations] collection contatins some data in
    /// case some of the stations matched the criteria.
    /// </summary>
    public class ChannelSearchResult
    {
        private ChannelList _channels;
        private StationList _stations;
        private string _errorMessage;

        public ChannelSearchResult()
        {
            _channels = new ChannelList();
            _stations = new StationList();
        }

        public ChannelSearchResult(ChannelList channels)
        {
            _channels = channels;
            _stations = new StationList();
        }

        public ChannelSearchResult(StationList stations)
        {
            _channels = new ChannelList();
            _stations = stations;
        }
        
        public ChannelList Channels
        {
            get { return _channels; }   
        }

        public StationList Stations
        {
            get 
            {
                if (_channels.Count > 0)
                {
                    return getUniqueStations(_channels);
                }
                else
                {
                    return _stations;
                }
            }
        }

        public bool HasMatchingChannels
        {
            get { return _channels.Count > 0; }
        }

        public bool HasMatchingStations
        {
            get 
            {
                if (_channels.Count > 0)
                {
                    return true;
                }
                else if (_stations.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //The error message to display
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        private StationList getUniqueStations(ChannelList channels)
        {
            if (channels != null)
            {
                List<int> stIdList = new List<int>();
                StationList stations = new StationList();
                foreach (Channel ch in channels)
                {
                    if (!(stIdList.Contains(ch.StationId)))
                    {
                        stations.Add(ch.Station);
                        stIdList.Add(ch.StationId);
                    }
                }
                return stations;
            }
            return null;
        }
    }
}
