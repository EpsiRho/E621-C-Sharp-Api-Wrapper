using System;
using System.Collections.Generic;
using System.Text;

namespace e6API
{

    public class Post
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int change_seq { get; set; }
        public Files files { get; set; }
        public int uploader_id { get; set; }
        public string uploader_name { get; set; }
        public int? approver_id { get; set; }
        public Stats stats { get; set; }
        public Flags flags { get; set; }
        public Has has { get; set; }
        public Relationships relationships { get; set; }
        public int?[] pools { get; set; }
        public string rating { get; set; }
        public string[] locked_tags { get; set; }
        public string[] sources { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
    }

    public class Files
    {
        public Meta meta { get; set; }
        public Original original { get; set; }
        public Preview preview { get; set; }
        public Sample sample { get; set; }
    }

    public class Meta
    {
        public string md5 { get; set; }
        public string ext { get; set; }
        public int size { get; set; }
        public float? duration { get; set; }
        public bool has_sample { get; set; }
    }

    public class Original
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    public class Preview
    {
        public int width { get; set; }
        public int height { get; set; }
        public string jpg { get; set; }
        public string webp { get; set; }
    }

    public class Sample
    {
        public int width { get; set; }
        public int height { get; set; }
        public string jpg { get; set; }
        public string webp { get; set; }
    }

    public class Stats
    {
        public Score score { get; set; }
        public int fav_count { get; set; }
        public bool is_favorited { get; set; }
        public int vote { get; set; }
        public int comment_count { get; set; }
        public float hotness { get; set; }
    }

    public class Score
    {
        public int up { get; set; }
        public int down { get; set; }
        public int total { get; set; }
    }

    public class Flags
    {
        public bool pending { get; set; }
        public bool flagged { get; set; }
        public bool note_locked { get; set; }
        public bool status_locked { get; set; }
        public bool rating_locked { get; set; }
        public bool deleted { get; set; }
    }

    public class Has
    {
        public bool parent { get; set; }
        public bool children { get; set; }
        public bool active_children { get; set; }
        public bool notes { get; set; }
        public bool sample { get; set; }
    }

    public class Relationships
    {
        public int? parent_id { get; set; }
        public int?[] children { get; set; }
    }

}
