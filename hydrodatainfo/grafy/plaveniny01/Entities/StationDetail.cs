using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.BO
{
    public class StationDetail
    {
        #region Declarations

        string _stName;
        string _url;
        string _rivName;
        string _elevation;

        #endregion


        #region constructors

        /// <summary>
        /// Creates a new station detail object
        /// </summary>
        public StationDetail()
        {

        }

        #endregion


        #region Properties

        /// <summary>
        /// returns a river corresponding to this station
        /// TODO: only use this property for Hydrol. station
        /// </summary>
        public string RiverName
        {
            get { return _rivName; }
            set { _rivName = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Name
        {
            get { return _stName; }
            set { _stName = value; }
        }

        public string ElevationString
        {
            get { return (_elevation); }
            set { _elevation = value; }
        }


        public string ObservationPeriodString
        {
            get { return "test"; }
        }


        #endregion
    }
}
