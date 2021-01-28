using RestSharp;
using System;
using System.Collections.Generic;
using TesonetTask1.Interfaces;

namespace TesonetTask1.Request
{
    public class TokenRequest : IRequest
    {
        private readonly string Username;
        private readonly string Password;
        public TokenRequest(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public string GetResource()
        {
            return "/tokens";
        }

        public Method GetMethod()
        {
            return Method.POST;
        }

        public IDictionary<string, object> GetRequestParameters()
        {
            var parameters = new Dictionary<string, object>
            {
                {"username", Username },
                { "password", Password }
            };
            return parameters;
        }

        public IDictionary<string, string> GetRequestHeaders()
        {
            return null;
        }
    }
}
