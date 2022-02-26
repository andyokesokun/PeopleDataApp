using PeopleData.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeopleData.Business.Services
{
    public interface IPersonService
    {
        Task<People> GetPeople();
    }
}
