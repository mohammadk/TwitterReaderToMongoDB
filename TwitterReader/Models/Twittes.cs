using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterReader.Models
{
    public class User
    {
        public string profile_image_url { get; set; }
        public string friends_count { get; set; }
        public string followers_count { get; set; }
        public string screen_name { get; set; }
        public string statuses_count { get; set; }
    }

    public class Tweet
    {
        public string created_at { get; set; }
        public string text { get; set; }
        public User user { get { return Twittes.UserProfile; } set { Twittes.UserProfile = value; } }
  
    }

    public class Twittes
    {
        public List<Tweet> Tweets { get; set; }
        public static User UserProfile { get; set; }
    }
}