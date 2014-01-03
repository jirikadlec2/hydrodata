using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.BO
{
    public class Station
    {
        #region Declarations

        int _id = -1;
        string _name;
        string _url;
        string _operator;
        long _riverId = -1;
        long _locationId = -1;
        River _river;
        VariableList _variables;
        ChannelList _channels; //TODO: create channel object

        double _latitude;
        double _longitude;
        double _elevation;

        #endregion


        #region constructors

        /// <summary>
        /// Creates a new station object, given the station id, name, url
        /// and operator info.
        /// this constructor does not require access to the database
        /// </summary>
        public Station(int id, string name, string url)
        {
            _id = id;
            _name = name;
            _url = url;
            _riverId = -1;
        }
        
        public Station(int id, string name, string url, long riverId, string oper)
        {
            _id = id;
            _name = name;
            _url = url;
            _operator = oper;
        }

        #endregion


        #region Properties

        /// <summary>
        /// returns a collection of all variables measured by
        /// the station
        /// </summary>
        public VariableList Variables
        {
            get { return _variables; }
            set { _variables = value; }  
        }

        /// <summary>
        /// collection of all channels belonging to the station
        /// </summary>
        public ChannelList Channels
        {
            get { return _channels; }
            set { _channels = value; }
        }

        /// <summary>
        /// returns a river corresponding to this station
        /// TODO: only use this property for Hydrol. station
        /// </summary>
        public River River
        {
            get { return _river; }
            set { _river = value; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Operator
        {
            get { return _operator; }
        }

        public long RiverId
        {
            get { return _riverId; }
        }

        public long LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }

        public bool IsOnRiver
        {
            get { return (_riverId > 0); }
        }

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public double Elevation
        {
            get { return _elevation; }
            set { _elevation = value; }
        }

        /// <summary>
        /// finds out if the specified station is an existing
        /// station in the database
        /// </summary>
        public bool Loaded
        {
            get
            {
                return (_id > 0);
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (River != null)
            {
                return River.ToString() + " - " + Name;
            }
            return _name;
        }

        /// <summary>
        /// finds out if the station measures a specified variable
        /// </summary>
        public bool HasVariable(Variable var)
        {
            if (_variables != null)
            {
                return (Variables.Contains(var));
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
