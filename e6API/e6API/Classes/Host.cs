/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.2
    Update 0.1 03/17/21@1:07
    Update 0.2 02/11/22@7:15PM (Can you believe it's been almost a year?)
*/

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private async Task<RestRequest> CreateNewRestRequest(E6RequestType type)
        {
            RestRequest request = null;
            // Set the url based on type
            switch (type)
            {
                case E6RequestType.CreateNote:
                    HostClient = new RestClient("https://e621.net/notes.json?");
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.CreatePool:
                    HostClient = new RestClient("https://e621.net/pools.json?");
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.CreatePost:
                    HostClient = new RestClient("https://e621.net/uploads.json?");
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.CreatePostFlag:
                    HostClient = new RestClient("https://e621.net/post_flags.json?");
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.DeleteNote:
                    //  /notes/<Note_ID>.json
                    request = new RestRequest();
                    request.Method = Method.Delete;
                    break;
                case E6RequestType.ListNotes:
                    HostClient = new RestClient("https://e621.net/notes.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.ListPools:
                    HostClient = new RestClient("https://e621.net/pools.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.ListPostFlags:
                    HostClient = new RestClient("https://e621.net/post_flags.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.ListPosts:
                    HostClient = new RestClient("https://e621.net/posts.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.ListTagAliases:
                    HostClient = new RestClient("https://e621.net/tag_aliases.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.ListTags:
                    HostClient = new RestClient("https://e621.net/tags.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    break;
                case E6RequestType.RevertNote:
                    //	/notes/<Note_ID>/revert.json
                    request = new RestRequest();
                    request.Method = Method.Put;
                    break;
                case E6RequestType.RevertPool:
                    //  /pools/<Pool_ID>/revert.json
                    request = new RestRequest();
                    request.Method = Method.Put;
                    break;
                case E6RequestType.UpdateNote:
                    //  /notes/<Note_ID>.json
                    request = new RestRequest();
                    request.Method = Method.Put;
                    break;
                case E6RequestType.UpdatePool:
                    // 	/pools/<Pool_ID>.json
                    request = new RestRequest();
                    request.Method = Method.Put;
                    break;
                case E6RequestType.UpdatePost:
                    //  /posts/<Post_ID>.json
                    request = new RestRequest();
                    request.Method = Method.Patch;
                    break;
                case E6RequestType.VoteOnPost:
                    //	/posts/<Post_ID>/votes.json
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.ListComments:
                    HostClient = new RestClient("https://e621.net/comments.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddQueryParameter("group_by", "comment");
                    request.AddQueryParameter("format", "json");
                    break;
                case E6RequestType.FavoritePost:
                    HostClient = new RestClient("https://e621.net/favorites.json?");
                    request = new RestRequest();
                    request.Method = Method.Post;
                    break;
                case E6RequestType.UnfavoritePost:
                    //	/posts/<Post_ID>/votes.json
                    request = new RestRequest();
                    request.Method = Method.Delete;
                    break;
                case E6RequestType.ListUsers:
                    HostClient = new RestClient("https://e621.net/users.json?");
                    request = new RestRequest();
                    request.Method = Method.Get;
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
            request.AddHeader("User-Agent", UserAgent);

            // Check if login parameters are set
            if(Username == null && ApiKey == null)
            {

            }
            else if (Username != null && ApiKey != null)
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
        public async Task<List<Post>> GetPosts(string Tags, int Limit = 75, int Page = 1)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPosts);

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
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
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
        public async Task<List<Post>> GetPosts(string Tags, char PaginationChar, int PostId, int Limit = 75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPosts);

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
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
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
        public async Task<Vote> VotePost(int PostId, int Vote)
        {
            var request = await CreateNewRestRequest(E6RequestType.VoteOnPost);

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
            HostClient = new RestClient($"https://e621.net/posts/{PostId.ToString()}/votes.json");

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
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
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
        public async Task<List<Tag>> SearchTags(string Search, int Limit = 75, int Page = 1, TagCategory Category=TagCategory.All, TagsSortOrder Sort = TagsSortOrder.date, bool HideEmpty=false)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListTags);

            // Set The tag search query
            request.AddQueryParameter("search[name_matches]", $"{Search}*");

            // Set the category if specified
            if (Category != TagCategory.All)
            {
                request.AddQueryParameter("search[category]", ((int)Category).ToString());
            }

            // Set the sort order if specified
            if(Sort != TagsSortOrder.date)
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
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
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
        /// <param name="PaginationChar">Either 'b' or 'a' (before or after the post id).</param>
        /// <param name="PostId">The id of the post to start at.</param>
        /// <param name="Category">The category of tag to search under</param>
        /// <param name="Sort">The order to return results in</param>
        /// <param name="HideEmpty">Hide tags with no posts</param>
        /// <param name="Limit">The max number of results to return</param>
        /// <returns>A list of a class 'Tag' that holds info on each resulting tag</returns>
        public async Task<List<Tag>> SearchTags(string Search, char PaginationChar, int PostId, int Limit = 75, TagCategory Category = TagCategory.All, TagsSortOrder Sort = TagsSortOrder.date, bool HideEmpty = false)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListTags);

            // Set The tag search query
            request.AddQueryParameter("search[name_matches]", Search);

            // Set the category if specified
            if (Category != TagCategory.All)
            {
                request.AddQueryParameter("search[category]", ((int)Category).ToString());
            }

            // Set the sort order if specified
            if (Sort != TagsSortOrder.date)
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
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
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
        /// Get comments from a post
        /// </summary>
        /// <param name="PostId">The id of the post</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns>A list of Comment classes</returns>
        public async Task<List<Comment>> GetPostComments(int PostId, int Limit=75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListComments);

            // Try to set the post id
            if (PostId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid post id"
                };
                return null;
            }
            request.AddQueryParameter("search[post_id]", PostId.ToString());

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

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            CommentsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<CommentsHolder>($"{{comments:{response.Content}}}");
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
            return json.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> SearchComments(string Body, int Limit=75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListComments);

            // Try to set the post id
            if (Body != null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Body is null"
                };
                return null;
            }
            request.AddQueryParameter("search[body_matches]", Body);

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

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            CommentsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<CommentsHolder>($"{{comments:{response.Content}}}");
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
            return json.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetUserComments(string CreatorName, int Limit=75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListComments);

            // Try to set the post id
            if (CreatorName != null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorName is null"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_name]", CreatorName);

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

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            CommentsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<CommentsHolder>($"{{comments:{response.Content}}}");
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
            return json.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetUserComments(int CreatorId, int Limit=75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListComments);

            // Try to set the post id
            if (CreatorId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorId is invalid"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_id]", CreatorId.ToString());

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

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            CommentsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<CommentsHolder>($"{{comments:{response.Content}}}");
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
            return json.comments;
        }

        /// <summary>
        /// Searches for pools with matching names.
        /// </summary>
        /// <param name="Search">The string to search for.<br></br>Use "*" for wildcard searches.</param>
        /// <param name="Limit">The max ammount of returned pools.</param>
        /// <param name="SortOrder">The order to list the pools.</param>
        /// <returns>A list of Pool classes.</returns>
        public async Task<List<Pool>> SearchPoolNames(string Search, int Limit=75, PoolsSortOrder SortOrder=PoolsSortOrder.updated_at)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPools);

            // Add the search query
            request.AddQueryParameter("search[name_matches]",Search);
            
            // Add the limit query
            if(Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            if(SortOrder != PoolsSortOrder.updated_at)
            {
                request.AddQueryParameter("search[order]", SortOrder.ToString());
            }

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            PoolsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PoolsHolder>($"{{Pools:{response.Content}}}");
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
            return json.Pools;
        }

        /// <summary>
        /// Searches for pools with matching descriptions.
        /// </summary>
        /// <param name="Search">The string to search for.<br></br>Use "*" for wildcard searches.</param>
        /// <param name="Limit">The max ammount of returned pools.</param>
        /// <param name="SortOrder">The order to list the pools.</param>
        /// <returns>A list of Pool classes.</returns>
        public async Task<List<Pool>> SearchPoolDescriptions(string Search, int Limit = 75, PoolsSortOrder SortOrder = PoolsSortOrder.updated_at)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPools);

            // Add the search query
            request.AddQueryParameter("search[description_matches]", Search);

            // Add the limit query
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            if (SortOrder != PoolsSortOrder.updated_at)
            {
                request.AddQueryParameter("search[order]", SortOrder.ToString());
            }

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            PoolsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PoolsHolder>($"{{Pools:{response.Content}}}");
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
            return json.Pools;
        }

        /// <summary>
        /// Searches for pools created by a user.
        /// </summary>
        /// <param name="CreatorName">The creator's name.<br></br>Use "*" for wildcard searches.</param>
        /// <param name="Limit">The max ammount of returned pools.</param>
        /// <param name="SortOrder">The order to list the pools.</param>
        /// <returns>A list of Pool classes.</returns>
        public async Task<List<Pool>> SearchPoolUserPools(string CreatorName, int Limit = 75, PoolsSortOrder SortOrder = PoolsSortOrder.updated_at)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPools);

            // Add the search query
            request.AddQueryParameter("search[creator_name]", CreatorName);

            // Add the limit query
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            if (SortOrder != PoolsSortOrder.updated_at)
            {
                request.AddQueryParameter("search[order]", SortOrder.ToString());
            }

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            PoolsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PoolsHolder>($"{{Pools:{response.Content}}}");
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
            return json.Pools;
        }

        /// <summary>
        /// Searches for pools created by a user.
        /// </summary>
        /// <param name="CreatorName">The creator's name.<br></br>Use "*" for wildcard searches.</param>
        /// <param name="Limit">The max ammount of returned pools.</param>
        /// <param name="SortOrder">The order to list the pools.</param>
        /// <returns>A list of Pool classes.</returns>
        public async Task<List<Pool>> SearchPoolUserPools(int CreatorId, int Limit = 75, PoolsSortOrder SortOrder = PoolsSortOrder.updated_at)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPools);

            // Add the search query
            if(CreatorId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid CreatorId"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_id]", CreatorId.ToString());

            // Add the limit query
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());

            if (SortOrder != PoolsSortOrder.updated_at)
            {
                request.AddQueryParameter("search[order]", SortOrder.ToString());
            }

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            PoolsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PoolsHolder>($"{{Pools:{response.Content}}}");
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
            return json.Pools;
        }

        /// <summary>
        /// Gets info on a pool
        /// </summary>
        /// <param name="PoolId">The id of the pool</param>
        /// <returns>A single Pool class</returns>
        public async Task<Pool> GetPoolInfo(int PoolId)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListPools);

            // Add the search query
            if (PoolId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PoolId is invalid"
                };
                return null;
            }
            request.AddQueryParameter("search[id]", PoolId.ToString());

            request.AddQueryParameter("limit", "1");

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            PoolsHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<PoolsHolder>($"{{Pools:{response.Content}}}");
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

            if(json.Pools.Count <= 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.NotFound,
                    Message = $"Couldn't find pool {PoolId}"
                };
                return null;
            }

            // Return the obtained posts
            return json.Pools[0];
        }

        /// <summary>
        /// Favorite a post. Must have username and apikey set and valid.
        /// </summary>
        /// <param name="PostId">The id of the post to be favorited</param>
        /// <returns>True on success, false if there is an error/post already favorited.</returns>
        public async Task<bool> FavoritePost(int PostId)
        {
            var request = await CreateNewRestRequest(E6RequestType.FavoritePost);

            // If the username and api key are not set, you can't favorite
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to favorite a post."
                };
                return false;
            }

            // Check if post id is not negative
            if(PostId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId is invalid."
                };
                return false;
            }
            request.AddQueryParameter("post_id", PostId.ToString());

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            // Check if the post returned already favorited
            if (response.Content.Contains("You have already favorited this post"))
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }

            // Check for http errors
            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unfavorites a post.
        /// </summary>
        /// <param name="PostId">The id of the post to unfavorite</param>
        /// <returns>True on success, false on failure.</returns>
        public async Task<bool> UnfavoritePost(int PostId)
        {
            var request = await CreateNewRestRequest(E6RequestType.UnfavoritePost);

            // If the username and api key are not set, you can't favorite
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to favorite a post."
                };
                return false;
            }

            // Check if post id is not negative
            if (PostId < 0)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId is invalid."
                };
                return false;
            }
            request.AddQueryParameter("post_id", PostId.ToString());


            HostClient = new RestClient("https://e621.net/favorites/" + PostId.ToString() + ".json");
            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            // Check for http errors
            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }

            return true;
        }

        /// <summary>
        /// Searches for users. Many fields in the User class wont be set unless it's for the currently logged in user.
        /// </summary>
        /// <param name="Search">The string to search for. Use "*" for wildcard searches</param>
        /// <param name="Limit">The max ammount of users to return</param>
        /// <returns>A list of users</returns>
        public async Task<List<User>> SearchUsers(string Search, int Limit=75)
        {
            var request = await CreateNewRestRequest(E6RequestType.ListUsers);

            request.AddQueryParameter("search[name_matches]", Search);

            if(Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }

            // Send the request
            var response = await HostClient.ExecuteAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails()
                {
                    ResponseCode = (StatusCode)(int)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }

            // Try to deserialize the json into a list of Posts
            UsersHolder json;
            try
            {
                json = JsonConvert.DeserializeObject<UsersHolder>($"{{Users:{response.Content}}}");
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
            return json.Users;
        }


    }
}
