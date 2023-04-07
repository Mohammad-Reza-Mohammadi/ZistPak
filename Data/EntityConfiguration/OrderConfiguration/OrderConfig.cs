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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();

            #region Other Property
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.OrderStar).IsRequired();
            builder.Property(c => c.IsFinaly).IsRequired();

            #endregion


        }
    }
}
