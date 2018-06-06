using Ddd;

namespace Sample.Domain.Queries
{
    public class FindTeamNameById : IDomainQuery<string>
    {
        public TeamId TeamId { get; set; }
    }
}
