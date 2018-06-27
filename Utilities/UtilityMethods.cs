using CESCommon.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CESJapanDataServices.Utilities
{
    public static class UtilityMethods
    {
        public static FacilityInfo QueryFacility(string utilityAreaCode, string ktbNumber,
           string retailerCode)
        {

            return new FacilityInfo();
        }

        public static string GetMyRetailerCode()
        {
            return "12345";
        }
    }
}