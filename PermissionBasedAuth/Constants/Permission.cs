namespace PermissionBasedAuth.Constants
{
    public static class Permission
    {
        public static List<string> GenerateModuleClaimsList(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
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
    }
}
