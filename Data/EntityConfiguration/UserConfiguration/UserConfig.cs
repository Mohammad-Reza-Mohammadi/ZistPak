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

            #region Inheritence Property
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.CreateDate).IsRequired();
            builder.Property(c => c.UpdateDate).IsRequired(false);
            #endregion

            #region Other Property
            builder.Property(c => c.PasswordHash).IsRequired();
            builder.Property(c => c.UserName).IsRequired();
            builder.Property(c => c.UserAge).IsRequired();
            builder.Property(c => c.UserPhoneNumber).IsRequired(false);
            builder.Property(c => c.UserEmail).IsRequired(false);
            builder.Property(c => c.UserImage).IsRequired(false);
            builder.Property(c => c.UserStar).IsRequired();
            builder.Property(c => c.UserToken).IsRequired(false);
            builder.Property(c => c.UserIsActive).IsRequired();
            builder.Property(c => c.UserGender).IsRequired();
            //builder.Property(c => c.UserRole).IsRequired();
            
            #endregion

            #region Owned Type Property
            builder.OwnsOne(c => c.UserFullName);

            builder.OwnsMany(c => c.UserAddresses, c =>
            {
                c.WithOwner().HasForeignKey(c => c.UserAddressOwnerId);
                c.HasKey(c => c.UserAddressId);
                c.Property(c => c.UserAddressTitle).IsRequired();
                c.Property(c => c.UserAddressCity).IsRequired();
                c.Property(c => c.UserAddressTown).IsRequired(false);
                c.Property(c => c.UserAddressStreet).IsRequired(false);
                c.Property(c => c.UserAddressPostalCode).IsRequired(false);
            });


            #endregion

            #region Navigation Property

            builder.HasOne(c => c.UserParentEmployee)
                .WithMany(c => c.UserChiledEmployee)
                .HasForeignKey(c => c.UserParetnEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(c=>c.UserParetnEmployeeId).IsRequired(false);
            //با حذف کارمند ها نباید پیمانکار حذف شوند
            #endregion
        }
    }
}
