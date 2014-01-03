using System;
using System.Collections;

namespace jk.plaveninycz.Presentation
{


    /// <summary>
    /// auxiliary class for showing main navigation menu
    /// </summary>
    public class NavMenuItem : IComparable
    {
        public NavMenuItem(string key, string url, string text, string cssClass)
        {
            m_key = key;
            m_url = url;
            m_text = text;
            m_cssClass = cssClass;
        }
        public NavMenuItem(string key, string url, string text)
        {
            m_key = key;
            m_url = url;
            m_text = text;
            m_cssClass = null;
        }

        

        public override string ToString()
        {
            return ( this.Key );
        }

        /// <summary>
        /// change this instance to a current menu item
        /// </summary>
        public void MakeCurrent()
        {
            m_cssClass = "current";
        }

        public void MakeDefault()
        {
            if ( m_cssClass == "current" )
            {
                m_cssClass = null;
            }
        }

        #region public properties

        public string Key
        {
            get { return m_key; }
        }

        public string Url
        { 
            get { return m_url; }
            set { m_url = value; }
        }
        
        public string Text
        {
            get { return m_text; }
        }
        public string CssClass
        {
            get { return m_cssClass; }
        }

        /// <summary>
        /// evaluates if this instance is labelled as "current"
        /// navigation menu item (different graphical appearence
        /// given by CssClass)
        /// </summary>
        /// <returns>true if this is a "current" menu item</returns>
        public bool IsCurrent()
        {
            return ( m_cssClass == "current" );
        }
                
        #endregion

        #region private
        private string m_key;
        private string m_text;
        private string m_cssClass;
        private string m_url;
        #endregion

        

        #region IComparable members
        public int CompareTo(object obj)
        {
            string menuItemKey = (string) obj;
            return ( this.Key.CompareTo(menuItemKey) );
        }
        #endregion


        public override bool Equals(object obj)
        {
            if ( obj.ToString() == this.Key )
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
