/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

namespace e6API
{
    public class ErrorDetails
    {
        /// <summary>
        /// Hold a status code returned from a REST response.
        /// </summary>
        public StatusCode ResponseCode
        {
            get { return responseCode; }
            set
            {
                if(responseCode != value)
                {
                    responseCode = value;
                }
            }
        }
        private StatusCode responseCode;

        /// <summary>
        /// The error message returned by a function or http request
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                if(message != value)
                {
                    message = value;
                }
            }
        }
        private string message;
    }
}
