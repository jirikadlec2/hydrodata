using System;
using System.Collections;
using System.Collections.Generic;
using jk.plaveninycz.Presentation;


namespace jk.plaveninycz.Presentation
{

    /// <summary>
    /// list of navigation menu items
    /// </summary>
    public class NavMenuList : IEnumerable
    {
        public NavMenuList()
        {
            m_items = new ArrayList();
        }

        public NavMenuItem this[int index]
        {
            get { return ((NavMenuItem) m_items[index]); }
            set { m_items[index] = value; }
        }

        
        /// <summary>
        /// gets or sets the navigation menu item
        /// with the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the corresponding menu item</returns>
        public NavMenuItem this[string key]
        {
            get 
            { 
                return ( this.find(key) ); 
            }
            set
            {
                int index = m_items.IndexOf(key);
                m_items[index] = value;
            }
        }

        public int Count
        {
            get { return m_items.Count; }
        }

        public void Add(NavMenuItem item)
        {
            m_items.Add(item);
        }

        public void Add(string key, string url, string text, string cssClass)
        {
            m_items.Add(new NavMenuItem(key, url, text, cssClass));
        }
        public void Add(string key, string url, string text)
        {
            m_items.Add(new NavMenuItem(key, url, text));
        }


        /// <summary>
        /// returns the NavMenuItem instance based on its key.
        /// if not found, returns null
        /// </summary>
        /// <param name="key">string, specified in menuList member
        /// of master page</param>
        /// <returns></returns>
        public NavMenuItem find(string key)
        {
            int index = m_items.IndexOf(key);
            if ( index >= 0 )
            {
                return (NavMenuItem) m_items[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// changes the menu item specified by key to "current"
        /// the previously selected menu item is switched back
        /// to ordinary
        /// </summary>
        /// <param name="key">the key of selected menu item</param>
        public void SetCurrentItem(string key)
        {
            // set all items to default
            foreach ( NavMenuItem item in this )
            {
                item.MakeDefault();
                if ( item.Key == key )
                {
                    item.MakeCurrent();
                }
            }
            // set specified item to current
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return( new NavMenuListEnumerator( this ));
        }

        #endregion

        
        // arraylist to store the values
        private ArrayList m_items;
    }




    public class NavMenuListEnumerator : IEnumerator
    {
        NavMenuList navList;
        int index;

        #region IEnumerator Members

        public void Reset()
        {
            index = -1;
        }

        public object Current
        {
            get
            {
                return ( navList[index] );
            }
        }

        public bool MoveNext()
        {
            if ( ++index >= navList.Count )
                return ( false );
            else
                return ( true );
        }

        #endregion
        internal NavMenuListEnumerator(NavMenuList navList)
        {
            this.navList = navList;
            Reset();
        }
    }


}
