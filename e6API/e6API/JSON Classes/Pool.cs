using System;
using System.Collections.Generic;

namespace e6API
{
    public class Pool
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int creator_id { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public string category { get; set; }
        public bool is_deleted { get; set; }
        public List<int> post_ids { get; set; }
        public string creator_name { get; set; }
        public int post_count { get; set; }
    }

    public class PoolsHolder
    {
        public List<Pool> Pools { get; set; }
    }
}
