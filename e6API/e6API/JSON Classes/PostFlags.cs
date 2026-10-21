using System;

namespace e6API
{
    public class PostFlag
    {
        public int id { get; set; }
        public int post_id { get; set; }
        public int? creator_id { get; set; }
        public string reason { get; set; }
        public string reason_name { get; set; }
        public string note { get; set; }
        public bool is_resolved { get; set; }
        public bool is_deletion { get; set; }
        public bool needs_parent_id { get; set; }
        public bool needs_staff_reason { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
