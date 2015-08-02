using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api
{
    public class DocHelper
    {
        public HttpResponse Resp { get; set; }

        public string AppPath { get; set; }
        
        public DocHelper(HttpResponse resp, string appPath)
        {
            Resp = resp;
            AppPath = appPath;
        }

        public void Write(string txt)
        {
            Resp.Write(string.Format("<p><a href=\"{0}{1}\">{1}</a></p>", AppPath, txt));
        }
    }
}