using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PeopleData.Data.Models.Response.Enums
{
    public enum  PersonFilter
    {
        [Description(nameof(Person.FirstName))]
        FIRSTNAME=1,
        [Description(nameof(Person.LastName))]
        LASTNAME=2,
        [Description(nameof(Person.Gender))]
        GENDER = 3
    }
}
