using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.SwaggerConfig.Permissions
{
    public static class Permissions
    {
        public static string Permission = "Permissions";

        public static class User
        {
            public const string GetAll = "Permissions.User.GetAll";
            public const string Delete = "Permissions.User.Delete";
            public const string GetUserById = "Permissions.User.GetUserById";
        }
        public static class Item
        {
            public const string AddItem = "Permissions.Item.AddItem";
            public const string UpdateItem = "Permissions.Item.UpdateItem";
            public const string Delete = "Permissions.Item.Delete";
        }

        public static class Cargo
        {
            public const string AddCargo = "Permissions.Cargo.AddCargo";
            public const string UpdateCargo = "Permissions.Cargo.UpdateCargo";
            public const string DeleteCargo = "Permissions.Cargo.DeleteCargo";

        }
        public static class Municipality
        {
            public const string AddCantractorPermissionById = "Permissions.Municipality.AddAllSoperviserPermission";
            public const string AddAllCantractorPermission = "Permissions.Municipality.AddSoperviserPermissionById";


        }

        // میتوان که برای چندین کنترلر یک مجوز ساخت که نیاز نباشد به کاربر برای هر کنترلر یک مجوز بدهیم

    }
}
      