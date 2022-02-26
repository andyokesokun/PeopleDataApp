using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleData.Business.Common.Extensions;
using PeopleData.Data.Models.Response;
using PeopleData.Data.Models.Response.Enums;
using PeopleData.Data.Settings;
using System;
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

        public async Task<APIResponse<People>> FilterPeople(PersonFilter personFilter, string filterValue)
        {
            try
            {
                var url = _odaSettings.BaseUrl
                         .AppendPathSegment(_odaSettings.Key)
                         .AppendPathSegment(_odaSettings.PeoplePath)
                         .SetQueryParam("$Filter", $"{personFilter.ToDescription()} eg '{filterValue}'");

               var result= await url.GetJsonAsync<People>();

                if (result !=null) {

                    return APIResponse<People>.Success(result, ResponseCode.Ok.ToDescription());
                }

                return APIResponse<People>.Failed( errorMessage: ResponseCode.Failed.ToDescription());

            } catch (Exception e) {
                return APIResponse<People>.Failed( e.Message, ResponseCode.Exeception);
            }


         
        }

        public async Task<APIResponse<People>> GetPeople()
        {

            try
            {
                var url = _odaSettings.BaseUrl
                       .AppendPathSegment(_odaSettings.Key)
                       .AppendPathSegment(_odaSettings.PeoplePath);

                var result = await url.GetJsonAsync<People>();

                if (result != null)
                {

                    return APIResponse<People>.Success(result, ResponseCode.Ok.ToDescription());
                }

                return APIResponse<People>.Failed(errorMessage: ResponseCode.Failed.ToDescription());

            }
            catch (Exception e)
            {
                return APIResponse<People>.Failed(e.Message, ResponseCode.Exeception);
            }

        }


    }
}
