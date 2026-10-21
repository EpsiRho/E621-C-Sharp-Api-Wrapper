using System.Net;
using e6API;
using Newtonsoft.Json;

static class ApiCompletionChecks
{
    public static async Task Run(Func<string, Func<Task>, Task> run, string postJson)
    {
        await run("Flag records, privacy fields, filters and pagination", async () =>
        {
            using var http = new MemoryTransport("[{\"id\":8,\"post_id\":42,\"creator_id\":null,\"is_resolved\":false,\"updated_at\":null,\"type\":\"flag\"}]");
            var host = http.Host();
            var flags = await host.GetPostFlags(42, 20, 2, "inferior*", false, "flag", 7, "user", "sample");
            Check(flags.Count == 1 && flags[0].creator_id == null && flags[0].updated_at == null && flags[0].type == "flag", "flag model");
            Route(http, HttpMethod.Get, "/post_flags.json");
            http.Last.QueryIs("search[post_id]", "42"); http.Last.QueryIs("search[is_resolved]", "false");
            http.Last.QueryIs("search[reason_matches]", "inferior*"); http.Last.QueryIs("search[type]", "flag");
            http.Last.QueryIs("search[creator_id]", "7"); http.Last.QueryIs("search[creator_name]", "user");
            http.Last.QueryIs("search[post_tags_match]", "sample"); http.Last.QueryIs("page", "2");
            await host.GetPostFlags();
            Missing(http.Last, "search[post_id]", "search[is_resolved]", "search[reason_matches]", "search[type]");
            http.Json = "[]";
            Check((await host.GetPostFlags()).Count == 0, "empty flags");
        });

        await run("Flag creation uses reason_name and optional parent/explanation", async () =>
        {
            using var http = new MemoryTransport("{}", HttpStatusCode.Created);
            var host = http.Host(authenticated: true);
            Check(await host.FlagPost(42, "inferior", 43, "higher resolution"), "flag creation");
            Route(http, HttpMethod.Post, "/post_flags.json");
            http.Last.QueryIs("post_flag[post_id]", "42"); http.Last.QueryIs("post_flag[reason_name]", "inferior");
            http.Last.QueryIs("post_flag[parent_id]", "43"); http.Last.QueryIs("post_flag[note]", "higher resolution");
            http.Last.QueryIs("login", "user"); http.Last.QueryIs("api_key", "test-key");
            Missing(http.Last, "post_flag[reason]");
            await host.FlagPost(42, "other");
            Missing(http.Last, "post_flag[parent_id]", "post_flag[note]");
        });

        await run("Flagged-post lists preserve V2 and both pagination modes", async () =>
        {
            using var http = new MemoryTransport(postJson);
            var host = http.Host();
            Check((await host.GetFlaggedPosts())[0].files.original.width == 800, "V2 flagged result");
            http.Last.QueryIs("tags", "status:flagged"); http.Last.QueryIs("v2", "true");
            await host.GetFlaggedPosts("sample", 20, 2);
            http.Last.QueryIs("tags", "sample status:flagged"); http.Last.QueryIs("page", "2");
            await host.GetFlaggedPosts("sample", 'b', 100, 20);
            http.Last.QueryIs("tags", "sample status:flagged"); http.Last.QueryIs("page", "b100"); http.Last.QueryIs("v2", "true");
        });

        await run("Post edits preserve omitted fields and support clearing, diffs and old values", async () =>
        {
            using var http = new MemoryTransport("", HttpStatusCode.NoContent);
            var host = http.Host(authenticated: true);
            Check(await host.UpdatePost(42, Description: ""), "clear post description");
            Route(http, HttpMethod.Patch, "/posts/42.json");
            http.Last.QueryIs("post[description]", ""); http.Last.QueryIs("v2", "true");
            Missing(http.Last, "post[tag_string]", "post[rating]", "post[source]", "post[parent_id]", "post[old_description]");
            Check(await host.UpdatePost(42, "new_tag", "s", "https://example.invalid/source", "updated", 0, "correction", "+new -old", "-old_source", "old_tag", "q", "old_source", "old_description", 99), "full post edit");
            http.Last.QueryIs("post[tag_string]", "new_tag"); http.Last.QueryIs("post[rating]", "s");
            http.Last.QueryIs("post[parent_id]", ""); http.Last.QueryIs("post[old_parent_id]", "99");
            http.Last.QueryIs("post[edit_reason]", "correction"); http.Last.QueryIs("post[tag_string_diff]", "+new -old");
            http.Last.QueryIs("post[source_diff]", "-old_source"); http.Last.QueryIs("post[old_tag_string]", "old_tag");
            http.Last.QueryIs("post[old_description]", "old_description");
        });

        await run("Alias models and filters use both names and pagination modes", async () =>
        {
            using var http = new MemoryTransport("[{\"id\":12,\"antecedent_name\":\"old_tag\",\"consequent_name\":\"new_tag\",\"status\":\"active\"}]");
            var host = http.Host();
            var aliases = await host.GetTagAliases("*tag", 20, 3, "active", "old_tag", "new_tag");
            Check(aliases[0].antecedent_name == "old_tag" && aliases[0].consequent_name == "new_tag", "alias model, not a Tag");
            Route(http, HttpMethod.Get, "/tag_aliases.json");
            http.Last.QueryIs("search[name_matches]", "*tag"); http.Last.QueryIs("search[status]", "active");
            http.Last.QueryIs("search[antecedent_name]", "old_tag"); http.Last.QueryIs("search[consequent_name]", "new_tag");
            http.Last.QueryIs("page", "3");
            foreach (var cursor in new[] { 'a', 'b' })
            {
                await host.GetTagAliases("*tag", cursor, 100, 10);
                http.Last.QueryIs("page", $"{cursor}100");
            }
            http.Json = "[]";
            Check((await host.GetTagAliases()).Count == 0, "empty aliases");
        });

        await run("Note list fields, optional active filter and revision IDs", async () =>
        {
            using var http = new MemoryTransport("[{\"id\":5,\"post_id\":42,\"body\":\"text\",\"x\":0,\"y\":10,\"width\":20,\"height\":30,\"is_active\":false,\"version\":2}]");
            var host = http.Host();
            var notes = await host.GetNotes(42, 20, 3, "text*", false, 7, "user", "sample");
            Check(notes[0].width == 20 && !notes[0].is_active && notes[0].body == "text", "note model");
            Route(http, HttpMethod.Get, "/notes.json");
            http.Last.QueryIs("search[post_id]", "42"); http.Last.QueryIs("search[is_active]", "false");
            http.Last.QueryIs("search[body_matches]", "text*"); http.Last.QueryIs("search[creator_id]", "7");
            http.Last.QueryIs("search[creator_name]", "user"); http.Last.QueryIs("search[post_tags_match]", "sample");
            await host.GetNotes(); Missing(http.Last, "search[post_id]", "search[is_active]");
            http.Json = "[{\"id\":501,\"note_id\":5,\"post_id\":42,\"body\":\"old text\",\"version\":2}]";
            var revisions = await host.GetNoteVersions(5, 10, 2);
            Check(revisions[0].id == 501 && revisions[0].version == 2, "revision ID differs from version number");
            Route(http, HttpMethod.Get, "/note_versions.json");
            http.Last.QueryIs("search[note_id]", "5"); http.Last.QueryIs("page", "2");
        });

        foreach (var shape in new[] { "encoded", "object", "flat" })
            await run($"CreateNote parses {shape} response", async () =>
            {
                var note = new Note { id = 5, post_id = 42, body = "translation" };
                var json = shape switch
                {
                    "encoded" => JsonConvert.SerializeObject(new { note = JsonConvert.SerializeObject(note), dtext = "translation" }),
                    "object" => JsonConvert.SerializeObject(new { note, dtext = "translation" }),
                    _ => JsonConvert.SerializeObject(note)
                };
                using var http = new MemoryTransport(json);
                Check(await http.Host(authenticated: true).CreateNote(42, 0, 10, 20, 30, "translation") == 5, "created note ID");
                Route(http, HttpMethod.Post, "/notes.json");
                http.Last.QueryIs("note[post_id]", "42"); http.Last.QueryIs("note[x]", "0"); http.Last.QueryIs("note[y]", "10");
                http.Last.QueryIs("note[width]", "20"); http.Last.QueryIs("note[height]", "30"); http.Last.QueryIs("note[body]", "translation");
            });

        await run("Note partial edits, delete and version reversion", async () =>
        {
            using var http = new MemoryTransport("", HttpStatusCode.NoContent);
            var host = http.Host(authenticated: true);
            Check(await host.UpdateNote(5, X: 0), "coordinate-only note edit");
            Route(http, HttpMethod.Put, "/notes/5.json"); http.Last.QueryIs("note[x]", "0");
            Missing(http.Last, "note[body]", "note[y]", "note[width]", "note[height]", "note[post_id]");
            Check(await host.UpdateNote(5, "updated", 1, 2, 30, 40), "full note edit");
            http.Last.QueryIs("note[body]", "updated"); http.Last.QueryIs("note[height]", "40");
            Check(await host.DeleteNote(5), "note delete"); Route(http, HttpMethod.Delete, "/notes/5.json");
            Check(await host.RevertNote(5, 501), "note revert"); Route(http, HttpMethod.Put, "/notes/5/revert.json");
            http.Last.QueryIs("version_id", "501"); Missing(http.Last, "note[version_id]");
        });

        await run("Pool creation and partial edits preserve order and explicit empty values", async () =>
        {
            using var http = new MemoryTransport("{\"id\":9,\"name\":\"sample_pool\"}", HttpStatusCode.Created);
            var host = http.Host(authenticated: true);
            Check(await host.CreatePool("sample_pool", "description", "collection", [43, 42], false) == 9, "created pool ID");
            Route(http, HttpMethod.Post, "/pools.json");
            http.Last.QueryIs("pool[name]", "sample_pool"); http.Last.QueryIs("pool[category]", "collection");
            http.Last.QueryIs("pool[post_ids_string]", "43 42"); http.Last.QueryIs("pool[is_active]", "false");
            await host.CreatePool("sample_pool"); Missing(http.Last, "pool[post_ids_string]");
            http.Status = HttpStatusCode.NoContent; http.Json = "";
            Check(await host.UpdatePool(9, Description: ""), "clear pool description");
            Route(http, HttpMethod.Put, "/pools/9.json"); http.Last.QueryIs("pool[description]", "");
            Missing(http.Last, "pool[name]", "pool[category]", "pool[post_ids_string]", "pool[is_active]");
            Check(await host.UpdatePool(9, PostIds: [], IsActive: false), "clear posts and deactivate pool");
            http.Last.QueryIs("pool[post_ids_string]", ""); http.Last.QueryIs("pool[is_active]", "false");
        });

        await run("Pool history and reversion support 64-bit revision IDs", async () =>
        {
            using var http = new MemoryTransport("[{\"id\":3000000000,\"pool_id\":9,\"post_ids\":[42],\"added_post_ids\":[42],\"removed_post_ids\":[],\"updater_id\":null,\"version\":2}]");
            var host = http.Host(authenticated: true);
            var revisions = await host.GetPoolVersions(9, 10, 2);
            Check(revisions[0].id == 3000000000L && revisions[0].post_ids[0] == 42 && revisions[0].updater_id == null, "pool revision model");
            Route(http, HttpMethod.Get, "/pool_versions.json"); http.Last.QueryIs("search[pool_id]", "9");
            http.Json = ""; http.Status = HttpStatusCode.NoContent;
            Check(await host.RevertPool(9, revisions[0].id), "pool revert");
            Route(http, HttpMethod.Put, "/pools/9/revert.json"); http.Last.QueryIs("version_id", "3000000000");
        });

        await run("Comment update, hide/unhide and administrator deletion routes", async () =>
        {
            using var http = new MemoryTransport("", HttpStatusCode.NoContent);
            var host = http.Host(authenticated: true);
            Check(await host.UpdateComment(8, "edited"), "comment update");
            Route(http, HttpMethod.Put, "/comments/8.json"); http.Last.QueryIs("comment[body]", "edited");
            Missing(http.Last, "comment[post_id]", "comment[is_sticky]", "comment[is_hidden]");
            await host.UpdateComment(8, "edited", false, false);
            http.Last.QueryIs("comment[is_sticky]", "false"); http.Last.QueryIs("comment[is_hidden]", "false");
            Check(await host.HideComment(8), "hide comment"); Route(http, HttpMethod.Post, "/comments/8/hide.json");
            Check(await host.UnhideComment(8), "unhide comment"); Route(http, HttpMethod.Post, "/comments/8/unhide.json");
            Check(await host.DeleteComment(8), "delete comment"); Route(http, HttpMethod.Delete, "/comments/8.json");
            Check(typeof(RequestHost).GetMethod("RevertComment") == null, "do not expose an unsupported comment-revert endpoint");
        });

        var mutations = new (string Name, Func<RequestHost, Task<bool>> Call)[]
        {
            ("flag post", h => h.FlagPost(42, "inferior")), ("update post", h => h.UpdatePost(42, Tags: "sample")),
            ("update note", h => h.UpdateNote(5, "updated")), ("delete note", h => h.DeleteNote(5)),
            ("revert note", h => h.RevertNote(5, 501)), ("update pool", h => h.UpdatePool(9, Description: "updated")),
            ("revert pool", h => h.RevertPool(9, 901)), ("update comment", h => h.UpdateComment(8, "edited")),
            ("delete comment", h => h.DeleteComment(8)), ("hide comment", h => h.HideComment(8)), ("unhide comment", h => h.UnhideComment(8))
        };
        foreach (var status in new[] { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted, HttpStatusCode.NoContent, HttpStatusCode.Forbidden, HttpStatusCode.UnprocessableEntity, HttpStatusCode.InternalServerError })
            await run($"All new mutation methods handle HTTP {(int)status}", async () =>
            {
                using var http = new MemoryTransport("", status);
                var host = http.Host(authenticated: true);
                var success = (int)status is >= 200 and <= 204;
                foreach (var mutation in mutations)
                {
                    Check(await mutation.Call(host) == success, $"{mutation.Name} status");
                    if (!success) Check((int)host.LastError.ResponseCode == (int)status, $"{mutation.Name} LastError");
                    Check(http.Last.UserAgent == "RecoveryTests/1.0" && http.Last.Accept == "application/json", $"{mutation.Name} headers");
                }
            });

        await run("New mutations and pool history require complete authentication", async () =>
        {
            using var http = new MemoryTransport("{}");
            var host = http.Host();
            foreach (var mutation in mutations) Check(!await mutation.Call(host), $"{mutation.Name} authentication");
            Check(await host.CreateNote(42, 0, 0, 10, 10, "text") == -1, "create note authentication");
            Check(await host.CreatePool("sample") == -1, "create pool authentication");
            Check(await host.GetPoolVersions(9) == null, "pool history authentication");
            Check(http.Requests.Count == 0 && host.LastError.ResponseCode == StatusCode.MissingOrInvalidParameter, "no unauthenticated HTTP");
            host.Username = "user";
            Check(!await host.UpdatePost(42, Tags: "sample"), "partial credentials");
            host.ApiKey = "";
            Check(!await host.UpdateNote(5, "edited"), "blank API key");
            Check(http.Requests.Count == 0, "no partial/blank credential HTTP");
        });

        await run("Invalid new input is rejected before HTTP", async () =>
        {
            using var http = new MemoryTransport("{}");
            var host = http.Host(authenticated: true);
            Check(!await host.FlagPost(0, "reason") && !await host.FlagPost(42, " ") && !await host.FlagPost(42, "inferior", 42), "invalid flag");
            Check(await host.GetPostFlags(PostId: 0) == null && await host.GetPostFlags(Type: "invalid") == null && await host.GetPostFlags(Page: 0) == null, "invalid flag list");
            Check(await host.GetFlaggedPosts(null!) == null && await host.GetFlaggedPosts("", 'x', 5) == null, "invalid flagged-post query");
            Check(!await host.UpdatePost(42) && !await host.UpdatePost(42, Rating: "bad") && !await host.UpdatePost(42, ParentId: 42), "invalid post edit");
            Check(await host.GetTagAliases(null!) == null && await host.GetTagAliases("", Limit: 0) == null && await host.GetTagAliases("", 'x', 5) == null, "invalid aliases");
            Check(await host.GetNotes(PostId: 0) == null && await host.GetNotes(Limit: 321) == null && await host.GetNoteVersions(0) == null, "invalid note list");
            Check(await host.CreateNote(42, -1, 0, 10, 10, "text") == -1 && await host.CreateNote(42, 0, 0, 0, 10, "text") == -1 && await host.CreateNote(42, 0, 0, 10, 10, " ") == -1, "invalid note creation");
            Check(!await host.UpdateNote(5) && !await host.UpdateNote(5, Width: 0) && !await host.UpdateNote(5, " "), "invalid note edit");
            Check(!await host.DeleteNote(0) && !await host.RevertNote(5, 0), "invalid note IDs");
            Check(await host.CreatePool(" ") == -1 && await host.CreatePool("sample", Category: "bad") == -1 && await host.CreatePool("sample", PostIds: [0]) == -1, "invalid pool creation");
            Check(!await host.UpdatePool(9) && !await host.UpdatePool(9, Name: " ") && !await host.UpdatePool(9, PostIds: [-1]), "invalid pool edits");
            Check(await host.GetPoolVersions(0) == null && !await host.RevertPool(9, 0), "invalid pool revision IDs");
            Check(!await host.UpdateComment(8, " ") && !await host.DeleteComment(0) && !await host.HideComment(0) && !await host.UnhideComment(0), "invalid comments");
            Check(http.Requests.Count == 0, "invalid calls must not send HTTP");
        });

        await run("All new list/creation methods report HTTP and JSON errors", async () =>
        {
            using var http = new MemoryTransport("not JSON", HttpStatusCode.Forbidden);
            var host = http.Host(authenticated: true);
            var readers = new Func<Task<object?>>[]
            {
                async () => await host.GetPostFlags(), async () => await host.GetTagAliases(), async () => await host.GetTagAliases("", 'b', 100),
                async () => await host.GetNotes(), async () => await host.GetNoteVersions(5), async () => await host.GetPoolVersions(9)
            };
            foreach (var read in readers)
                Check(await read() == null && host.LastError.ResponseCode == StatusCode.Forbidden, "reader HTTP error");
            Check(await host.CreateNote(42, 0, 0, 10, 10, "text") == -1 && host.LastError.ResponseCode == StatusCode.Forbidden, "note creation HTTP error");
            Check(await host.CreatePool("sample") == -1 && host.LastError.ResponseCode == StatusCode.Forbidden, "pool creation HTTP error");
            http.Status = HttpStatusCode.OK;
            foreach (var json in new[] { "not JSON", "null", "{}" })
            {
                http.Json = json;
                foreach (var read in readers)
                    Check(await read() == null && host.LastError.ResponseCode == StatusCode.JsonConversionFailure, "reader JSON error");
                Check(await host.CreateNote(42, 0, 0, 10, 10, "text") == -1 && host.LastError.ResponseCode == StatusCode.JsonConversionFailure, "note creation JSON error");
                Check(await host.CreatePool("sample") == -1 && host.LastError.ResponseCode == StatusCode.JsonConversionFailure, "pool creation JSON error");
            }
        });

        await run("All new methods return their error sentinel when User-Agent is missing", async () =>
        {
            using var http = new MemoryTransport("{}");
            var host = http.Host(authenticated: true);
            host.UserAgent = null!;
            foreach (var mutation in mutations) Check(!await mutation.Call(host), $"{mutation.Name} missing User-Agent");
            Check(await host.CreatePool("sample") == -1 && await host.CreateNote(42, 0, 0, 10, 10, "text") == -1, "creation sentinels");
            Check(await host.GetNotes() == null && await host.GetNoteVersions(5) == null && await host.GetPoolVersions(9) == null, "note/pool list sentinels");
            Check(await host.GetPostFlags() == null && await host.GetFlaggedPosts() == null && await host.GetTagAliases() == null, "post/alias list sentinels");
            Check(http.Requests.Count == 0 && host.LastError.ResponseCode == StatusCode.MissingOrInvalidParameter, "no HTTP without User-Agent");
        });
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Route(MemoryTransport http, HttpMethod method, string path)
    {
        Check(http.Last.Method == method && http.Last.Uri.AbsolutePath == path, $"expected {method} {path}, got {http.Last.Method} {http.Last.Uri.AbsolutePath}");
    }

    private static void Missing(CapturedRequest request, params string[] keys)
    {
        var present = request.Uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => Uri.UnescapeDataString(p.Split('=', 2)[0])).ToHashSet();
        foreach (var key in keys) Check(!present.Contains(key), $"omitted field was sent: {key}");
    }
}
