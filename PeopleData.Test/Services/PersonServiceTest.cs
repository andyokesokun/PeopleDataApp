using Flurl.Http.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PeopleData.Business.Services;
using PeopleData.Business.Services.Implementation;
using PeopleData.Data.Settings;
using PeopleData.Test.Mock;
using System;
using System.Collections.Generic;
using System.IO;
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

            _httpTest.RespondWithJson(mockData, 200);
            var list = await _personService.GetPeople();

            Assert.Equal(mockData.Value.Count, list.Value.Count);

        }

        public void Dispose()
        {
            _httpTest.Dispose();
        }
    }
}
