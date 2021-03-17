/*
    E621 API Wrapper
    Written by Epsi Rho
    Version 0.1
    Last Updated 0.1 03/17/21@1:07
*/

namespace e6API
{
    public class CreatePostResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string location { get; set; }
        public string reason { get; set; }
        public int post_id { get; set; }
    }
}