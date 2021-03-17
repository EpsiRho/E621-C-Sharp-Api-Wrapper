/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

using System;
using System.Collections.Generic;

namespace e6API
{
    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public int post_count { get; set; }
        public string related_tags { get; set; }
        public DateTime related_tags_updated_at { get; set; }
        public int category { get; set; }
        public bool is_locked { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class TagsHolder
    {
        public List<Tag> tags { get; set; }
    }
}