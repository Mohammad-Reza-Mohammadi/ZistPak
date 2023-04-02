using Entities.Cargo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityConfiguration
{
    public class CargoConfig : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();


            #region Other Property
            builder.Property(c => c.status).IsRequired();
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            builder.Property(c => c.Rating).IsRequired();
            builder.Property(c => c.Count).IsRequired();
            builder.Property(c=>c.Whight).IsRequired();
            #endregion


        }
    }
}
