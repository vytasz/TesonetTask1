using System;
using System.Collections.Generic;
using System.Text;

namespace TesonetTask1.Interfaces
{
    interface IRequest
    {
        /// <summary>
        /// Get API resource
        /// </summary>
        /// <returns></returns>
        string GetResource();
        /// <summary>
        /// Which method (POST, GET, PUT etc) should be used for a REST API call
        /// </summary>
        RestSharp.Method GetMethod();


        /// <summary>
        /// Dictionary of parameters with values when using GET method
        /// </summary>
        IDictionary<string, object> GetRequestParameters();

        /// <summary>
        /// Dictionary of headers for specific request
        /// </summary>
        IDictionary<string, string> GetRequestHeaders();
    }
}
