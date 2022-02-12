using System;
using System.Collections.Generic;
using System.Text;

namespace e6API
{
    public class Comment
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public int post_id { get; set; }
        public int creator_id { get; set; }
        public string body { get; set; }
        public int score { get; set; }
        public DateTime updated_at { get; set; }
        public int updater_id { get; set; }
        public bool do_not_bump_post { get; set; }
        public bool is_hidden { get; set; }
        public bool is_sticky { get; set; }
        public string creator_name { get; set; }
        public string updater_name { get; set; }
    }

    public class CommentsHolder
    {
        public List<Comment> comments { get; set; }
    }
}
