using CESCommon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CESJapanDataServices
{
    public class EnumConstants
    {
        public enum Connection
        {
            [StringValue("DefaultConnection")]
            DefaultConnection,
            [StringValue("AWSRetailConnection")]
            JapanRetailConnection
        }

    }
}