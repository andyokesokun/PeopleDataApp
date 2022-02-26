using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using PeopleData.Data.Models.Response;
using PeopleData.Data.Settings;
using System.Threading.Tasks;

namespace PeopleData.Business.Services.Implementation
{
    public class PersonService : IPersonService
    {
        private ILogger<IPersonService> _logger;
        private ODataConfig _odaSettings;

        public PersonService(ILogger<IPersonService> logger, IOptionsSnapshot<ODataConfig> odaSettings)
        {
            _logger = logger;
            _odaSettings = odaSettings.Value;

        }
        public async Task<People> GetPeople()
        {
            var url = _odaSettings.BaseUrl
                       .AppendPathSegment(_odaSettings.Key)
                       .AppendPathSegment(_odaSettings.PeoplePath);

           return await url.GetJsonAsync<People>();
                        
        }


    }
}
