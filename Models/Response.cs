namespace SocialNetwork.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public Registration Registration { get; set; }

        public List<Registration> ListRegistration { get; set; }

        public List<Article> ListArticles { get; set; }

        public List<News> ListNews { get; set; }

        public List<Events> ListEvents { get; set; }

        public List<Staff> ListStaff { get; set; }

        public object Data { get; set;  }
    }
}
