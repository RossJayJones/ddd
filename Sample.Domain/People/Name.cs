namespace Sample.Domain.People
{
    public class Name
    {
        public string First { get; set; }

        public string Last { get; set; }

        public string Full => string.Join(" ", First, Last);
    }
}
