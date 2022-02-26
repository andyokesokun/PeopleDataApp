using PeopleData.Data.Models.Response.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleData.Data.Models.Response
{
    public class APIResponse
    {
        public APIResponse()
        {
            Response = ResponseCode.Ok;
        }

        public ResponseCode Response { get; set; }
        public string Message { get; set; }

    }


    public class APIResponse<T> : APIResponse {

        public T Result { get; set; }

        public static APIResponse<T> Success(T result, string message) => new APIResponse<T> { Response = ResponseCode.Ok, Result = result, Message = message };

        public static APIResponse<T> Failed(string errorMessage,ResponseCode response = ResponseCode.Failed) =>   new APIResponse<T> { Response = response, Message = errorMessage };

        public static APIResponse<T> Failed(T result, string message) =>   new APIResponse<T> { Response = ResponseCode.Failed, Result = result, Message = message };


        public bool IsResultEmpty() => (Result == null);

    }
}
