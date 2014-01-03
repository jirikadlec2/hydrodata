using System;
using System.Collections.Generic;
using System.Text;
using jk.plaveninycz.BO;
using jk.plaveninycz.Bll;

namespace jk.plaveninycz.search
{
    /// <summary>
    /// Represents functions to search for a channel by various criteria
    /// </summary>
    public class ChannelSearchEngine
    {
        public static ChannelSearchResult FindByUrlAndVariable(string url, Variable v)
        {
            ChannelSearchResult result;

            //TODO: try to use delegates in future version to eliminate duplicate code

            ChannelList channels = ChannelManager.GetListByStationUrlAndVariable(url, v.Id);
            if (channels != null)
            {
                result = new ChannelSearchResult(channels);
                return result;
            }

            MessageWriter writer = new MessageWriter();
            writer.VariableName = v.Name;
            StationList stations = StationManager.GetListByUrl(url);

            if (stations != null)
            {
                result = new ChannelSearchResult(stations);
                writer.StationName = stations[0].Name;
                result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoStationVariable);
                return result;
            }

            result = new ChannelSearchResult();
            result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoVariable);
            return result;
        }

        public static ChannelSearchResult FindByNameAndVariable(string name, Variable v)
        {
            ChannelSearchResult result;

            //TODO: try to use delegates in future version to eliminate duplicate code

            ChannelList channels = ChannelManager.GetListByStationNameAndVariable(name, v.Id);
            if (channels != null)
            {
                result = new ChannelSearchResult(channels);
                return result;
            }

            MessageWriter writer = new MessageWriter();
            writer.VariableName = v.Name;
            StationList stations = StationManager.GetListByName(name);

            if (stations != null)
            {
                result = new ChannelSearchResult(stations);
                writer.StationName = stations[0].Name;
                result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoStationVariable);
                return result;
            }

            result = new ChannelSearchResult();
            result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoStation);
            return result;
        }

        public static ChannelSearchResult FindByStationIdAndVariable(int stId, Variable v)
        {
            ChannelSearchResult result;
            bool foundChannel = false;
            //bool foundStation = false;

            ChannelList channels = ChannelManager.GetListByStationAndVariable(stId, v.Id);
            if (channels.Count > 0)
            {
                result = new ChannelSearchResult(channels);
                foundChannel = true;
                return result;
            }

            //if the requested variable is daily precipitation - try to find
            //hourly precipitation instead
            if (foundChannel == false && v.VarEnum == VariableEnum.Precip)
            {
                //"1" means "hourly precipitation"
                channels = ChannelManager.GetListByStationAndVariable(stId, 1);
                if (channels.Count > 0)
                {
                    foundChannel = true;
                    foreach (Channel ch in channels)
                    {
                        ch.Variable = VariableManager.GetItemByEnum(VariableEnum.Precip);
                    }
                    result = new ChannelSearchResult(channels);
                    return result;
                }
            }

            //channel not found --> search if a a station exists
            MessageWriter writer = new MessageWriter();
            writer.VariableName = v.Name;
            Station station = StationManager.GetItemById(stId, false);

            if (station != null)
            {
                result = new ChannelSearchResult();
                result.Stations.Add(station);
                writer.StationName = station.Name;
                result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoStationVariable);
                return result;
            }

            result = new ChannelSearchResult();
            result.ErrorMessage = writer.WriteErrorMessage(MessageType.NoStation);
            return result;
        }
    }
}
