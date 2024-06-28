using BandLabTestAssignment.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BandLabTestAssignment.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.Creator)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
