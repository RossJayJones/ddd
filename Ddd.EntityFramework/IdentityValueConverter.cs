using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ddd.EntityFramework
{
    public class IdentityValueConverter<TModel> : ValueConverter<TModel, string>
        where TModel : Identity
    {
        public IdentityValueConverter(Func<string, TModel> factory) 
            : base(item => item.ToString(), item => factory(item))
        {
        }
    }
}
