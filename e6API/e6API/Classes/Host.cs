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
using Newtonsoft.Json.Linq;
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
            case E6RequestType.ListNoteVersions:
                HostClient = RestClientFactory("https://e621.net/note_versions.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
                break;
            case E6RequestType.ListPoolVersions:
                HostClient = RestClientFactory("https://e621.net/pool_versions.json?");
                restRequest = new RestRequest();
                restRequest.Method = Method.Get;
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
                restRequest.AddQueryParameter("v2", "true");
                break;
            case E6RequestType.VoteOnPost:
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.UpdateComment:
                restRequest = new RestRequest();
                restRequest.Method = Method.Put;
                break;
            case E6RequestType.DeleteComment:
                restRequest = new RestRequest();
                restRequest.Method = Method.Delete;
                break;
            case E6RequestType.HideComment:
                restRequest = new RestRequest();
                restRequest.Method = Method.Post;
                break;
            case E6RequestType.UnhideComment:
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

        
        /// <summary>
        /// Flags a post using a server-defined reason name. Some reasons require ParentId or Note.
        /// </summary>
        public async Task<bool> FlagPost(int PostId, string ReasonName, int? ParentId = null, string Note = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreatePostFlag);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (PostId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return false;
            }
            if (string.IsNullOrWhiteSpace(ReasonName))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: ReasonName is required"
                };
                return false;
            }
            if (ParentId.HasValue && (ParentId.Value <= 0 || ParentId.Value == PostId))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: ParentId must be a different positive post ID"
                };
                return false;
            }
            request.AddQueryParameter("post_flag[post_id]", PostId.ToString());
            request.AddQueryParameter("post_flag[reason_name]", ReasonName);
            if (ParentId != null)
            {
                request.AddQueryParameter("post_flag[parent_id]", ParentId.Value.ToString());
            }
            if (Note != null)
            {
                request.AddQueryParameter("post_flag[note]", Note);
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lists flag records, optionally filtered by post, reason, resolution, creator, or post tags.
        /// </summary>
        public async Task<List<PostFlag>> GetPostFlags(int? PostId = null, int Limit = 75, int Page = 1, string Reason = null, bool? IsResolved = null, string Type = null, int? CreatorId = null, string CreatorName = null, string PostTags = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPostFlags);
            if (request == null)
            {
                return null;
            }
            if (PostId.HasValue && PostId.Value <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return null;
            }
            if (CreatorId.HasValue && CreatorId.Value <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorId must be positive"
                };
                return null;
            }
            if (Type != null && Type != "flag" && Type != "deletion")
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Type must be flag or deletion"
                };
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be at least 1"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            request.AddQueryParameter("page", Page.ToString());
            if (PostId != null)
            {
                request.AddQueryParameter("search[post_id]", PostId.Value.ToString());
            }
            if (Reason != null)
            {
                request.AddQueryParameter("search[reason_matches]", Reason);
            }
            if (IsResolved != null)
            {
                request.AddQueryParameter("search[is_resolved]", IsResolved.Value ? "true" : "false");
            }
            if (Type != null)
            {
                request.AddQueryParameter("search[type]", Type);
            }
            if (CreatorId != null)
            {
                request.AddQueryParameter("search[creator_id]", CreatorId.Value.ToString());
            }
            if (CreatorName != null)
            {
                request.AddQueryParameter("search[creator_name]", CreatorName);
            }
            if (PostTags != null)
            {
                request.AddQueryParameter("search[post_tags_match]", PostTags);
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<PostFlag> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<PostFlag>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>Lists V2 posts with status:flagged, using numbered pages.</summary>
        public async Task<List<Post>> GetFlaggedPosts(string Tags = "", int Limit = 75, int Page = 1)
        {
            if (Tags == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            return await GetPosts((Tags + " status:flagged").Trim(), Limit, Page);
        }

        /// <summary>Lists V2 flagged posts before or after a post ID.</summary>
        public async Task<List<Post>> GetFlaggedPosts(string Tags, char PaginationChar, int PostId, int Limit = 75)
        {
            if (Tags == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Tag variable is null"
                };
                return null;
            }
            return await GetPosts((Tags + " status:flagged").Trim(), PaginationChar, PostId, Limit);
        }

        /// <summary>
        /// Updates supplied post fields. Null omits a field; empty strings clear text fields and ParentId = 0 clears the parent. Old fields enable conflict checks.
        /// </summary>
        public async Task<bool> UpdatePost(int PostId, string Tags = null, string Rating = null, string Sources = null, string Description = null, int? ParentId = null, string EditReason = null, string TagDiff = null, string SourceDiff = null, string OldTags = null, string OldRating = null, string OldSources = null, string OldDescription = null, int? OldParentId = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.UpdatePost);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (PostId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return false;
            }
            if (Rating != null && Rating != "s" && Rating != "q" && Rating != "e")
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Rating must be s, q, or e"
                };
                return false;
            }
            if (OldRating != null && OldRating != "s" && OldRating != "q" && OldRating != "e")
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: OldRating must be s, q, or e"
                };
                return false;
            }
            if (ParentId.HasValue && (ParentId.Value < 0 || ParentId.Value == PostId))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: ParentId must be zero or a different positive post ID"
                };
                return false;
            }
            if (OldParentId.HasValue && OldParentId.Value < 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: OldParentId must be zero or positive"
                };
                return false;
            }
            if (Tags == null && Rating == null && Sources == null && Description == null && ParentId == null && TagDiff == null && SourceDiff == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: At least one post field must be supplied"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/posts/" + PostId + ".json");
            if (Tags != null)
            {
                request.AddQueryParameter("post[tag_string]", Tags);
            }
            if (Rating != null)
            {
                request.AddQueryParameter("post[rating]", Rating);
            }
            if (Sources != null)
            {
                request.AddQueryParameter("post[source]", Sources);
            }
            if (Description != null)
            {
                request.AddQueryParameter("post[description]", Description);
            }
            if (ParentId != null)
            {
                request.AddQueryParameter("post[parent_id]", ParentId.Value == 0 ? "" : ParentId.Value.ToString());
            }
            if (EditReason != null)
            {
                request.AddQueryParameter("post[edit_reason]", EditReason);
            }
            if (TagDiff != null)
            {
                request.AddQueryParameter("post[tag_string_diff]", TagDiff);
            }
            if (SourceDiff != null)
            {
                request.AddQueryParameter("post[source_diff]", SourceDiff);
            }
            if (OldTags != null)
            {
                request.AddQueryParameter("post[old_tag_string]", OldTags);
            }
            if (OldRating != null)
            {
                request.AddQueryParameter("post[old_rating]", OldRating);
            }
            if (OldSources != null)
            {
                request.AddQueryParameter("post[old_source]", OldSources);
            }
            if (OldDescription != null)
            {
                request.AddQueryParameter("post[old_description]", OldDescription);
            }
            if (OldParentId != null)
            {
                request.AddQueryParameter("post[old_parent_id]", OldParentId.Value == 0 ? "" : OldParentId.Value.ToString());
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lists tag aliases matching either name. The cursor PostId parameter is an alias record ID, matching the existing tag-list convention.
        /// </summary>
        public async Task<List<TagAlias>> GetTagAliases(string Search = "", int Limit = 75, int Page = 1, string Status = null, string Antecedent = null, string Consequent = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListTagAliases);
            if (request == null)
            {
                return null;
            }
            if (Search == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Search is null"
                };
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be at least 1"
                };
                return null;
            }
            request.AddQueryParameter("page", Page.ToString());
            request.AddQueryParameter("search[name_matches]", Search);
            request.AddQueryParameter("limit", Limit.ToString());
            if (Status != null)
            {
                request.AddQueryParameter("search[status]", Status);
            }
            if (Antecedent != null)
            {
                request.AddQueryParameter("search[antecedent_name]", Antecedent);
            }
            if (Consequent != null)
            {
                request.AddQueryParameter("search[consequent_name]", Consequent);
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<TagAlias> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<TagAlias>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>
        /// Lists tag aliases matching either name. The cursor PostId parameter is an alias record ID, matching the existing tag-list convention.
        /// </summary>
        public async Task<List<TagAlias>> GetTagAliases(string Search, char PaginationChar, int PostId, int Limit = 75, string Status = null, string Antecedent = null, string Consequent = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListTagAliases);
            if (request == null)
            {
                return null;
            }
            if (Search == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Search is null"
                };
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
            if (PaginationChar != 'a' && PaginationChar != 'b')
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Invalid PaginationChar"
                };
                return null;
            }
            if (PostId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return null;
            }
            request.AddQueryParameter("page", PaginationChar.ToString() + PostId);
            request.AddQueryParameter("search[name_matches]", Search);
            request.AddQueryParameter("limit", Limit.ToString());
            if (Status != null)
            {
                request.AddQueryParameter("search[status]", Status);
            }
            if (Antecedent != null)
            {
                request.AddQueryParameter("search[antecedent_name]", Antecedent);
            }
            if (Consequent != null)
            {
                request.AddQueryParameter("search[consequent_name]", Consequent);
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<TagAlias> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<TagAlias>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>
        /// Lists notes with optional post, body, active state, creator, and post-tag filters.
        /// </summary>
        public async Task<List<Note>> GetNotes(int? PostId = null, int Limit = 75, int Page = 1, string Body = null, bool? IsActive = null, int? CreatorId = null, string CreatorName = null, string PostTags = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListNotes);
            if (request == null)
            {
                return null;
            }
            if (PostId.HasValue && PostId.Value <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return null;
            }
            if (CreatorId.HasValue && CreatorId.Value <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CreatorId must be positive"
                };
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be at least 1"
                };
                return null;
            }
            request.AddQueryParameter("limit", Limit.ToString());
            request.AddQueryParameter("page", Page.ToString());
            if (PostId != null)
            {
                request.AddQueryParameter("search[post_id]", PostId.Value.ToString());
            }
            if (Body != null)
            {
                request.AddQueryParameter("search[body_matches]", Body);
            }
            if (IsActive != null)
            {
                request.AddQueryParameter("search[is_active]", IsActive.Value ? "true" : "false");
            }
            if (CreatorId != null)
            {
                request.AddQueryParameter("search[creator_id]", CreatorId.Value.ToString());
            }
            if (CreatorName != null)
            {
                request.AddQueryParameter("search[creator_name]", CreatorName);
            }
            if (PostTags != null)
            {
                request.AddQueryParameter("search[post_tags_match]", PostTags);
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<Note> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<Note>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>
        /// Creates a note and returns its ID, or -1 on failure. Handles the server response containing JSON-encoded note data.
        /// </summary>
        public async Task<int> CreateNote(int PostId, int X, int Y, int Width, int Height, string Body)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreateNote);
            if (request == null)
            {
                return -1;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return -1;
            }
            if (PostId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostId must be positive"
                };
                return -1;
            }
            if (X < 0 || Y < 0 || Width <= 0 || Height <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Note coordinates must be nonnegative and dimensions positive"
                };
                return -1;
            }
            if (string.IsNullOrWhiteSpace(Body))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Body is required"
                };
                return -1;
            }
            request.AddQueryParameter("note[post_id]", PostId.ToString());
            request.AddQueryParameter("note[x]", X.ToString());
            request.AddQueryParameter("note[y]", Y.ToString());
            request.AddQueryParameter("note[width]", Width.ToString());
            request.AddQueryParameter("note[height]", Height.ToString());
            request.AddQueryParameter("note[body]", Body);
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return -1;
            }
            Note json;
            try
            {
                JObject envelope = JObject.Parse(response.Content);
                JToken note = envelope["note"] ?? envelope;
                json = note.Type == JTokenType.String
                    ? JsonConvert.DeserializeObject<Note>(note.Value<string>())
                    : note.ToObject<Note>();
                if (json == null || json.id <= 0)
                {
                    throw new JsonSerializationException("The response did not contain a created note ID.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return -1;
            }
            return json.id;
        }

        /// <summary>
        /// Updates only the supplied note text or rectangle fields.
        /// </summary>
        public async Task<bool> UpdateNote(int NoteId, string Body = null, int? X = null, int? Y = null, int? Width = null, int? Height = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.UpdateNote);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (NoteId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: NoteId must be positive"
                };
                return false;
            }
            if (Body == null && X == null && Y == null && Width == null && Height == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: At least one note field must be supplied"
                };
                return false;
            }
            if (Body != null && string.IsNullOrWhiteSpace(Body))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Body cannot be blank"
                };
                return false;
            }
            if ((X.HasValue && X.Value < 0) || (Y.HasValue && Y.Value < 0) || (Width.HasValue && Width.Value <= 0) || (Height.HasValue && Height.Value <= 0))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Note coordinates must be nonnegative and dimensions positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/notes/" + NoteId + ".json");
            if (Body != null)
            {
                request.AddQueryParameter("note[body]", Body);
            }
            if (X != null)
            {
                request.AddQueryParameter("note[x]", X.Value.ToString());
            }
            if (Y != null)
            {
                request.AddQueryParameter("note[y]", Y.Value.ToString());
            }
            if (Width != null)
            {
                request.AddQueryParameter("note[width]", Width.Value.ToString());
            }
            if (Height != null)
            {
                request.AddQueryParameter("note[height]", Height.Value.ToString());
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes a note by marking it inactive.
        /// </summary>
        public async Task<bool> DeleteNote(int NoteId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.DeleteNote);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (NoteId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: NoteId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/notes/" + NoteId + ".json");
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Permanently deletes a comment. The server requires an administrator account.
        /// </summary>
        public async Task<bool> DeleteComment(int CommentId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.DeleteComment);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (CommentId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CommentId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/comments/" + CommentId + ".json");
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reverts to a version record ID, not the sequential version number.
        /// </summary>
        public async Task<bool> RevertNote(int NoteId, int VersionId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.RevertNote);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (NoteId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: NoteId must be positive"
                };
                return false;
            }
            if (VersionId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: VersionId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/notes/" + NoteId + "/revert.json");
            request.AddQueryParameter("version_id", VersionId.ToString());
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reverts to a version record ID, not the sequential version number.
        /// </summary>
        public async Task<bool> RevertPool(int PoolId, long VersionId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.RevertPool);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (PoolId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PoolId must be positive"
                };
                return false;
            }
            if (VersionId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: VersionId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/pools/" + PoolId + "/revert.json");
            request.AddQueryParameter("version_id", VersionId.ToString());
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lists revision records whose IDs can be passed to the corresponding revert method.
        /// </summary>
        public async Task<List<NoteVersion>> GetNoteVersions(int NoteId, int Limit = 75, int Page = 1)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListNoteVersions);
            if (request == null)
            {
                return null;
            }
            if (NoteId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: NoteId must be positive"
                };
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be at least 1"
                };
                return null;
            }
            request.AddQueryParameter("search[note_id]", NoteId.ToString());
            request.AddQueryParameter("limit", Limit.ToString());
            request.AddQueryParameter("page", Page.ToString());
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<NoteVersion> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<NoteVersion>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>
        /// Lists revision records whose IDs can be passed to the corresponding revert method.
        /// </summary>
        public async Task<List<PoolVersion>> GetPoolVersions(int PoolId, int Limit = 75, int Page = 1)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.ListPoolVersions);
            if (request == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return null;
            }
            if (PoolId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PoolId must be positive"
                };
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
            if (Page < 1)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Page must be at least 1"
                };
                return null;
            }
            request.AddQueryParameter("search[pool_id]", PoolId.ToString());
            request.AddQueryParameter("limit", Limit.ToString());
            request.AddQueryParameter("page", Page.ToString());
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return null;
            }
            List<PoolVersion> json;
            try
            {
                json = JsonConvert.DeserializeObject<List<PoolVersion>>(response.Content);
                if (json == null)
                {
                    throw new JsonSerializationException("Expected a JSON array.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return null;
            }
            return json;
        }

        /// <summary>
        /// Creates a series or collection pool and returns its ID, or -1 on failure.
        /// </summary>
        public async Task<int> CreatePool(string Name, string Description = "", string Category = "series", int[] PostIds = null, bool IsActive = true)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.CreatePool);
            if (request == null)
            {
                return -1;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return -1;
            }
            if (string.IsNullOrWhiteSpace(Name))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Name is required"
                };
                return -1;
            }
            if (Category != "series" && Category != "collection")
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Category must be series or collection"
                };
                return -1;
            }
            if (PostIds != null && Array.Exists(PostIds, id => id <= 0))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostIds must contain positive post IDs"
                };
                return -1;
            }
            request.AddQueryParameter("pool[name]", Name);
            request.AddQueryParameter("pool[description]", Description ?? "");
            request.AddQueryParameter("pool[category]", Category);
            request.AddQueryParameter("pool[is_active]", IsActive ? "true" : "false");
            if (PostIds != null)
            {
                request.AddQueryParameter("pool[post_ids_string]", string.Join(" ", PostIds));
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return -1;
            }
            Pool json;
            try
            {
                json = JsonConvert.DeserializeObject<Pool>(response.Content);
                if (json == null || json.id <= 0)
                {
                    throw new JsonSerializationException("The response did not contain a created pool ID.");
                }
            }
            catch (Exception e)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.JsonConversionFailure,
                    Message = "Couldn't convert json: " + e.Message
                };
                return -1;
            }
            return json.id;
        }

        /// <summary>
        /// Updates supplied pool fields. Null preserves a field, and an empty PostIds array removes all posts from the pool.
        /// </summary>
        public async Task<bool> UpdatePool(int PoolId, string Name = null, string Description = null, string Category = null, int[] PostIds = null, bool? IsActive = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.UpdatePool);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (PoolId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PoolId must be positive"
                };
                return false;
            }
            if (Name == null && Description == null && Category == null && PostIds == null && IsActive == null)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: At least one pool field must be supplied"
                };
                return false;
            }
            if (Name != null && string.IsNullOrWhiteSpace(Name))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Name cannot be blank"
                };
                return false;
            }
            if (Category != null && Category != "series" && Category != "collection")
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Category must be series or collection"
                };
                return false;
            }
            if (PostIds != null && Array.Exists(PostIds, id => id <= 0))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: PostIds must contain positive post IDs"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/pools/" + PoolId + ".json");
            if (Name != null)
            {
                request.AddQueryParameter("pool[name]", Name);
            }
            if (Description != null)
            {
                request.AddQueryParameter("pool[description]", Description);
            }
            if (Category != null)
            {
                request.AddQueryParameter("pool[category]", Category);
            }
            if (PostIds != null)
            {
                request.AddQueryParameter("pool[post_ids_string]", string.Join(" ", PostIds));
            }
            if (IsActive != null)
            {
                request.AddQueryParameter("pool[is_active]", IsActive.Value ? "true" : "false");
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates a comment body. Sticky and hidden flags require the corresponding server-side staff permissions.
        /// </summary>
        public async Task<bool> UpdateComment(int CommentId, string Body, bool? IsSticky = null, bool? IsHidden = null)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.UpdateComment);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (CommentId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CommentId must be positive"
                };
                return false;
            }
            if (string.IsNullOrWhiteSpace(Body))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: Body is required"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/comments/" + CommentId + ".json");
            request.AddQueryParameter("comment[body]", Body);
            if (IsSticky != null)
            {
                request.AddQueryParameter("comment[is_sticky]", IsSticky.Value ? "true" : "false");
            }
            if (IsHidden != null)
            {
                request.AddQueryParameter("comment[is_hidden]", IsHidden.Value ? "true" : "false");
            }
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Hides a comment if the server permits the current user to hide it.
        /// </summary>
        public async Task<bool> HideComment(int CommentId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.HideComment);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (CommentId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CommentId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/comments/" + CommentId + "/hide.json");
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

        /// <summary>
        /// Unhides a comment. The server requires moderator permissions.
        /// </summary>
        public async Task<bool> UnhideComment(int CommentId)
        {
            RestRequest request = await CreateNewRestRequest(E6RequestType.UnhideComment);
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(ApiKey))
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Must be logged in to perform this operation."
                };
                return false;
            }
            if (CommentId <= 0)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = StatusCode.MissingOrInvalidParameter,
                    Message = "Invalid Parameter: CommentId must be positive"
                };
                return false;
            }
            HostClient = RestClientFactory("https://e621.net/comments/" + CommentId + "/unhide.json");
            RestResponse response = await HostClient.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.NoContent)
            {
                LastError = new ErrorDetails
                {
                    ResponseCode = (StatusCode)response.StatusCode,
                    Message = response.StatusDescription
                };
                return false;
            }
            return true;
        }

    }
}
