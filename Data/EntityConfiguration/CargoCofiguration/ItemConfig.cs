using Entities.Cargo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityConfiguration
{
    public class ItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i=>i.Id);

            builder.Property(i => i.Name).IsRequired();
            builder.Property(i => i.Whight).IsRequired();
            builder.Property(i => i.Rating).IsRequired();

            #region Navigation Property
            builder.HasOne(c => c.cargo)
                .WithMany(c => c.Items)
                .HasForeignKey(c => c.CargoId)
                .OnDelete(DeleteBehavior.Restrict);
            //باحذف هر ایتمی که در محموله هست نباید محموله حذف شود                
            #endregion
        }
    }
}
