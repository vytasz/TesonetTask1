using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using TesonetTask1.Interfaces;

namespace TesonetTask1.Modules
{
    class Connection<TRequest, TResponse> : IDisposable
        where TRequest : IRequest
    {
        Logger logger = Logger.GetInstance();

        private RestClient RestClient { get; }
        private RestRequest RestRequest { get; }
        private IRestResponse RestResponse { get; set; }
        /// <summary>
        /// Establish connection to API
        /// </summary>
        /// <param name="request">The type of request made</param>
        /// <param name="baseAddress">The API address</param>
        public Connection(TRequest request, string baseAddress)
        {
            var resource = request.GetResource();
            if (baseAddress.EndsWith("/") && resource.StartsWith("/"))
            {
                baseAddress = baseAddress.TrimEnd('/');
            }
            else if (!baseAddress.EndsWith("/") && !resource.StartsWith("/"))
            {
                baseAddress += "/";
            }
            RestClient = new RestClient(baseAddress);
            RestRequest = new RestRequest(resource, request.GetMethod());

            var requestParameters = request.GetRequestParameters();
            if (requestParameters != null)
            {
                foreach (var parameter in requestParameters)
                {
                    RestRequest.AddParameter(parameter.Key, parameter.Value);
                }
            }

            var requestHeaders = request.GetRequestHeaders();
            if (requestHeaders != null)
            {
                foreach (var header in requestHeaders)
                {
                    RestRequest.AddHeader(header.Key, header.Value);
                }
            }
         
        }

        public TResponse Execute()
        {
            try
            {
                RestResponse = RestClient.Execute(RestRequest);
                if (RestResponse.ResponseStatus != ResponseStatus.Completed)
                { 
                   throw new Exception(RestResponse.ErrorMessage, RestResponse.ErrorException);
                }
                if (RestResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    logger.Unauthorized(RestResponse.ResponseUri.ToString());
                    return default;
                }
                var result = JsonConvert.DeserializeObject<TResponse>(RestResponse.Content);

                return result;
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw exception;
            }

        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
