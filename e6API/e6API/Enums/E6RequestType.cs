using System;

namespace e6API
{
    public enum E6RequestType
    {
        /*
            POST => 1
            GET => 2
            PATCH => 3
            DELETE => 4
            PUT => 5
        */

        // Posts
        CreatePost=10,
        VoteOnPost=12,
        CreatePostFlag=13,
        ListPosts=20,
        ListPostFlags=21,
        UpdatePost=30,

        // Tags
        ListTags=22,
        ListTagAliases=23,

        // Notes
        CreateNote=14,
        ListNotes=24,
        DeleteNote=40,
        UpdateNote=50,
        RevertNote=51,

        // Pools
        CreatePool=17,
        ListPools=25,
        UpdatePool=52,
        RevertPool=53

    }
}
