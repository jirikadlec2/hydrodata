using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Resources;
using System.Text;
using jk.plaveninycz.BO;
using Presentation;

namespace jk.plaveninycz.Presentation
{
    public class StatisticsItem
    {
        public StatisticsItem(string statCode, string statValue)
        {
            _statCode = statCode;
            _statValue = statValue;
        }

        public StatisticsItem(string statCode, string statValue, ResourceManager manager)
        {
            _statCode = statCode;
            _statValue = statValue;
            _manager = manager;
        }

        public string Label
        {
            get
            {
                if (_manager != null)
                {
                    return _manager.GetString(_statCode);
                }
                else
                {
                    return _statCode;
                }
            }
        }

        public string Value
        {
            get
            {
                return _statValue;
            }
        }

        private string _statCode;
        private string _statValue;
        private ResourceManager _manager = null;
    }

    /// <summary>
    /// Represents a list of observation statistics
    /// that can be bound to a control on the presentation
    /// website. It ensures that the labels are correctly
    /// localized.
    /// </summary>
    public class StatisticsList : List<StatisticsItem>
    {
        

        private void AddItem(string label, string value)
        {
            Add(new StatisticsItem(label, value, _manager));
        }

        private ResourceManager _manager;
    }
    
    /// <summary>
    /// This class presents a list of 
    /// statistical summary for a time series
    /// of observations. It ensures that the
    /// labels and values are in the correct language.
    /// The strings are saved StatisticsList.resx file.
    /// </summary>
    internal class StatisticsList2 : IEnumerable
    {
        #region Private Fields

        private ListDictionary _statList;
        private ResourceManager _manager;

        #endregion
        
        

        private void AddItem(string key, string val)
        {
            _statList.Add(key, val);
        }        

        #region Public Methods
        public StatisticsItem this[int index]
        {
            get 
            {
                DictionaryEntry entry = (DictionaryEntry) _statList[index];
                string localizedLabel = _manager.GetString(entry.Key.ToString());
                return new StatisticsItem(localizedLabel, entry.Value.ToString());
            }
        }

        public int Count
        {
            get { return _statList.Count; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator  GetEnumerator()
        {
            return new StatisticsEnumerator(this);
        }

        #endregion
    }


    public class StatisticsEnumerator : IEnumerator
    {
        internal StatisticsEnumerator(StatisticsList2 list)
        {
            statList = list;
            Reset();
        }
        
        private StatisticsList2 statList;
        private int index;
        
        #region IEnumerator Members

        public object Current
        {
            get
            {
                return (statList[index]);
            }
        }

        public bool MoveNext()
        {
            return (++index < statList.Count);
        }

        public void Reset()
        {
            index = -1;
        }

        #endregion
    }

    
}
