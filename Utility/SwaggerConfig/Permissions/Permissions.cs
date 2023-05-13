namespace Utility.SwaggerConfig.Permissions
{
    public static class Permissions
    {
        public static string Permission = "Permissions";


        public static class Admin
        {
            public const string admin = "Permissions.Admin.admin";

        }
        public static class User
        {
            public const string GetAll = "Permissions.User.GetAll";
            public const string Delete = "Permissions.User.Delete";
            public const string GetUserById = "Permissions.User.GetUserById";
            public const string AddAllSoperviserPermission = "Permissions.User.AddAllSoperviserPermission";
            public const string AddSoperviserPermissionById = "Permissions.User.AddSoperviserPermissionById";

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

        public static class Order
        {
            public const string AddToOrder = "Permissions.Order.AddToOrder";
            public const string ShowOrder = "Permissions.Order.ShowOrder";
            public const string DeleteFromOrder = "Permissions.Order.DeleteFromOrder";
            public const string UpdateOrederDetailInOreder = "Permissions.Order.UpdateOrederDetailInOreder";
        }

        // میتوان که برای چندین کنترلر یک مجوز ساخت که نیاز نباشد به کاربر برای هر کنترلر یک مجوز بدهیم

    }
}
      