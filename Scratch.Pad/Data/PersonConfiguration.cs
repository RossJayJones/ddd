using Ddd.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Scratch.Pad.Data
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable(nameof(Person));
            builder.HasKey(nameof(Person.Id));
            builder.Property(nameof(Person.Id)).HasConversion(new IdentityValueConverter<PersonId>(item => new PersonId(item)));
            builder.Property(nameof(Person.FirstName)).IsRequired();
            builder.Property(nameof(Person.LastName)).IsRequired();
        }
    }
}
