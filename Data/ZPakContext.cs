using Data.EntityConfiguration;
using Entities.BaseEntityFolder;
using Entities.Cargo;
using Entities.Useres;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Utility;

namespace Data
{
    public class ZPakContext : IdentityDbContext<User,Role,int>
        //DbContext
    {
        #region ctor discription
        //اگر کانکشن استرینگ را در قستمت سرویس ها اضاف کردیم حتما باید سازنده پایین را صدا بزنیم  که 
        //options را بگیرد و به کلاس بیس بده
        //نکته : اگر از سازنده استفاده میکینم باید کانکشن استرینگ را در قسمت سرویس ها (DI) پیاده سازی کنیم 
        //و اگر از سازنده استفاده نمیکنیم باید از تابع OnConfiguring که پایین نوشتیم استفاده کنیم استفاده از هر دوروش احتمال ارور
        #endregion
        public ZPakContext(DbContextOptions options) : base(options)
        {            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("");
        //    base.OnConfiguring(optionsBuilder);
        //}

        #region OnModelCreatinng discription
        /// OnModelCreating یکی از متد های Dbcontext هست که وظیفه اش ساخت جدول از روی entityهایمان و ایجاد تغییرات fluentApi است
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //get assemply leyer of Entity and Data
            var entitiesAssembly = typeof(IEntity).Assembly;
      

            //Dynamicaly register all Entities that inherit from specific BaseType
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZPakContext).Assembly);

            /// <summary>
            /// Dynamicaly load all IEntityTypeConfiguration with Reflection
            /// </summary>
            /// var EntityConfigurationAssembly = typeof(CargoConfig).Assembly;
            /// modelBuilder.RegisterEntityTypeConfiguration(EntityConfigurationAssembly);
            /// modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);






        }



    }
}
