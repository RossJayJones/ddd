using Ddd;

namespace Sample.Domain.People
{
    public class ContactId : Identity
    {
        public ContactId(string value) : base(value)
        {
        }
    }

    public class Contact : Entity<ContactId>
    {
        public TelNumber Work { get; set; }

        public Email Email { get; set; }

        public TelNumber WorkTel { get; set; }
    }
}
