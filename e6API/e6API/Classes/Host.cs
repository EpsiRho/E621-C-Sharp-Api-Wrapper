/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

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

        private RestClient HostClient;

        public RequestHost()
        {
            HostClient = new RestClient();
        }
        public RequestHost(string UserAgent)
        {
            HostClient = new RestClient();
            this.UserAgent = UserAgent;
        }
        public RequestHost(string UserAgent, string Username, string ApiKey)
        {
            HostClient = new RestClient();
            this.UserAgent = UserAgent;
            this.Username = Username;
            this.ApiKey = ApiKey;
        }

        private RestRequest CreateNewRestRequest(E6RequestType type)
        {
            RestRequest request = null;
            // Set the url based on type
            switch (type)
            {
                case E6RequestType.CreateNote:
                    HostClient.BaseUrl = new Uri("https://e621.net/notes.json?");
                    request = new RestRequest(Method.POST);
                    break;
                case E6RequestType.CreatePool:
                    HostClient.BaseUrl = new Uri("https://e621.net/pools.json?");
                    request = new RestRequest(Method.POST);
                    break;
                case E6RequestType.CreatePost:
                    HostClient.BaseUrl = new Uri("https://e621.net/uploads.json?");
                    request = new RestRequest(Method.POST);
                    break;
                case E6RequestType.CreatePostFlag:
                    HostClient.BaseUrl = new Uri("https://e621.net/post_flags.json?");
                    request = new RestRequest(Method.POST);
                    break;
                case E6RequestType.DeleteNote:
                    //  /notes/<Note_ID>.json
                    request = new RestRequest(Method.DELETE);
                    break;
                case E6RequestType.ListNotes:
                    HostClient.BaseUrl = new Uri("https://e621.net/notes.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.ListPools:
                    HostClient.BaseUrl = new Uri("https://e621.net/pools.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.ListPostFlags:
                    HostClient.BaseUrl = new Uri("https://e621.net/post_flags.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.ListPosts:
                    HostClient.BaseUrl = new Uri("https://e621.net/posts.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.ListTagAliases:
                    HostClient.BaseUrl = new Uri("https://e621.net/tag_aliases.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.ListTags:
                    HostClient.BaseUrl = new Uri("https://e621.net/tags.json?");
                    request = new RestRequest(Method.GET);
                    break;
                case E6RequestType.RevertNote:
                    //	/notes/<Note_ID>/revert.json
                    request = new RestRequest(Method.PUT);
                    break;
                case E6RequestType.RevertPool:
                    //  /pools/<Pool_ID>/revert.json
                    request = new RestRequest(Method.PUT);
                    break;
                case E6RequestType.UpdateNote:
                    //  /notes/<Note_ID>.json
                    request = new RestRequest(Method.PUT);
                    break;
                case E6RequestType.UpdatePool:
                    // 	/pools/<Pool_ID>.json
                    request = new RestRequest(Method.PUT);
                    break;
                case E6RequestType.UpdatePost:
                    //  /posts/<Post_ID>.json
                    request = new RestRequest(Method.PATCH);
                    break;
                case E6RequestType.VoteOnPost:
                    //	/posts/<Post_ID>/votes.json
                    request = new RestRequest(Method.POST);
                    break;
            }

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
            HostClient.UserAgent = UserAgent;

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

            return request;
        }

        /// <summary>
        /// Gets posts from e621.
        /// </summary>
        /// <param name="Tags">The tags to look for when getting posts.</param>
        /// <param name="Limit">The amount of posts to get.<br></br>Must be within 1 - 320.</param>
        /// <param name="Page">The page of posts to grab.</param>
        /// <returns>A list of the Post class.</returns>
        public List<Post> GetPosts(string Tags, int Limit = 75, int Page = 1)
        {
            var request = CreateNewRestRequest(E6RequestType.ListPosts);

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
            var response = HostClient.Execute(request);

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
            PostsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PostsHolder>(response.Content);
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
            var request = CreateNewRestRequest(E6RequestType.ListPosts);

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
            var response = HostClient.Execute(request);

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
            PostsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PostsHolder>(response.Content);
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
        /// Votes on a post
        /// </summary>
        /// <param name="PostId">The post id to vote on</param>
        /// <param name="Vote">1: Vote Up<br></br>-1: Vote Down</param>
        /// <returns>The response from the server in the vote class.<br></br>
        ///          Contains success, total score, and the user's score.</returns>
        public Vote VotePost(int PostId, int Vote)
        {
            var request = CreateNewRestRequest(E6RequestType.VoteOnPost);

            // Check if the post id is not negative
            if(PostId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }

            // set the base uri since it requires the post id
            HostClient.BaseUrl = new Uri($"https://e621.net/posts/{PostId.ToString()}/votes.json");

            // set the score to vote
            if(Vote != 1 && Vote != -1)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Vote variable must be 1 or -1"
                };
                return null;
            }
            request.AddQueryParameter("score", Vote.ToString());

            // Send the request
            var response = HostClient.Execute(request);

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
            Vote json;
            try
            {
                json = JsonConvert.DeserializeObject<Vote>(response.Content);
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
            return json;
        }

        /// <summary>
        /// Search for tags in the database.
        /// </summary>
        /// <param name="Search">The tag to search for.<br></br>End with '*' to search for entries starting with.</param>
        /// <param name="Category">The category of tag to search under</param>
        /// <param name="Sort">The order to return results in</param>
        /// <param name="HideEmpty">Hide tags with no posts</param>
        /// <param name="Limit">The max number of results to return</param>
        /// <param name="Page">The page of results to show</param>
        /// <returns>A list of a class 'Tag' that holds info on each resulting tag</returns>
        public List<Tag> SearchForTags(string Search, TagCategory Category=TagCategory.All, SortOrder Sort=SortOrder.date, bool HideEmpty=false, int Limit=75, int Page=1)
        {
            var request = CreateNewRestRequest(E6RequestType.ListTags);

            // Set The tag search query
            request.AddQueryParameter("search[name_matches]", Search);

            // Set the category if specified
            if (Category != TagCategory.All)
            {
                request.AddQueryParameter("search[category]", ((int)Category).ToString());
            }

            // Set the sort order if specified
            if(Sort != SortOrder.date)
            {
                request.AddQueryParameter("search[order]", ((int)Sort).ToString());
            }

            // Remove empty posts if specified
            if (HideEmpty)
            {
                request.AddQueryParameter("search[hide_empty]", "True");
            }

            // Check if the limit is valid, then set if it is
            if(Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            // Set the page
            if (Page < 1)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("search[hide_empty]", Page.ToString());

            // Send the request
            var response = HostClient.Execute(request);

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
            TagsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<TagsHolder>($"{{tags:{response.Content}}}");
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
            return json.tags;
        }

        /// <summary>
        /// Search for tags in the database.
        /// </summary>
        /// <param name="Search">The tag to search for.<br></br>End with '*' to search for entries starting with.</param>
        /// <param name="PaginationChar">either 'b' or 'a' (before or after the post id).</param>
        /// <param name="PostId">The id of the post to start at.</param>
        /// <param name="Category">The category of tag to search under</param>
        /// <param name="Sort">The order to return results in</param>
        /// <param name="HideEmpty">Hide tags with no posts</param>
        /// <param name="Limit">The max number of results to return</param>
        /// <returns>A list of a class 'Tag' that holds info on each resulting tag</returns>
        public List<Tag> SearchForTags(string Search, char PaginationChar, int PostId, TagCategory Category = TagCategory.All, SortOrder Sort = SortOrder.date, bool HideEmpty = false, int Limit = 75)
        {
            var request = CreateNewRestRequest(E6RequestType.ListTags);

            // Set The tag search query
            request.AddQueryParameter("search[name_matches]", Search);

            // Set the category if specified
            if (Category != TagCategory.All)
            {
                request.AddQueryParameter("search[category]", ((int)Category).ToString());
            }

            // Set the sort order if specified
            if (Sort != SortOrder.date)
            {
                request.AddQueryParameter("search[order]", ((int)Sort).ToString());
            }

            // Remove empty posts if specified
            if (HideEmpty)
            {
                request.AddQueryParameter("search[hide_empty]", "True");
            }

            // Check if the limit is valid, then set if it is
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            // Set the page
            if (PaginationChar != 'a' && PaginationChar != 'b')
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
            var response = HostClient.Execute(request);

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
            TagsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<TagsHolder>($"{{tags:{response.Content}}}");
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
            return json.tags;
        }


    }
}
