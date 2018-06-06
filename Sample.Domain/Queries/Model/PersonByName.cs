using Sample.Domain.People;

namespace Sample.Domain.Queries.Model
{
    public class PersonByName
    {
        public PersonId PersonId { get; set; }

        public Name Name { get; set; }
    }
}
