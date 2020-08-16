namespace CoreBanking.Models
{
    public class AddressModel
    {
        public results[] results;
    }
    public class results
    {
        public string gender { get; set; }
        public name name { get; set; }
        public location location { get; set; }
    }

    public class name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }

    }

    public class location
    {
        public street street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
    }

    public class street
    {
        public string number { get; set; }
        public string name { get; set; }
    }
}
