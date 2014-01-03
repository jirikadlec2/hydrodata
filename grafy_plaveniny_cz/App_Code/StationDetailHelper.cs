using System;
using System.Collections.Generic;
using System.Web;
using jk.plaveninycz.Bll;
using jk.plaveninycz.BO;

/// <summary>
/// Summary description for StationDetailHelper
/// </summary>
public class StationDetailHelper
{
	public StationDetailHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static StationDetailList GetDetailListByVariableOrdered(int variableId, int orderBy)
    {
        StationDetailList sdList = StationManager.GetDetailListByVariableOrdered(variableId, orderBy);
        Variable v = VariableManager.GetItemById(variableId);
        string baseUrl = HttpContext.Current.Request.ApplicationPath + "/" + v.UrlLang + "/" + v.Url + "/";
        
        foreach (StationDetail sd in sdList)
        {
            sd.Url = baseUrl + sd.Url + ".aspx";
        }
        return sdList;
    }
}