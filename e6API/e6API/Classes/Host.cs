/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

using e6API.Enums;
using e6API.JSON_Classes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace e6API
{
    public class RequestHost
    {
        /// <summary>
        /// Your User Agent to be sent with the request.<br></br>
        /// This is required for a request to work.
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set
            {
                if (userAgent != value)
                {
                    userAgent = value;
                }
            }
        }
        private string userAgent;

        /// <summary>
        /// The username to login with.<br></br>
        /// Must also set the ApiKey.
        /// </summary>
        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                }
            }
        }
        private string username;

        /// <summary>
        /// The api key to login with.<br></br>
        /// Must also set the Username.
        /// </summary>
        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                if(apiKey != value)
                {
                    apiKey = value;
                }
            }
        }
        private string apiKey;

        /// <summary>
        /// The details of the last error.
        /// </summary>
        public ErrorDetails LastError
        {
            get { return lastError; }
            set
            {
                if(lastError != value)
                {
                    lastError = value;
                }
            }
        }
        private ErrorDetails lastError;

        /// <summary>
        /// Gets posts from e621.
        /// </summary>
        /// <param name="Tags">The tags to look for when getting posts.</param>
        /// <param name="Limit">The amount of posts to get.<br></br>Must be within 1 - 320.</param>
        /// <param name="Page">The page of posts to grab.</param>
        /// <returns>A list of the Post class.</returns>
        public List<Post> GetPosts(string Tags, int Limit = 75, int Page = 1)
        {
            // Create a new Rest Client handler
            var client = new RestClient();

            // Set the REST method to GET
            var request = new RestRequest(RestSharp.Method.GET);

            // Set the base url to e621's post.json endpoint
            client.BaseUrl = new Uri("https://e621.net/posts.json?");

            // Try to set the UserAgent
            if(UserAgent == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: UserAgent"
                };
                return null;
            }
            client.UserAgent = UserAgent;

            // Check if login parameters are set
            if (Username != null && ApiKey != null)
            {
                request.AddQueryParameter("login", Username);
                request.AddQueryParameter("api_key", ApiKey);
            }
            else if(Username != null && ApiKey == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Missing ApiKey"
                };
                return null;
            }
            else if (Username == null && ApiKey != null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Missing Username"
                };
                return null;
            }

            // Add any tags into the request
            if (Tags == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("tags", Tags);

            // Try to set the limit
            if(Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            // Set the page
            if (Page < 1 || Page > 750)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be a value within 1 - 750"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());

            // Send the request
            var response = client.Execute(request);

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            ListPostsJson json;
            try
            {
                json = JsonConvert.DeserializeObject<ListPostsJson>(response.Content);
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = $"Couldn't convert json: {e.Message}"
                };
                return null;
            }

            // Return the obtained posts
            return json.posts;
        }

        /// <summary>
        /// Gets posts from e621 using pagination.
        /// </summary>
        /// <param name="Tags">The tags to look for when getting posts.</param>
        /// <param name="Limit">The amount of posts to get.<br></br>Must be within 1 - 320.</param>
        /// <param name="PaginationChar">either 'b' or 'a' (before or after the post id).</param>
        /// <param name="PostId">The id of the post to start at.</param>
        /// <returns>A list of the Post class</returns>
        public List<Post> GetPosts(string Tags, char PaginationChar, int PostId, int Limit = 75)
        {
            // Create a new Rest Client handler
            var client = new RestClient();

            // Set the REST method to GET
            var request = new RestRequest(RestSharp.Method.GET);

            // Set the base url to e621's post.json endpoint
            client.BaseUrl = new Uri("https://e621.net/posts.json?");

            // Try to set the UserAgent
            if (UserAgent == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: UserAgent"
                };
                return null;
            }
            client.UserAgent = UserAgent;

            // Check if login parameters are set
            if (Username != null && ApiKey != null)
            {
                request.AddQueryParameter("login", Username);
                request.AddQueryParameter("api_key", ApiKey);
            }
            else if (Username != null && ApiKey == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Missing ApiKey"
                };
                return null;
            }
            else if (Username == null && ApiKey != null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Missing Username"
                };
                return null;
            }

            // Add any tags into the request
            if (Tags == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("tags", Tags);

            // Try to set the limit
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            // Set the page
            if(PaginationChar != 'a' && PaginationChar != 'b')
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid PaginationChar"
                };
                return null;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid post id"
                };
                return null;
            }
            request.AddQueryParameter("page", $"{PaginationChar}{PostId}");

            // Send the request
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            ListPostsJson json;
            try
            {
                json = JsonConvert.DeserializeObject<ListPostsJson>(response.Content);
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = $"Couldn't convert json: {e.Message}"
                };
                return null;
            }

            // Return the obtained posts
            return json.posts;
        }


    }
}
