using Bogus;
using PeopleData.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleData.Test.Mock
{
    public static class PeopleSample
    {
        public static People GetPeopleData(int count)
        {

            Faker<People> data = new Faker<People>()
                   .RuleFor(x => x.Value, GeneratPersonList(count));

            return data.Generate();

        }
        public static List<Data.Models.Response.Person> GeneratPersonList(int count)
        {

            Faker<Data.Models.Response.Person> data = new Faker<Data.Models.Response.Person>()
                   .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                   .RuleFor(x => x.LastName, f => f.Person.LastName)
                   .RuleFor(x => x.Gender, f => f.Person.Gender.ToString())
                   .RuleFor(x => x.UserName, f => f.Person.UserName);

            return data.Generate(count);
        }

    }

}