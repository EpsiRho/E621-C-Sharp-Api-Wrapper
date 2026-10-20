using System;
using System.Collections.Generic;

namespace e6API
{
    public class Blip
    {
        public int id { get; set; }

        public int creator_id { get; set; }

        public string body { get; set; }

        public int? response_to { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_hidden { get; set; }

        public object warning_type { get; set; }

        public object warning_user_id { get; set; }

        public string creator_name { get; set; }
    }

    public class BlipsJSON
    {
        public List<Blip> blips { get; set; }
    }
}
