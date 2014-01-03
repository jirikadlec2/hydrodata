using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz.BO
{
    public class River
    {
        #region Declarations

        long _id = -1;
        string _name;
        string _url;
        long _recipId;
        River _recipient;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new river object, given the river id, name, url
        /// and recipient id
        /// this constructor does not require access to the database
        /// </summary>
        public River(long id, string name, string url, long recipId)
        {
            _id = id;
            _name = name;
            _url = url;
            _recipId = recipId;
        }

        #endregion


        #region Properties

        public long Id
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

        public River Recipient
        {
            get { return _recipient; }
            set { _recipient = value; }
        }

        public long RecipientId
        {
            get { return _recipId; }
        }

        /// <summary>
        /// finds out if the specified river is an existing
        /// river in the database
        /// </summary>
        public bool IsValid
        {
            get
            {
                return ( _id > 0 );
            }
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
