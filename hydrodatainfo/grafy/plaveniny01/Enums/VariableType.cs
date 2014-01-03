using System;
using System.Collections.Generic;
using System.Text;

namespace jk.plaveninycz
{
    /// <summary>
    /// Ennumerates all observed variables
    /// (precipitation, snow, stage, discharge, evaporation, ...)
    /// </summary>
    public enum VariableEnum
    {
        Snow,
        Precip,
        PrecipHour,
        Stage,
        Discharge,
        Evap,
        SoilWater10,
        SoilWater50,
        PrecipSum,
        Temperature
    }

    /// <summary>
    /// Ennumerates all possible attributes (properties) of the observation
    /// type (url, name, short name, ...)
    /// used in the constructor of VariableInfo to set the
    /// correct parameter
    /// </summary>
    public enum VariableProperty
    {
        Url,
        Name,
        ShortName
    }

}
