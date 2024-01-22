namespace PermissionBasedAuth.Constants
{
    public static class Permission
    {
        public static List<string> GenerateModuleClaimsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Index",
                $"Permissions.{module}.Details",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static List<string> GenerateAllModuleClaims()
        {
            var modules=Enum.GetValues(typeof(Modules));
            var allPermission=new List<string>();
            foreach(var module in modules)
            {
                allPermission.AddRange(GenerateModuleClaimsList(module.ToString()));
            }

            return allPermission;
        }

        public static class Order
        {
            public const string Index = "Permissions.Order.Index";
            public const string Details = "Permissions.Order.Details";
            public const string Create = "Permissions.Order.Create";
            public const string Edit = "Permissions.Order.Edit";
            public const string Delete = "Permissions.Order.Delete";
        }
    }
}
