using System;
using System.Data;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Globalization;
using jk.plaveninycz;
using jk.plaveninycz.DataSources;

namespace jk.plaveninycz.BO
{   
    /// <summary>
    /// Provides information about the observed variable,
    /// enables to identify the variable by different attributes
    /// (url, name, short name, variable ennumeration..)
    /// </summary>
    public class Variable
    {
        public Variable(DataRow row)
        {
            _row = row;
        }

        #region Private_Members
        
        private DataRow _row;

        #endregion

        #region Public Properties

        public System.Globalization.CultureInfo Culture
        {
            get { return CultureHelper.ShortStringToCulture(UrlLang); }
        }

        public string UrlLang
        {
            get { return Convert.ToString(_row["lang_abrev"]); }
        }

        public int Id
        {
            get 
            { 
                return Convert.ToInt32(_row["var_id"]);
            }
        }
        // variable as part of the url (without "/" on either size)
        public string Url
        {
            get
            {          
                return _row["url"].ToString();        
            }
        }

        // commonly used name in the current language
        public string Name
        {
            get 
            {
                return _row["name"].ToString();
            }
        }

        // short name like wtr, snw, pcp
        public string ShortName
        {
            get
            {
                return _row["shortname"].ToString();
            }
        }

        public int TimeIntervalHours
        {
            get
            {
                try
                {
                    return Convert.ToInt32(_row["interval_h"]);
                }
                catch
                {
                    return 24;
                }
            }
        }

        public VariableEnum VarEnum
        {
            get
            {
                VariableEnum v;
                switch ( Convert.ToInt32(_row["var_id"]) )
                {
                    case 1:
                        v = VariableEnum.PrecipHour;
                        break;
                    case 2:
                        v = VariableEnum.Precip;
                        break;
                    case 4:
                        v = VariableEnum.Stage;
                        break;
                    case 5:
                        v = VariableEnum.Discharge;
                        break;
                    case 8:
                        v = VariableEnum.Snow;
                        break;
                    case 12:
                        v = VariableEnum.Evap;
                        break;
                    case 13:
                        v = VariableEnum.SoilWater10;
                        break;
                    case 14:
                        v = VariableEnum.SoilWater50;
                        break;
                    case 15:
                        v = VariableEnum.PrecipSum;
                        break;
                    case 16:
                        v = VariableEnum.Temperature;
                        break;
                    default:
                        v = VariableEnum.Stage;
                        break;
                }
                return v;
            }
        }

        public string Units
        {
            get { return Convert.ToString(_row["units"]); }
        }

        public double ScaleFactor
        {
            get { return Convert.ToDouble(_row["scalefactor"]); }
        }
        
        /// <summary>
        /// indicates if the variable exists in the database
        /// </summary>
        public bool IsValid
        {
            get { return ( _row != null ); }
        }

        #endregion

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if ( obj == null )
            {
                return false;
            }

            // If parameter cannot be cast to Variable return false.
            Variable v = obj as Variable;
            if ( (System.Object) v == null )
            {
                return false;
            }

            // Return true if the fields match:
            return ( Id == v.Id );
        }

        public bool Equals(Variable v)
        {
            // If parameter is null return false:
            if ( (object) v == null )
            {
                return false;
            }

            // Return true if the fields match:
            return ( Id == v.Id );
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}