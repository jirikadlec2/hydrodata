using System;
using System.Text.RegularExpressions;
using jk.plaveninycz;
using jk.plaveninycz.Bll;
using jk.plaveninycz.BO;

namespace jk.plaveninycz.Validation
{

    /// <summary>
    /// A helper class which validates query string input
    /// and retrieves the parameters for web chart engine
    /// </summary>
    public class QueryStringValidator
    {
        public QueryStringValidator(string QueryString)
        {
            _qstr = QueryString;
            _writer = new MessageWriter();
        }

        public void Validate()
        {
            Regex reg = new Regex(
                @"(?<1>[a-z]{2})?-*(?<2>[a-z0-9]{3})-(?<3>\d+)-(?<4>\d{8})-(?<5>\d{8})",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match m = reg.Match(_qstr);
            if (m.Success)
            {
                ValidateLanguage(m.Groups[1].Value);
                ValidateVariable(m.Groups[2].Value);
                ValidateStation(m.Groups[3].Value);
                ValidateDate(m.Groups[4].Value, 1); //start date
                ValidateDate(m.Groups[5].Value, 2); //end date
            }
            else
            {
                _IsValid = false;
                _message = MessageType.BadQueryStringFormat; // "Incorrect query string!";
            }
        }
        public string Culture
        {
            get { return _culture; }
        }

        public string ErrorMessage
        {
            get 
            {
                return _writer.WriteErrorMessage(_message);
            }
        }
        
        public bool IsValid
        {
            get { return _IsValid; }
        }

        public int StationId
        {
            get { return _StId; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
        }

        public Variable Variable
        {
            get { return _var; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
        }

        // function to validate language shortcut and set the
        // value of _VarEnum
        private void ValidateLanguage(string str)
        {
            if ( _IsValid )
            {
                switch ( str )
                {
                    case "en":
                        _culture = "en-us";
                        break;
                    case "cz":
                    case "cs":
                        _culture = "cs-cz";
                        break;
                    default:
                        _culture = "en-us";
                        _IsValid = false;
                        _message = MessageType.NoLanguage; // "Bad language specified!";
                        break;
                }

                // also set the current culture for localized chart generation
                System.Threading.Thread.CurrentThread.CurrentCulture =
                   new System.Globalization.CultureInfo(_culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                   new System.Globalization.CultureInfo(_culture);
            }
        }
        
        
        // function to validate variable id and set the value of
        //_VarEnum
        private void ValidateVariable(string str)
        {
            if ( _IsValid )
            {
                Variable var = VariableManager.GetItemByShortName(str);
                if ( var.IsValid )
                {
                    _var = var;
                }
                else
                {
                    _IsValid = false;
                    _message = MessageType.NoVariable; // "Bad variable specified!";
                }
            }
        }

        private void ValidateStation(string str)
        {
            if ( _IsValid )
            {
                _StId = Convert.ToInt32(str);
            }
        }

        private void ValidateDate(string date, int order)
        {
            if ( _IsValid )
            {
                try
                {
                    DateTime convDate = new DateTime
                        (Convert.ToInt32(date.Substring(0,4)), 
                            Convert.ToInt32(date.Substring(4,2)), 
                            Convert.ToInt32(date.Substring(6,2)));
                    if ( order == 1 )
                    {
                        _startDate = convDate;
                    }
                    else
                    {
                        _endDate = convDate;
                    }
                }
                catch (System.Exception)
                {
                    _IsValid = false;
                    _message = MessageType.BadDateFormat; // "Bad date format, please use 'yyyyMMdd' format";
                }
            }
        }

        #region private_members
        private string _qstr;
        private bool _IsValid = true;
        private string _culture = "en";
        private int _StId = 0;
        private DateTime _endDate;
        private Variable _var;
        private DateTime _startDate;
        private MessageType _message;

        private MessageWriter _writer;

        #endregion
    }
}
