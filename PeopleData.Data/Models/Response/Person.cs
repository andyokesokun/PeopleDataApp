using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleData.Data.Models.Response
{
    public class Person
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public ICollection<string> Emails { get; set; } = new List<string>();
        public string FavoriteFeature { get; set; }
        public ICollection<string> Features { get; set; } = new List<string>();
        public ICollection<AddressDescription> AddressInfo { get; set; } = new List<AddressDescription>();
        public AddressDescription HomeAddress { get; set; }

    }
}
