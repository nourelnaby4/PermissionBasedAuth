namespace PermissionBasedAuth.Constants
{
    public static class Permission
    {
        public static List<string> GenerateModuleClaimsList(string module)
        {
            return new List<string>()
            {
                $"Permission.{module}.Index",
                $"Permission.{module}.Details",
                $"Permission.{module}.Create",
                $"Permission.{module}.Edit",
                $"Permission.{module}.Delete",
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
            public const string Index = "Permission.Order.Index";
            public const string Details = "Permission.Order.Details";
            public const string Create = "Permission.Order.Create";
            public const string Edit = "Permission.Order.Edit";
            public const string Delete = "Permission.Order.Delete";
        }
    }
}
