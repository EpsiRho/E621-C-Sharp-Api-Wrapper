using System;
using System.Collections.Generic;

namespace e6API
{
    public class Set
    {
        public int id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public int creator_id { get; set; }

        public bool is_public { get; set; }

        public string name { get; set; }

        public string shortname { get; set; }

        public string description { get; set; }

        public int post_count { get; set; }

        public bool transfer_on_delete { get; set; }

        public int[] post_ids { get; set; }
    }

    public class SetsJSON
    {
        public List<Set> sets { get; set; }
    }
}
