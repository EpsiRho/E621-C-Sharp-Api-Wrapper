/*
    E621 API Wrapper
    Written by Epsi Rho
    Recovered from the published 0.7.8 assembly and merged with the V2 migration.
*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace e6API
{
    public class RequestHost
    {
        private string userAgent;

        private string username;

        private string apiKey;

        private ErrorDetails lastError;

        private RestClient HostClient;

        // Allows the regression suite to supply an in-memory HTTP transport.
        internal Func<string, RestClient> RestClientFactory { get; set; } = baseUrl => new RestClient(baseUrl);

        /// <summary>
        /// Your User Agent to be sent with the request.<br></br>
        /// This is required for a request to work.
        /// </summary>
        public string UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                if (userAgent != value)
                {
                    userAgent = value;
                }
            }
        }

        /// <summary>
        /// The username to login with.<br></br>
        /// Must also set the ApiKey.
        /// </summary>
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    username = value;
                }
            }
        }

        /// <summary>
        /// The api key to login with.<br></br>
        /// Must also set the Username.
        /// </summary>
        public string ApiKey
        {
            get
            {
                return apiKey;
            }
            set
            {
                if (apiKey != value)
                {
                    apiKey = value;
                }
            }
        }

        /// <summary>
        /// The details of the last error.
        /// </summary>
        public ErrorDetails LastError
        {
            get
            {
                return lastError;
            }
            set
            {
                if (lastError != value)
                {
                    lastError = value;
                }
            }
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

        private Task<RestRequest> CreateNewRestRequest(E6RequestType type)
        {
            RestRequest restRequest = null;
            switch (type)
            {
            case E6RequestType.CreateComment:
                HostClient = RestClientFactory("https://e621.net/comments.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.CreateNote:
                HostClient = RestClientFactory("https://e621.net/notes.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.CreatePool:
                HostClient = RestClientFactory("https://e621.net/pools.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.CreatePost:
                HostClient = RestClientFactory("https://e621.net/uploads.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.CreatePostFlag:
                HostClient = RestClientFactory("https://e621.net/post_flags.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.DeleteNote:
                restRequest = new RestRequest();
                restRequest.Method = Method.Delete;
                break;
            case E6RequestType.ListNotes:
                HostClient = RestClientFactory("https://e621.net/notes.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListPools:
                HostClient = RestClientFactory("https://e621.net/pools.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListPostFlags:
                HostClient = RestClientFactory("https://e621.net/post_flags.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListPosts:
                HostClient = RestClientFactory("https://e621.net/posts.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                restRequest.AddQueryParameter("v2", "true");
                break;
            case E6RequestType.ListTagAliases:
                HostClient = RestClientFactory("https://e621.net/tag_aliases.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListTags:
                HostClient = RestClientFactory("https://e621.net/tags.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.RevertNote:
                restRequest = new RestRequest();
                restRequest.Method = Method.Put;
                break;
            case E6RequestType.RevertPool:
                restRequest = new RestRequest();
                restRequest.Method = Method.Put;
                break;
            case E6RequestType.UpdateNote:
                restRequest = new RestRequest();
                restRequest.Method = Method.Put;
                break;
            case E6RequestType.UpdatePool:
                restRequest = new RestRequest();
                restRequest.Method = Method.Put;
                break;
            case E6RequestType.UpdatePost:
                restRequest = new RestRequest();
                restRequest.Method = Method.Patch;
                break;
            case E6RequestType.VoteOnPost:
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.ListComments:
                HostClient = RestClientFactory("https://e621.net/comments.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                restRequest.AddQueryParameter("group_by", "comment");
                restRequest.AddQueryParameter("format", "json");
                break;
            case E6RequestType.FavoritePost:
                HostClient = RestClientFactory("https://e621.net/favorites.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.UnfavoritePost:
                restRequest = new RestRequest();
                restRequest.Method = Method.Delete;
                break;
            case E6RequestType.ListUsers:
                HostClient = RestClientFactory("https://e621.net/users.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.SearchWiki:
                HostClient = RestClientFactory("https://e621.net/wiki_pages.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListSets:
                HostClient = RestClientFactory("https://e621.net/post_sets");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListBlips:
                HostClient = RestClientFactory("https://e621.net/blips");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.CreateBlip:
                HostClient = RestClientFactory("https://e621.net/blips");
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            }
            if (restRequest == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Unsupported request type"
                };
                return Task.FromResult<RestRequest>(null);
            }
            if (UserAgent == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: UserAgent"
                };
                return Task.FromResult<RestRequest>(null);
            }
            restRequest.AddHeader("User-Agent", UserAgent);
            restRequest.AddHeader("Accept", "application/json");
            if (Username != null || ApiKey != null)
            {
                if (Username != null && ApiKey != null)
                {
                    restRequest.AddQueryParameter("login", Username);
                    restRequest.AddQueryParameter("api_key", ApiKey);
                }
                else
                {
                    if (Username != null && ApiKey == null)
                    {
                        LastError = new ErrorDetails
                        {
                            ResponseCode = StatusCode.MissingOrInvalidParameter,
                            Message = "Invalid Parameter: Missing ApiKey"
                        };
                        return Task.FromResult<RestRequest>(null);
                    }
                    if (Username == null && ApiKey != null)
                    {
                        LastError = new ErrorDetails
                        {
                            ResponseCode = StatusCode.MissingOrInvalidParameter,
                            Message = "Invalid Parameter: Missing Username"
                        };
                        return Task.FromResult<RestRequest>(null);
                    }
                }
            }
            return Task.FromResult(restRequest);
        }

        public async Task<List<Tag>> GetAutocompletetags(string Tag)
        {
            HostClient = RestClientFactory("https://e621.net/tags/autocomplete.json?");
            RestRequest restRequest = new RestRequest();
            restRequest.Method = Method.Get;
            restRequest.AddHeader("User-Agent", UserAgent);
            restRequest.AddQueryParameter("search[name_matches]", Tag);
            restRequest.AddQueryParameter("expiry", "7");
            RestResponse restResponse = await HostClient.ExecuteAsync(restRequest);
            if (restResponse == null)
            {
                return null;
            }
            List<Tag> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<Tag>>(restResponse.Content ?? "");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return result;
        }

        public async Task<bool> UpdateUserBlacklist(string UserId, string Username, string ApiKey, string Blacklist)
        {
            HostClient = RestClientFactory("https://e621.net/users/" + UserId);
            RestRequest restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AlwaysMultipartFormData = true;
            restRequest.AddHeader("User-Agent", UserAgent);
            restRequest.AddParameter("login", Username);
            restRequest.AddParameter("api_key", ApiKey);
            restRequest.AddParameter("user[blacklisted_tags]", Blacklist);
            restRequest.AddParameter("_method", "patch");
            if ((await HostClient.ExecuteAsync(restRequest)).StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> TryAuthenticate(string Username, string ApiKey)
        {
            HostClient = RestClientFactory("https://e621.net/posts.json?");
            RestRequest restRequest = new RestRequest();
            restRequest.Method = Method.Get;
            restRequest.AddHeader("User-Agent", UserAgent);
            restRequest.AddQueryParameter("login", Username);
            restRequest.AddQueryParameter("api_key", ApiKey);
            restRequest.AddQueryParameter("limit", "1");
            if ((await HostClient.ExecuteAsync(restRequest)).Content.Contains("AuthenticationFailure"))
            {
                return false;
            }
            return true;
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPosts);
            if (request == null)
            {
                return null;
            }
            if (Tags == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("tags", Tags);
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            if (Page < 1 || Page > 750)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be a value within 1 - 750"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<Post> posts;
            try
            {
                posts = JsonConvert.DeserializeObject<List<Post>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return posts;
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPosts);
            if (request == null)
            {
                return null;
            }
            if (Tags == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("tags", Tags);
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            if (PaginationChar != 'a' && PaginationChar != 'b')
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid PaginationChar"
                };
                return null;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid post id"
                };
                return null;
            }
            request.AddQueryParameter("page", $"{PaginationChar}{PostId}");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<Post> posts;
            try
            {
                posts = JsonConvert.DeserializeObject<List<Post>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return posts;
        }

        /// <summary>
        /// Change the user's vote on a post
        /// </summary>
        /// <param name="PostId">The post id to vote on</param>
        /// <param name="Vote">1: Vote Up<br></br>-1: Vote Down</param>
        /// <param name="Toggle">Set to true to have this score replace the old score. Repeat votes will not remove the vote.</param>
        /// <returns>The response from the server in the vote class.<br></br>
        ///          Contains success, total score, and the user's score.</returns>
        public async Task<Vote> SetVote(int PostId, int Vote, bool Toggle = false)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.VoteOnPost);
            if (request == null)
            {
                return null;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            HostClient = RestClientFactory("https://e621.net/posts/" + PostId + "/votes.json");
            if (Vote != 1 && Vote != -1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Vote variable must be 1 or -1"
                };
                return null;
            }
            request.AddQueryParameter("score", Vote.ToString());
            request.AddQueryParameter("no_unvote", Toggle ? "true" : "false");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            Vote result;
            try
            {
                result = JsonConvert.DeserializeObject<Vote>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return result;
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
        public async Task<List<Tag>> GetTags(string Search, int Limit = 75, int Page = 1, TagCategory Category = TagCategory.All, TagsSortOrder Sort = TagsSortOrder.date, bool HideEmpty = false)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListTags);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[name_matches]", Search ?? "");
            if (Category != TagCategory.All)
            {
                int num = (int)Category;
                request.AddQueryParameter("search[category]", num.ToString());
            }
            if (Sort != 0)
            {
                request.AddQueryParameter("search[order]", Sort.ToString());
            }
            if (HideEmpty)
            {
                request.AddQueryParameter("search[hide_empty]", "True");
            }
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            TagsHolder tagsHolder;
            try
            {
                tagsHolder = JsonConvert.DeserializeObject<TagsHolder>("{tags:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return tagsHolder.tags;
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
        public async Task<List<Tag>> GetTags(string Search, char PaginationChar, int PostId, int Limit = 75, TagCategory Category = TagCategory.All, TagsSortOrder Sort = TagsSortOrder.date, bool HideEmpty = false)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListTags);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[name_matches]", Search);
            if (Category != TagCategory.All)
            {
                int num = (int)Category;
                request.AddQueryParameter("search[category]", num.ToString());
            }
            if (Sort != 0)
            {
                request.AddQueryParameter("search[order]", Sort.ToString());
            }
            if (HideEmpty)
            {
                request.AddQueryParameter("search[hide_empty]", "True");
            }
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            if (PaginationChar != 'a' && PaginationChar != 'b')
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid PaginationChar"
                };
                return null;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid post id"
                };
                return null;
            }
            request.AddQueryParameter("page", $"{PaginationChar}{PostId}");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            TagsHolder tagsHolder;
            try
            {
                tagsHolder = JsonConvert.DeserializeObject<TagsHolder>("{tags:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return tagsHolder.tags;
        }

        /// <summary>
        /// Get comments from a post
        /// </summary>
        /// <param name="PostId">The id of the post</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns>A list of Comment classes</returns>
        public async Task<List<Comment>> GetPostComments(int PostId, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListComments);
            if (request == null)
            {
                return null;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid post id"
                };
                return null;
            }
            request.AddQueryParameter("search[post_id]", PostId.ToString());
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            CommentsHolder commentsHolder;
            try
            {
                commentsHolder = JsonConvert.DeserializeObject<CommentsHolder>("{comments:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return commentsHolder.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> SearchComments(string Body, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListComments);
            if (request == null)
            {
                return null;
            }
            if (Body == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Body is null"
                };
                return null;
            }
            request.AddQueryParameter("search[body_matches]", Body);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            CommentsHolder commentsHolder;
            try
            {
                commentsHolder = JsonConvert.DeserializeObject<CommentsHolder>("{comments:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return commentsHolder.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetUserComments(string CreatorName, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListComments);
            if (request == null)
            {
                return null;
            }
            if (CreatorName == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorName is null"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_name]", CreatorName);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            CommentsHolder commentsHolder;
            try
            {
                commentsHolder = JsonConvert.DeserializeObject<CommentsHolder>("{comments:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return commentsHolder.comments;
        }

        /// <summary>
        /// Search comments for a string
        /// </summary>
        /// <param name="Body">A string to search comment bodies for. Use "*" for wildcard search</param>
        /// <param name="Limit">The max number of comments to get</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetUserComments(int CreatorId, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListComments);
            if (request == null)
            {
                return null;
            }
            if (CreatorId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorId is invalid"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_id]", CreatorId.ToString());
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            CommentsHolder commentsHolder;
            try
            {
                commentsHolder = JsonConvert.DeserializeObject<CommentsHolder>("{comments:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return commentsHolder.comments;
        }

        /// <summary>
        /// Searches for pools with matching names.
        /// </summary>
        /// <param name="Search">The string to search for.<br></br>Use "*" for wildcard searches.</param>
        /// <param name="Limit">The max ammount of returned pools.</param>
        /// <param name="SortOrder">The order to list the pools.</param>
        /// <param name="Page">The page of results to return.</param>
        /// <returns>A list of Pool classes.</returns>
        public async Task<List<Pool>> SearchPoolNames(string Search, int Limit = 75, int Page = 1, PoolsSortOrder SortOrder = PoolsSortOrder.updated_at)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPools);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[name_matches]", Search);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            PoolsHolder poolsHolder;
            try
            {
                poolsHolder = JsonConvert.DeserializeObject<PoolsHolder>("{Pools:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return poolsHolder.Pools;
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPools);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[description_matches]", Search);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
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
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            PoolsHolder poolsHolder;
            try
            {
                poolsHolder = JsonConvert.DeserializeObject<PoolsHolder>("{Pools:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return poolsHolder.Pools;
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPools);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[creator_name]", CreatorName);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
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
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            PoolsHolder poolsHolder;
            try
            {
                poolsHolder = JsonConvert.DeserializeObject<PoolsHolder>("{Pools:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return poolsHolder.Pools;
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPools);
            if (request == null)
            {
                return null;
            }
            if (CreatorId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid CreatorId"
                };
                return null;
            }
            request.AddQueryParameter("search[creator_id]", CreatorId.ToString());
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
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
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            PoolsHolder poolsHolder;
            try
            {
                poolsHolder = JsonConvert.DeserializeObject<PoolsHolder>("{Pools:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return poolsHolder.Pools;
        }

        /// <summary>
        /// Gets info on a pool
        /// </summary>
        /// <param name="PoolId">The id of the pool</param>
        /// <returns>A single Pool class</returns>
        public async Task<Pool> GetPoolInfo(int PoolId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPools);
            if (request == null)
            {
                return null;
            }
            if (PoolId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PoolId is invalid"
                };
                return null;
            }
            request.AddQueryParameter("search[id]", PoolId.ToString());
            request.AddQueryParameter("limit", "1");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            PoolsHolder poolsHolder;
            try
            {
                poolsHolder = JsonConvert.DeserializeObject<PoolsHolder>("{Pools:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            if (poolsHolder.Pools.Count <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.NotFound,
                    Message = $"Couldn't find pool {PoolId}"
                };
                return null;
            }
            return poolsHolder.Pools[0];
        }

        /// <summary>
        /// Favorite a post. Must have username and apikey set and valid.
        /// </summary>
        /// <param name="PostId">The id of the post to be favorited</param>
        /// <returns>True on success, false if there is an error/post already favorited.</returns>
        public async Task<bool> FavoritePost(int PostId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.FavoritePost);
            if (request == null)
            {
                return false;
            }
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to favorite a post."
                };
                return false;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId is invalid."
                };
                return false;
            }
            request.AddQueryParameter("post_id", PostId.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.Content.Contains("You have already favorited this post"))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return false;
            }
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
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
            RestRequest request = await CreateNewRestRequest(E6RequestType.UnfavoritePost);
            if (request == null)
            {
                return false;
            }
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to favorite a post."
                };
                return false;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId is invalid."
                };
                return false;
            }
            request.AddQueryParameter("post_id", PostId.ToString());
            HostClient = RestClientFactory("https://e621.net/favorites/" + PostId + ".json");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
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
        public async Task<List<User>> SearchUsers(string Search, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListUsers);
            if (request == null)
            {
                return null;
            }
            request.AddQueryParameter("search[name_matches]", Search);
            if (Limit < 1 || Limit > 1000)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be between 1 - 1000"
                };
                return null;
            }
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            UsersHolder usersHolder;
            try
            {
                usersHolder = JsonConvert.DeserializeObject<UsersHolder>("{Users:" + restResponse.Content + "}");
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return usersHolder.Users;
        }

        public async Task<List<WikiSearch>> SearchWikiEntries(string Search, int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.SearchWiki);
            if (request == null)
            {
                return null;
            }
            if (Search == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("search[title]", Search + "*");
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            request.AddQueryParameter("search[order]", "post_count");
            request.AddQueryParameter("search[hide_deleted]", "Yes");
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<WikiSearch> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<WikiSearch>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return result;
        }

        public async Task<List<Set>> SearchSets(string Search, int Limit = 75, int Page = 1)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListSets);
            if (request == null)
            {
                return null;
            }
            if (Search == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            request.AddQueryParameter("search[name]", Search + "*");
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId variable is invalid"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<Set> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<Set>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return result;
        }

        public async Task<List<Blip>> ListBlips(int Limit = 75)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListBlips);
            if (request == null)
            {
                return null;
            }
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<Blip> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<Blip>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return result;
        }

        public async Task CreateBlips(string body, string response_to = "")
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreateBlip);
            if (request == null)
            {
                return;
            }
            if (body == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: body variable is null"
                };
                return;
            }
            request.AddQueryParameter("blip[body]", body);
            if (response_to != "")
            {
                request.AddQueryParameter("blip[response_to]", response_to);
            }
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to create a blip."
                };
                return;
            }
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
            }
        }

        public async Task<bool> CommentOnPost(int PostId, string body)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreateComment);
            if (request == null)
            {
                return false;
            }
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to comment on a post."
                };
                return false;
            }
            if (PostId < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId is invalid."
                };
                return false;
            }
            request.AddQueryParameter("comment[post_id]", PostId.ToString());
            request.AddQueryParameter("comment[body]", body);
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return false;
            }
            return true;
        }

        public async Task<int> CreateNewPost(string tags, string rating, string sources, string md5, string url = "", string description = "", string parent_id = "")
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreatePost);
            if (request == null)
            {
                return -1;
            }
            if (Username == null || ApiKey == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to post."
                };
                return -1;
            }
            request.AddQueryParameter("login", Username);
            request.AddQueryParameter("api_key", ApiKey);
            request.AddQueryParameter("upload[tag_string]", tags);
            request.AddQueryParameter("upload[rating]", rating);
            request.AddQueryParameter("upload[source]", sources);
            if (description != "")
            {
                request.AddQueryParameter("upload[description]", description);
            }
            if (parent_id != "")
            {
                request.AddQueryParameter("upload[parent_id]", parent_id);
            }
            if (url != "")
            {
                request.AddQueryParameter("upload[direct_url]", url);
                RestResponse restResponse = await HostClient.ExecuteAsync(request);
                if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
                {
                    LastError = new ErrorDetails
                    {
                        ResponseCode = (StatusCode)restResponse.StatusCode,
                        Message = restResponse.StatusDescription
                    };
                    return -1;
                }
                CreatePostResponse createPostResponse;
                try
                {
                    createPostResponse = JsonConvert.DeserializeObject<CreatePostResponse>(restResponse.Content);
                }
                catch (Exception ex)
                {
                    LastError = new ErrorDetails
                    {
                        ResponseCode = StatusCode.JsonConversionFailure,
                        Message = "Couldn't convert json: " + ex.Message
                    };
                    return -1;
                }
                return createPostResponse.post_id;
            }
            LastError = new ErrorDetails
            {
                ResponseCode = StatusCode.MissingOrInvalidParameter,
                Message = "URL required!"
            };
            return -1;
        }

        public async Task<List<Post>> GetPopularPosts(PopularPosts Range, int Limit = 75, int Page = 1)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPosts);
            if (request == null)
            {
                return null;
            }
            DateTime now = DateTime.Now;
            string value = $"{now.Year}-{now.Month}-{now.Day}";
            switch (Range)
            {
            case PopularPosts.Day:
                HostClient = RestClientFactory("https://e621.net/explore/posts/popular?");
                request.AddQueryParameter("date", value);
                request.AddQueryParameter("scale", "day");
                break;
            case PopularPosts.Week:
                HostClient = RestClientFactory("https://e621.net/explore/posts/popular?");
                request.AddQueryParameter("date", value);
                request.AddQueryParameter("scale", "week");
                break;
            case PopularPosts.Month:
                HostClient = RestClientFactory("https://e621.net/explore/posts/popular?");
                request.AddQueryParameter("date", value);
                request.AddQueryParameter("scale", "month");
                break;
            }
            if (Limit < 1 || Limit > 320)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Limit must be a value within 1 - 320"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            if (Page < 1 || Page > 750)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be a value within 1 - 750"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            RestResponse restResponse = await HostClient.ExecuteAsync(request);
            if (restResponse.StatusCode != HttpStatusCode.OK && restResponse.StatusCode != HttpStatusCode.Created && restResponse.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)restResponse.StatusCode,
                    Message = restResponse.StatusDescription
                };
                return null;
            }
            List<Post> posts;
            try
            {
                posts = JsonConvert.DeserializeObject<List<Post>>(restResponse.Content);
            }
            catch (Exception ex)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + ex.Message
                };
                return null;
            }
            return posts;
        }

        
    }
}
