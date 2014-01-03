using System;
using System.Collections.Specialized;
using System.Text;
using jk.plaveninycz;
using jk.plaveninycz.DataSources;
using jk.plaveninycz.Validation;
using jk.plaveninycz.BO;
using jk.plaveninycz.Bll;

namespace jk.plaveninycz.Web
{
    /// <summary>
    /// validation of the main page query string,
    /// such as 'graf.aspx?lang=cz&var=snw&st=lysa-hora'
    /// this class checks a valid station, variable and
    /// station-variable combination.
    /// </summary>
    public class PageUrlValidator
    {
        #region Constructor
        public PageUrlValidator(NameValueCollection queryString)
        {
            _queryString = queryString;
            _isValid = false;
            _writer = new MessageWriter();
        }
        #endregion

        #region Properties
        public Variable Variable
        {
            get { return _variable; }
        }

        public Station Station
        {
            get { return _station; }
        }

        public bool IsValid
        {
            get { return _isValidVariable && _isValidStation && _isValidStationVariable; }
        }

        public bool IsValidStation
        {
            get { return _isValidStation; }
        }

        public bool IsValidVariable
        {
            get { return _isValidVariable; }
        }

        public bool IsValidStationVariable
        {
            get { return _isValidStationVariable; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Checks if there's a valid variable, station and
        /// station-variable combination
        /// </summary>
        public void Validate()
        {
            ValidateVariable();
            ValidateStation();
        }
        #endregion

        #region Private Methods
        private void ValidateVariable()
        {
            string varUrl;
            if ( _queryString["var"] != null )
            {
                varUrl = _queryString["var"];
                Variable var = VariableManager.GetItemByUrl(varUrl);
                if ( var.IsValid )
                {
                    _variable = var;
                    _isValidVariable = true;
                }
                else
                {
                    _errorMessage = _writer.WriteErrorMessage(MessageType.NoVariable);
                }
            }
            else
            {
                _variable = VariableManager.GetItemByEnum(VariableEnum.Stage); //default variable
            }
        }

        private void ValidateStation()
        {
            string stUrl;
            if ( _isValidVariable )
            {
                if ( _queryString["st"] != null )
                {
                    stUrl = _queryString["st"];
                }
                else
                {
                    stUrl = getDefaultStationUrl(_variable);
                }

                Station st = StationManager.GetItemByUrlAndVariable(stUrl, _variable.Id, false);
                if ( st.Loaded )
                {
                    _station = st;
                    _isValidStation = true;
                    _isValidStationVariable = true;
                }
                else
                {
                    st = StationManager.GetItemByUrl(stUrl, false);
                    if ( st.Loaded )
                    {
                        _station = st;
                        _isValidStation = true;
                        _errorMessage =
                            _writer.WriteErrorMessage(MessageType.NoStationVariable);
                    }
                    else
                    {
                        _errorMessage = _writer.WriteErrorMessage(MessageType.NoStation);
                        _station = new Station(0, stUrl, stUrl);
                    }
                }
            }
        }

        //this station is displayed on initial pages
        //with no station specified
        private string getDefaultStationUrl(Variable v)
        {
            VariableEnum varEnum = v.VarEnum;
            string stUrl;
            switch ( varEnum )
            {
                case VariableEnum.Snow:
                    stUrl = "lysa_hora";
                    break;
                case VariableEnum.Stage:
                    stUrl = "morava/straznice";
                    break;
                case VariableEnum.Discharge:
                    stUrl = "morava/straznice";
                    break;
                case VariableEnum.Precip:
                    stUrl = "churanov";
                    break;
                default:
                    stUrl = "primda";
                    break;
            }
            return stUrl;
        }
        #endregion

        //private DataManager _manager;
        private NameValueCollection _queryString;
        private Station _station;
        private Variable _variable;
        private bool _isValid;
        private string _errorMessage;
        private MessageWriter _writer;

        private bool _isValidStation = false;
        private bool _isValidVariable = false;
        private bool _isValidStationVariable = false;
    }
}
