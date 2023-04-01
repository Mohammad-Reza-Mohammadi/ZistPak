using Entities.User;
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
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            #region Base Property
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            #endregion

            #region Other Property
            builder.Property(c => c.HashPassword).IsRequired();
            builder.Property(c => c.Age).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired(false);
            builder.Property(c => c.Email).IsRequired(false);
            builder.Property(c => c.Image).IsRequired(false);
            builder.Property(c => c.Token).IsRequired(false);
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.Gender).IsRequired();
            builder.Property(c => c.Role).IsRequired();
            #endregion

            #region Owned Type Property
            builder.OwnsOne(c => c.FullName);
            //builder.Property(c => c.FullName.FirstName).IsRequired();
            //builder.Property(c => c.FullName.LastName).IsRequired(false);

            builder.OwnsMany(c => c.Addresses, c =>
            {

                c.WithOwner().HasForeignKey(c => c.OwnerId);
                c.HasKey(c => c.Id);
                c.Property(c => c.AddressTitle).IsRequired();
                c.Property(c => c.City).IsRequired();
                c.Property(c => c.Town).IsRequired(false);
                c.Property(c => c.Street).IsRequired(false);
                c.Property(c => c.PostalCode).IsRequired(false);
            });
            //builder.Property(c => c.Addresses).IsRequired(false);
            #endregion

            #region Navigation Property
            builder.HasOne(c => c.municipality)
                .WithMany(c => c.users)
                .HasForeignKey(c => c.municipalityId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.municipalityId).IsRequired(false);
            //باحذف کاربر نباید شهرداری حذف شود

            builder.HasOne(c => c.ParentEmployee)
                .WithMany(c => c.ChileEmployee)
                .HasForeignKey(c => c.ParetnEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            //با حذف کارمند ها نباید پیمانکار حذف شوند
            #endregion
        }
    }
}
