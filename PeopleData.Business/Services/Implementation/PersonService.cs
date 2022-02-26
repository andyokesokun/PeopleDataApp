using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PeopleData.Business.Common.Extensions;
using PeopleData.Data.Models.Response;
using PeopleData.Data.Models.Response.Enums;
using PeopleData.Data.Settings;
using System;
using System.Text.Json;
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

        public async Task<APIResponse<bool>> DeletePerson(string userName)
        {
            var url = _odaSettings.BaseUrl
                    .AppendPathSegment(_odaSettings.Key)
                    .AppendPathSegment($"{_odaSettings.PeoplePath}('{userName}')");

            return await HandleModifyResponse(url, MethodType.DELETE);
        }

        public async Task<APIResponse<People>> FilterPeople(PersonFilter personFilter, string filterValue)
        {
           
                var url = _odaSettings.BaseUrl
                         .AppendPathSegment(_odaSettings.Key)
                         .AppendPathSegment(_odaSettings.PeoplePath)
                         .SetQueryParam("$Filter", $"{personFilter.ToDescription()} eg '{filterValue}'");


            return await HandleGetResponse<People>(url);


        }

        public async Task<APIResponse<Person>> FindPerson(string userName)
        {
    
                var url = _odaSettings.BaseUrl
                       .AppendPathSegment(_odaSettings.Key)
                       .AppendPathSegment( $"{_odaSettings.PeoplePath}('{userName}')" );

            return await HandleGetResponse<Person>(url);


        }

        public async Task<APIResponse<People>> GetPeople()
        {

   
                var url = _odaSettings.BaseUrl
                       .AppendPathSegment(_odaSettings.Key)
                       .AppendPathSegment(_odaSettings.PeoplePath);

            return await HandleGetResponse<People>(url);

        }

        public async Task<APIResponse<bool>> UpdatePerson(Person person)
        {
            var url = _odaSettings.BaseUrl
                     .AppendPathSegment(_odaSettings.Key)
                     .AppendPathSegment($"{_odaSettings.PeoplePath}('{person.UserName}')");

            return await HandleModifyResponse(url, MethodType.PATCH, person);
        }

        private  async Task<APIResponse<T>>  HandleGetResponse<T>(Url url) {

            try
            {
            
                 var result = await url.GetJsonAsync<T>();

                if (result != null)
                {

                    return APIResponse<T>.Success(result, ResponseCode.Ok.ToDescription());
                }

                return APIResponse<T>.Failed(errorMessage: ResponseCode.Failed.ToDescription());

            }
            catch (Exception e)
            {
                return APIResponse<T>.Failed(e.Message, ResponseCode.Exeception);
            }

        }


        private async Task<APIResponse<bool>> HandleModifyResponse(Url url, MethodType methodType, Object objectData = null)
        {

            try
            {
    
                switch (methodType) {
                    case MethodType.PATCH:
                        await url.PatchJsonAsync(objectData);
                        break;
                    case MethodType.DELETE:
                        await url.DeleteAsync();
                        break;
                    default:
                        break;
                        

                }

          
              return APIResponse<bool>.Success(true, ResponseCode.Ok.ToDescription());


            }
            catch (Exception e)
            {
                return APIResponse<bool>.Success(true, e.Message);
            }

        }



    }
}
