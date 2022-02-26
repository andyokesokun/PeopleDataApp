using PeopleData.Data.Models.Response;
using PeopleData.Data.Models.Response.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeopleData.Business.Services
{
    public interface IPersonService
    {
        Task<APIResponse<People>> GetPeople();
        Task<APIResponse<People>> FilterPeople(PersonFilter personFilter, string filterValue );
    }
}
