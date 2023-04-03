using Entities.Useres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityConfiguration.UserConfiguration
{
    public class UserPermissions : IEntityTypeConfiguration<UPermissions>
    {
        public void Configure(EntityTypeBuilder<UPermissions> builder)
        {
            //key
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Permission).IsRequired();

            #region navigtion Property
            builder.HasOne(c=>c.user)
                .WithMany(c=>c.permissions)
                .HasForeignKey(c => c.userId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(c => c.municipality)
                .WithMany(c => c.UPermissions)
                .HasForeignKey(c => c.municiaplityId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }

    }
}
