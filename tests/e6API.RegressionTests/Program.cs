using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using e6API;
using Newtonsoft.Json;
using RestSharp;


const string PostJson = """
[{"id":42,"created_at":"2026-01-01T00:00:00Z","updated_at":"2026-01-02T00:00:00Z",
"uploader_id":7,"uploader_name":"sample","approver_id":null,
"files":{"meta":{"md5":"abc","ext":"jpg","size":10,"duration":null,"has_sample":true},
"original":{"width":800,"height":600,"url":"https://example.invalid/original.jpg"},
"preview":{"width":150,"height":100,"jpg":"preview.jpg","webp":"preview.webp"},
"sample":{"width":400,"height":300,"jpg":"sample.jpg","webp":"sample.webp"}},
"stats":{"score":{"up":3,"down":-1,"total":2},"fav_count":5,"is_favorited":true,"vote":1,"comment_count":2,"hotness":1.5},
"has":{"parent":false,"children":true,"active_children":true,"notes":false,"sample":true},
"relationships":{"parent_id":null,"children":[43]},"pools":[9],"tags":["sample_tag"],"rating":"s"}]
""";

var failures = new List<string>();
var passed = 0;
async Task Run(string name, Func<Task> check)
{
    try { await check(); passed++; Console.WriteLine($"PASS {name}"); }
    catch (Exception ex) { failures.Add(name); Console.Error.WriteLine($"FAIL {name}: {ex}"); }
}

await Run("V2 post array, nested fields, numeric pagination and headers", async () =>
{
    using var http = new MemoryTransport(PostJson);
    var host = http.Host();
    var posts = await host.GetPosts("sample_tag", 25, 2);
    Check(posts is { Count: 1 }, "post list");
    var post = posts[0];
    Check(post.id == 42 && post.files.original.width == 800 && post.files.preview.webp == "preview.webp", "V2 files");
    Check(post.stats.score.total == 2 && post.stats.is_favorited && post.tags[0] == "sample_tag", "V2 stats and tags");
    Check(post.relationships.parent_id == null && post.relationships.children[0] == 43 && post.has.sample, "V2 relationships");
    var request = http.Last;
    Check(request.Method == HttpMethod.Get && request.Uri.AbsolutePath == "/posts.json", "posts route");
    request.QueryIs("v2", "true"); request.QueryIs("tags", "sample_tag"); request.QueryIs("limit", "25"); request.QueryIs("page", "2");
    Check(request.UserAgent == "RecoveryTests/1.0" && request.Accept == "application/json", "headers");
});

foreach (var direction in new[] { 'a', 'b' })
    await Run($"V2 posts with {direction} cursor pagination", async () =>
    {
        using var http = new MemoryTransport(PostJson);
        var posts = await http.Host().GetPosts("sample", direction, 100, 20);
        Check(posts[0].files.meta.md5 == "abc", "V2 cursor response");
        http.Last.QueryIs("page", $"{direction}100"); http.Last.QueryIs("v2", "true");
    });

foreach (var range in Enum.GetValues<PopularPosts>())
    await Run($"V2 popular posts for {range}", async () =>
    {
        using var http = new MemoryTransport(PostJson);
        var posts = await http.Host().GetPopularPosts(range, 10, 3);
        Check(posts[0].stats.fav_count == 5, "popular response");
        Check(http.Last.Uri.AbsolutePath == "/explore/posts/popular", "popular route");
        http.Last.QueryIs("v2", "true"); http.Last.QueryIs("scale", range.ToString().ToLowerInvariant());
        http.Last.QueryIs("limit", "10"); http.Last.QueryIs("page", "3");
    });

await Run("Recovered autocomplete, wiki, set and blip searches", async () =>
{
    using var http = new MemoryTransport("[{\"id\":8,\"name\":\"sample\"}]");
    var host = http.Host();
    Check((await host.GetAutocompletetags("sam"))[0].name == "sample", "autocomplete result");
    Check(http.Last.Uri.AbsolutePath == "/tags/autocomplete.json", "autocomplete route"); http.Last.QueryIs("search[name_matches]", "sam");
    http.Json = "[{\"id\":9,\"title\":\"sample\"}]";
    Check((await host.SearchWikiEntries("sam"))[0].title == "sample", "wiki result");
    Check(http.Last.Uri.AbsolutePath == "/wiki_pages.json", "wiki route"); http.Last.QueryIs("search[title]", "sam*");
    http.Json = "[{\"id\":10,\"name\":\"samples\",\"post_ids\":[42]}]";
    Check((await host.SearchSets("sam", 20, 4))[0].post_ids[0] == 42, "set result");
    Check(http.Last.Uri.AbsolutePath == "/post_sets", "set route"); http.Last.QueryIs("page", "4");
    http.Json = "[{\"id\":11,\"body\":\"hello\",\"response_to\":null}]";
    Check((await host.ListBlips(12))[0].body == "hello", "blip result");
    Check(http.Last.Uri.AbsolutePath == "/blips", "blip route"); http.Last.QueryIs("limit", "12");
});

