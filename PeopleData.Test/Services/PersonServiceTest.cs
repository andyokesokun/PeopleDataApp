using Flurl;
using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PeopleData.Business.Services;
using PeopleData.Business.Services.Implementation;
using PeopleData.Data.Models.Response.Enums;
using PeopleData.Data.Settings;
using PeopleData.Test.Mock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PeopleData.Test.Services
{
    public class PersonServiceTest : IDisposable
    {


        private readonly IPersonService _personService;
        private readonly Mock<ILogger<PersonService>> _mocklogger;
        private readonly Mock<IOptionsSnapshot<ODataConfig>> _mockDataConfig;
        private readonly HttpTest _httpTest;

        public PersonServiceTest()
        {

            var x = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true)
               .Build();

            _httpTest = new HttpTest();
            _mocklogger = new Mock<ILogger<PersonService>>(MockBehavior.Loose);
            _mockDataConfig = new Mock<IOptionsSnapshot<ODataConfig>>(MockBehavior.Loose);

            var mockDataConfig = configuration.GetSection("ODataConfig").Get<ODataConfig>();

            _mockDataConfig.Setup(x => x.Value).Returns(mockDataConfig);

            _personService = new PersonService(_mocklogger.Object, _mockDataConfig.Object);

        }

        [Fact]
        public async Task ShouldReturnListOfPerson()
        {

            var mockData = PeopleSample.GetPeopleData(10);
            var odaSettings = _mockDataConfig.Object.Value;
           
            var url = odaSettings.BaseUrl
                         .AppendPathSegment(odaSettings.Key)
                         .AppendPathSegment(odaSettings.PeoplePath);


            _httpTest.RespondWithJson(mockData, 200);
            var list = await _personService.GetPeople();

            _httpTest.ShouldHaveCalled(url);
            Assert.Equal(ResponseCode.Ok, list.Response);
            Assert.Equal(mockData.Value.Count, list.Result.Value.Count );

        }

        [Fact]
        public async Task FilterPerson()
        {

            var mockData = PeopleSample.GetPeopleData(1);
            var person = mockData.Value.FirstOrDefault();
            var odaSettings = _mockDataConfig.Object.Value;

            var url = odaSettings.BaseUrl
                         .AppendPathSegment(odaSettings.Key)
                         .AppendPathSegment(odaSettings.PeoplePath)
                         .SetQueryParam("$Filter", $"FirstName eg '{person.FirstName}'");


            _httpTest.RespondWithJson(mockData, 200);
            var response = await _personService.FilterPeople(Data.Models.Response.Enums.PersonFilter.FIRSTNAME, person.FirstName);
            var actualPerson = response.Result.Value.FirstOrDefault();

            _httpTest.ShouldHaveCalled(url);
            Assert.NotNull(person);
            Assert.Equal(person.FirstName, actualPerson.FirstName);

        }


        [Fact]
        public async Task FindPerson()
        {

            var mockData = PeopleSample.GetPeopleData(1);
            var person = mockData.Value.FirstOrDefault();
            var odaSettings = _mockDataConfig.Object.Value;

            var url = odaSettings.BaseUrl
                         .AppendPathSegment(odaSettings.Key)
                         .AppendPathSegment($"{ odaSettings.PeoplePath}('{person.UserName}')");
       

            _httpTest.RespondWithJson(person, 200);
            var response = await _personService.FindPerson(person.UserName);
            var actualPerson = response.Result;

            _httpTest.ShouldHaveCalled(url);
            Assert.NotNull(person);
            Assert.Equal(person.UserName, actualPerson.UserName);

        }

        [Fact]
        public async Task ShouldUpdatePerson()
        {

            var mockData = PeopleSample.GetPeopleData(1);
            var person = mockData.Value.FirstOrDefault();
            person.FirstName = "John";

            var odaSettings = _mockDataConfig.Object.Value;

            var url = odaSettings.BaseUrl
                         .AppendPathSegment(odaSettings.Key)
                         .AppendPathSegment($"{ odaSettings.PeoplePath}('{person.UserName}')");


            _httpTest.RespondWith(status: 200);
            var response = await _personService.UpdatePerson(person);
            var updated = response.Result;

            _httpTest.ShouldHaveCalled(url);
            Assert.True(updated);
            Assert.Equal(ResponseCode.Ok, response.Response);

        }


        [Fact]
        public async Task ShouldDeletePerson()
        {

            var mockData = PeopleSample.GetPeopleData(1);
            var person = mockData.Value.FirstOrDefault();
            var odaSettings = _mockDataConfig.Object.Value;

            var url = odaSettings.BaseUrl
                         .AppendPathSegment(odaSettings.Key)
                         .AppendPathSegment($"{ odaSettings.PeoplePath}('{person.UserName}')");


            _httpTest.RespondWith(status: 200);
            var response = await _personService.DeletePerson(person.UserName);
            var deleted = response.Result;

            _httpTest.ShouldHaveCalled(url);
            Assert.True(deleted);
            Assert.Equal(ResponseCode.Ok, response.Response);

        }

       
        public void Dispose()
        {
            _httpTest.Dispose();
        }
    }
}
