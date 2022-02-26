using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PeopleData.Data.Models.Response.Enums
{
    public enum ResponseCode
    {
        [Description("Request was successful")]
        Ok = 0,
        [Description("Request failed. Please try again")]
        Failed = 1,
        [Description("Exception throwed")]
        Exeception = 2
   
    }

    
}
