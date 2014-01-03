using System;
using System.Collections.Generic;
using System.Text;
using jk.plaveninycz;

namespace jk.plaveninycz.BO
{
    /// <summary>
    /// Represents a specific measuring instrument
    /// (station-variable combination)
    /// </summary>
    public class Channel
    {
        #region Declarations

        private int _id;
        private int _stId;
        private int _varId;
        private TimeInterval _availableData;

        private Station _station;
        private Variable _variable;

        #endregion

        #region Constructor

        public Channel(int id, int stationId, int variableId, TimeInterval availableData)
        {
            _id = id;
            _stId = stationId;
            _varId = variableId;
            _availableData = availableData;
        }

        public Channel(int id, Station st, Variable var, TimeInterval availableData)
        {
            _id = id;
            _station = st;
            _variable = var;
            _availableData = availableData;
            _stId = _station.Id;
            _varId = _variable.Id;
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
        }

        public int StationId
        {
            get { return _stId; }
        }

        public int VariableId
        {
            get { return _varId; }
        }

        public Station Station
        {
            get { return _station; }
            set 
            { 
                _station = value;
                _stId = _station.Id;
            }
        }

        public Variable Variable
        {
            get { return _variable; }
            set 
            { 
                _variable = value;
                _varId = _variable.Id;
            }
        }

        public TimeInterval AvailableData
        {
            get { return _availableData; }
        }

        public DateTime StartOfOperation
        {
            get { return AvailableData.Start; }
        }

        public DateTime EndOfOperation
        {
            get { return AvailableData.End; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Variable.ToString() + "-" + Station.ToString();
        }

        #endregion
    }
}
