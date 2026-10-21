using System;

namespace e6API
{
    public class Note
    {
        public int id { get; set; }
        public int creator_id { get; set; }
        public string creator_name { get; set; }
        public int post_id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool is_active { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int version { get; set; }
    }

    public class NoteVersion
    {
        public int id { get; set; }
        public int note_id { get; set; }
        public int post_id { get; set; }
        public int updater_id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool is_active { get; set; }
        public string body { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int version { get; set; }
    }
}