await Run("Recovered authentication and blacklist update", async () =>
{
    using var http = new MemoryTransport("[]");
    var host = http.Host();
    Check(await host.TryAuthenticate("user", "test-key"), "authentication success");
    http.Last.QueryIs("login", "user"); http.Last.QueryIs("api_key", "test-key");
    http.Json = "{\"reason\":\"AuthenticationFailure\"}";
    Check(!await host.TryAuthenticate("user", "bad-key"), "authentication rejection");
    http.Json = ""; http.Status = HttpStatusCode.NoContent;
    Check(await host.UpdateUserBlacklist("7", "user", "test-key", "sample\nsecond"), "blacklist success");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/users/7", "blacklist route");
    Check(http.Last.Body.Contains("blacklisted_tags") && http.Last.Body.Contains("sample\nsecond") && http.Last.Body.Contains("patch"), "blacklist form");
});

await Run("Recovered comment creation, blip creation and URL upload", async () =>
{
    using var http = new MemoryTransport("{}", HttpStatusCode.Created);
    var host = http.Host(authenticated: true);
    Check(await host.CommentOnPost(42, "hello"), "comment creation");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/comments.json", "comment route");
    http.Last.QueryIs("comment[post_id]", "42"); http.Last.QueryIs("comment[body]", "hello");
    await host.CreateBlips("hello blip", "8");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/blips", "create blip route");
    http.Last.QueryIs("blip[body]", "hello blip"); http.Last.QueryIs("blip[response_to]", "8");
    http.Json = "{\"success\":true,\"post_id\":99}";
    Check(await host.CreateNewPost("sample", "s", "https://example.invalid/source", "md5", "https://example.invalid/image.jpg", "description", "42") == 99, "upload result");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/uploads.json", "upload route");
    http.Last.QueryIs("upload[direct_url]", "https://example.invalid/image.jpg");
    http.Last.QueryIs("upload[tag_string]", "sample"); http.Last.QueryIs("upload[parent_id]", "42");
});

await Run("Distinct enum values route favorites and comments correctly", async () =>
{
    Check((int)E6RequestType.FavoritePost != (int)E6RequestType.CreateComment, "enum collision");
    using var http = new MemoryTransport("{}");
    var host = http.Host(authenticated: true);
    Check(await host.FavoritePost(42), "favorite result");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/favorites.json", "favorite route");
    Check(await host.UnfavoritePost(42), "unfavorite result");
    Check(http.Last.Method == HttpMethod.Delete && http.Last.Uri.AbsolutePath == "/favorites/42.json", "unfavorite route");
});

await Run("SetVote no_unvote behavior", async () =>
{
    using var http = new MemoryTransport("{\"success\":true,\"our_score\":1}");
    var host = http.Host(authenticated: true);
    Check((await host.SetVote(42, 1)).our_score == 1, "legacy vote result");
    http.Last.QueryIs("score", "1"); http.Last.QueryIs("no_unvote", "false");
    await host.SetVote(42, -1, true);
    http.Last.QueryIs("score", "-1"); http.Last.QueryIs("no_unvote", "true");
    Check(http.Last.Method == HttpMethod.Post && http.Last.Uri.AbsolutePath == "/posts/42/votes.json", "vote route");
});

