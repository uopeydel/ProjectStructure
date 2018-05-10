using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pjs1.Main.Routing
{
    public class UrlInformation
    {
        public string Path { get; set; }
        public string TrimmedPath { get; set; }
        public string[] Segments { get; set; }

        public static UrlInformation Get(string path)
        {
            //Trim special charactor
            var pathTrimmed = path.Trim('/');
            pathTrimmed = pathTrimmed.IndexOf('%') >= 0 ? WebUtility.UrlDecode(pathTrimmed) : pathTrimmed;
            pathTrimmed = pathTrimmed.IndexOf('’') >= 0 ? pathTrimmed.Replace("’", "-") : pathTrimmed;

            var segments = pathTrimmed.Split('/');
            return new UrlInformation
            {
                Path = path,
                TrimmedPath = pathTrimmed,
                Segments = segments
            };
        }
    }
}
