using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TesonetTask1.Interfaces;

namespace TesonetTask1.Request
{
    public class ServerRequest : IRequest
    {
        private readonly string Token;
        public ServerRequest(string Token)
        {
            this.Token = Token;
        }
        public string GetResource()
        {
            return "/servers";
        }

        public Method GetMethod()
        {
            return Method.GET;
        }

        public IDictionary<string, object> GetRequestParameters()
        {
            return null;
        }

        public IDictionary<string, string> GetRequestHeaders()
        {
            var headers = new Dictionary<string, string>
            {
                {"Authorization", string.Format("Bearer {0}", Token)},
            };
            return headers;
        }
    }
}