await Run("Current tag/comment/user/pool API", async () =>
{
    using var http = new MemoryTransport("[{\"id\":7,\"name\":\"sample\"}]");
    var host = http.Host();
    Check((await host.GetTags("sam*", 20, 2))[0].id == 7, "GetTags");
    http.Last.QueryIs("page", "2");
    await host.GetTags("sam*", 'b', 20); http.Last.QueryIs("page", "b20");
    await host.GetTags("sam*", 'a', 20); http.Last.QueryIs("page", "a20");
    await host.SearchComments("hello");
    http.Last.QueryIs("search[body_matches]", "hello");
    await host.GetPostComments(42); http.Last.QueryIs("search[post_id]", "42");
    await host.GetUserComments("user"); http.Last.QueryIs("search[creator_name]", "user");
    await host.GetUserComments(7); http.Last.QueryIs("search[creator_id]", "7");
    await host.SearchUsers("user");
    http.Last.QueryIs("search[name_matches]", "user");
    await host.SearchPoolNames("pool", 20, 3); http.Last.QueryIs("page", "3");
    await host.SearchPoolNames("pool", 20, 1, PoolsSortOrder.name); http.Last.QueryIs("page", "1");
    await host.SearchPoolNames("pool", 20, 4); http.Last.QueryIs("page", "4");
    await host.SearchPoolDescriptions("desc");
    http.Last.QueryIs("search[description_matches]", "desc");
    await host.SearchPoolUserPools("user");
    http.Last.QueryIs("search[creator_name]", "user");
    await host.SearchPoolUserPools(7);
    http.Last.QueryIs("search[creator_id]", "7");
    Check((await host.GetPoolInfo(7)).id == 7, "pool info");
});

await Run("Invalid configuration, limits and missing authentication avoid HTTP", async () =>
{
    using var http = new MemoryTransport("[]");
    var host = http.Host();
    Check(await host.GetPosts(null!) == null, "null tags");
    Check(await host.GetPosts("", 0) == null, "invalid limit");
    Check(await host.GetPosts("", 75, 751) == null, "invalid page");
    Check(await host.GetPosts("", 'x', 1) == null, "invalid direction");
    Check(await host.GetPosts("", 'b', -1) == null, "invalid cursor id");
    Check(await host.SearchComments(null!) == null, "null comment search");
    Check(await host.GetUserComments((string)null!) == null, "null comment creator");
    Check(!await host.CommentOnPost(42, "hello"), "comment requires auth");
    await host.CreateBlips("hello");
    Check(await host.CreateNewPost("sample", "s", "source", "md5", "https://example.invalid/file") == -1, "upload requires auth");
    Check(!await host.FavoritePost(42) && !await host.UnfavoritePost(42), "favorites require auth");
    Check(http.Requests.Count == 0, "invalid calls must not send requests");
    host.UserAgent = null!;
    Check(await host.GetPosts("") == null && host.LastError.ResponseCode == StatusCode.MissingOrInvalidParameter, "missing User-Agent");
    Check(await host.SetVote(42, 1) == null, "missing User-Agent vote");
    host.UserAgent = "RecoveryTests/1.0"; host.Username = "user";
    Check(await host.SearchSets("sample") == null, "partial credentials");
    Check(http.Requests.Count == 0, "configuration failures must not send requests");
});

await Run("HTTP and JSON failures preserve LastError", async () =>
{
    using var http = new MemoryTransport("{}", HttpStatusCode.Forbidden);
    var host = http.Host();
    Check(await host.GetPosts("") == null && host.LastError.ResponseCode == StatusCode.Forbidden, "HTTP failure");
    http.Status = HttpStatusCode.OK; http.Json = "{broken";
    Check(await host.GetPosts("") == null && host.LastError.ResponseCode == StatusCode.JsonConversionFailure, "malformed JSON");
    http.Json = "{\"posts\":[]}";
    Check(await host.GetPosts("") == null && host.LastError.ResponseCode == StatusCode.JsonConversionFailure, "legacy envelope is not a V2 array");
    http.Json = "[]";
    Check((await host.GetPosts("")) is { Count: 0 }, "empty V2 array");
    Check(await host.GetPoolInfo(42) == null && host.LastError.ResponseCode == StatusCode.NotFound, "pool not found");
});

await Run("V2 and recovered creation models deserialize", () =>
{
    var posts = JsonConvert.DeserializeObject<List<Post>>(PostJson);
    Check(posts![0].files.original.width == 800, "V2 model shape");
    Check(JsonConvert.DeserializeObject<CreatePostReturn>("{\"post_id\":42}")!.post_id == 42, "recovered upload model");
    return Task.CompletedTask;
});

