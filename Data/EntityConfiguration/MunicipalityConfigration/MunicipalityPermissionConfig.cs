using Entities.Municipality;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityConfiguration.MunicipalityConfigration
{
    public class MunicipalityPermissionConfig : IEntityTypeConfiguration<MunicipalityPermissions>
    {
        public void Configure(EntityTypeBuilder<MunicipalityPermissions> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Permission).IsRequired();

            #region navigtion Property
            builder.HasOne(c => c.municipality)
                .WithMany(c => c.municipalityPermissions)
                .HasForeignKey(c => c.municiaplityId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
