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
    public class MunicipalityConfig : IEntityTypeConfiguration<Municipality>
    {
        public void Configure(EntityTypeBuilder<Municipality> builder)
        {
            // Key Property
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();

            #region OtherProperty
            builder.Property(c=>c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            builder.Property(c=>c.Name).IsRequired();
            builder.Property(c => c.Region).IsRequired();
            #endregion

        }
    }
}
