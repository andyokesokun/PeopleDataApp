using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleData.Data.Models.Response
{
    public class People
    {
        public ICollection<Person> Value { get; set; }
    }
}
