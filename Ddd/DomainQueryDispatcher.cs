using System;
using System.Threading.Tasks;

namespace Ddd
{
    public static class DomainQueryDispatcher
    {
        public static Func<IDomainQueryDispatcher> Factory;

        public static Task<TResponse> Execute<TResponse>(IDomainQuery<TResponse> query)
        {
            return Factory().Dispatch(query);
        }
    }
}