await ApiCompletionChecks.Run(Run, PostJson);
if (args.Length > 0)
    await Run("Existing public API preserved against the supplied baseline DLL", () =>
    {
        var context = new AssemblyLoadContext("recovery-baseline", isCollectible: true);
        try
        {
            var baseline = context.LoadFromAssemblyPath(Path.GetFullPath(args[0]));
            var restored = typeof(RequestHost).Assembly;
            var types = baseline.GetExportedTypes();
            foreach (var oldType in types)
            {
                var newType = restored.GetType(oldType.FullName!);
                Check(newType != null, $"missing public type {oldType.FullName}");
                foreach (var oldMember in oldType.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    var signature = Signature(oldMember);
                    var newMember = newType!.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .FirstOrDefault(m => Signature(m) == signature);
                    Check(newMember != null, $"missing or changed member {oldType.FullName}.{signature}");
                    if (oldMember is MethodInfo oldMethod && newMember is MethodInfo newMethod)
                    {
                        var expectedReturn = TypeName(oldMethod.ReturnType);
                        Check(TypeName(newMethod.ReturnType) == expectedReturn, $"return type {signature}");
                    }
                    if (oldMember is FieldInfo { IsLiteral: true } oldField && newMember is FieldInfo newField)
                        Check(Equals(oldField.GetRawConstantValue(), newField.GetRawConstantValue()), $"enum value {signature}");
                    if (oldMember is MethodBase oldCallable && newMember is MethodBase newCallable)
                        foreach (var pair in oldCallable.GetParameters().Zip(newCallable.GetParameters()))
                        {
                            Check(pair.First.Name == pair.Second.Name, $"parameter name {signature}");
                            Check(pair.First.IsOptional == pair.Second.IsOptional, $"optional parameter {signature}");
                            if (pair.First.HasDefaultValue)
                                Check(Convert.ToString(pair.First.DefaultValue) == Convert.ToString(pair.Second.DefaultValue), $"default value {signature}");
                        }
                }
            }
            Console.WriteLine($"  Compared all members of {types.Length} existing public types.");
        }
        finally { context.Unload(); }
        return Task.CompletedTask;
    });

Console.WriteLine($"{passed} checks passed; {failures.Count} failed. No live API calls were made.");
return failures.Count == 0 ? 0 : 1;

static void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

static string TypeName(Type type) => type.IsGenericType
    ? type.GetGenericTypeDefinition().FullName + "<" + string.Join(",", type.GetGenericArguments().Select(TypeName)) + ">"
    : type.FullName!;

static string Signature(MemberInfo member) => member switch
{
    MethodBase callable => member.MemberType + " " + member.Name + "(" + string.Join(",", callable.GetParameters().Select(p => TypeName(p.ParameterType))) + ")",
    PropertyInfo property => "Property " + property.Name + ":" + TypeName(property.PropertyType),
    FieldInfo field => "Field " + field.Name + ":" + TypeName(field.FieldType),
    _ => member.MemberType + " " + member.Name
};

sealed record CapturedRequest(HttpMethod Method, Uri Uri, string Body, string UserAgent, string Accept)
{
    public void QueryIs(string key, string expected)
    {
        var pairs = Uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split('=', 2))
            .Where(p => System.Uri.UnescapeDataString(p[0]) == key)
            .Select(p => System.Uri.UnescapeDataString(p.Length == 2 ? p[1].Replace("+", " ") : "")).ToArray();
        if (!pairs.Contains(expected)) throw new InvalidOperationException($"Query {key}: expected {expected}; got [{string.Join(", ", pairs)}]");
    }
}

sealed class MemoryTransport(string json, HttpStatusCode status = HttpStatusCode.OK) : HttpMessageHandler
{
    public string Json { get; set; } = json;
    public HttpStatusCode Status { get; set; } = status;
    public List<CapturedRequest> Requests { get; } = [];
    private readonly List<RestClient> clients = [];
    public CapturedRequest Last => Requests[^1];

    public RequestHost Host(bool authenticated = false)
    {
        var host = authenticated
            ? new RequestHost("RecoveryTests/1.0", "user", "test-key")
            : new RequestHost("RecoveryTests/1.0");
        Func<string, RestClient> factory = url =>
        {
            var client = new RestClient(new HttpClient(this, disposeHandler: false), new RestClientOptions(url), disposeHttpClient: true);
            clients.Add(client);
            return client;
        };
        typeof(RequestHost).GetProperty("RestClientFactory", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(host, factory);
        return host;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(new CapturedRequest(request.Method, request.RequestUri!,
            request.Content == null ? "" : await request.Content.ReadAsStringAsync(cancellationToken),
            request.Headers.UserAgent.ToString(), string.Join(",", request.Headers.Accept)));
        return new HttpResponseMessage(Status)
        {
            Content = new StringContent(Json, Encoding.UTF8, "application/json"),
            RequestMessage = request
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) foreach (var client in clients) client.Dispose();
        base.Dispose(disposing);
    }
}
