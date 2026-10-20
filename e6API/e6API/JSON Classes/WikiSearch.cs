using System;
using System.Collections.Generic;

namespace e6API
{
    public class WikiSearch
    {
        public int id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string title { get; set; }

        public string body { get; set; }

        public int creator_id { get; set; }

        public bool is_locked { get; set; }

        public int? updater_id { get; set; }

        public bool is_deleted { get; set; }

        public List<object> other_names { get; set; }

        public string creator_name { get; set; }

        public int category_name { get; set; }
    }
}
