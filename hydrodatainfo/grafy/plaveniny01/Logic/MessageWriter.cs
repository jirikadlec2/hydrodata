using System;
using System.Resources;
using jk.plaveninycz.BO;

namespace jk.plaveninycz.Bll
{

    /// <summary>
    /// Special class for creating messages for different input error types
    /// </summary>
    public class MessageWriter
    {
        private string _stName = " ";
        private string _varName = " ";

        public MessageWriter()
        {
            new MessageWriter("", "");
            _rm = jk.plaveninycz.GlobalResource.ResourceManager;
        }
        
        public MessageWriter(string st, string var)
        {
            _rm = jk.plaveninycz.GlobalResource.ResourceManager;
            _stName = st;
            _varName = var;
        }

        public string StationName
        {
            get { return _stName; }
            set { _stName = value; }
        }

        public string VariableName
        {
            get { return _varName; }
            set { _varName = value; }
        }

        public string WriteErrorMessage(MessageType errorType)
        {
            string str = _rm.GetString("ChartError_" + errorType.ToString());
            switch ( errorType )
            {
                case MessageType.NoStation:
                    return String.Format(str, _stName);   
                case MessageType.NoStationVariable:
                    return String.Format(str, _stName, _varName);
                default:
                    break;
            }
            return str;
        }

        private ResourceManager _rm;
    }

    #region Enumerations
    public enum MessageType
    {
        BadQueryStringFormat,
        BadDateFormat,
        NoLanguage,
        NoVariable,
        NoStation,
        NoStationVariable,
        NoDataForPeriod,
        UnknownError
    }
    #endregion

}
