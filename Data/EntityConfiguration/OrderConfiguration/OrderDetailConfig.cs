using Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityConfiguration.OrderConfiguration
{
    public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();

            #region Other Property
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            builder.Property(c => c.StarCargo).IsRequired();
            builder.Property(c => c.CountCargo).IsRequired();
            #endregion

            #region Navigation Property
            builder.HasOne(c=>c.Order)
                .WithMany(c=>c.orderDetails)
                .HasForeignKey(c=>c.OrderId);

            builder.HasOne(c => c.Cargo)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(c => c.CargoId);
            #endregion

        }
    }
}
