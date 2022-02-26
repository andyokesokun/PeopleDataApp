using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PeopleData.Data.Models.Response.Enums
{
    public enum  PersonFilter
    {
        [Description(nameof(Person.FirstName))]
        FIRSTNAME=0,
        [Description(nameof(Person.LastName))]
        LASTNAME=1,
        [Description(nameof(Person.Gender))]
        GENDER =  2
    }
}
