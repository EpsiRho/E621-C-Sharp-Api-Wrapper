using System;
using System.Collections.Generic;

namespace e6API
{
    public class TagAlias
    {
        public int id { get; set; }
        public string antecedent_name { get; set; }
        public string reason { get; set; }
        public int creator_id { get; set; }
        public DateTime created_at { get; set; }
        public int? forum_post_id { get; set; }
        public DateTime? updated_at { get; set; }
        public int? forum_topic_id { get; set; }
        public string consequent_name { get; set; }
        public string status { get; set; }
        public int post_count { get; set; }
        public int? approver_id { get; set; }
    }

    public class TagAliasesHolder
    {
        public List<Tag> Tags { get; set; }
    }
}