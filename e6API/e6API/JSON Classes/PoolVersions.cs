using System;

namespace e6API
{
    public class PoolVersion
    {
        public long id { get; set; }
        public int pool_id { get; set; }
        public int[] post_ids { get; set; }
        public int[] added_post_ids { get; set; }
        public int[] removed_post_ids { get; set; }
        public int? updater_id { get; set; }
        public string description { get; set; }
        public bool description_changed { get; set; }
        public string name { get; set; }
        public bool name_changed { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool is_active { get; set; }
        public string category { get; set; }
        public int version { get; set; }
    }
}
